using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class c_ground_movement : action
    {
        [Depend]
        m_capsule_character_controller mccc;
        [Depend]
        m_ground_data mgd;
        [Depend]
        m_skin_foot_ik msfi;
        [Depend]
        public m_skin ms;
        [Depend]
        m_standard_character_controller_host mscch;
        
        [Depend]
        m_reaction_receiver mrr;
        [Depend]
        m_hit mh;

        public SuperKey state;
        public Vector3 walkDir;
        public Vector3 rotDir;
        Vector3 walkLatDir;
        public float walkFactor;
        // TODO separate tired from groundmovement
        public bool tired;

        #region action
        protected override void BeginStep()
        {
            walkDir = Vector3.zero;
            walkLatDir = Vector3.zero;
            rotDir = Vecteur.LDir ( ms.rotY, Vector3.forward ) ;
            ToIdle ();
            mccc.Aquire (this);

            mrr.reactable.Hit = mh.HitSimple;
        }

        protected override bool Step()
        {
            WalkAnimation();
            WalkRotation();

            // fall check
            if (!mgd.onGround && mccc.verticalVelocity < 0)
            {
                mscch.ss.nextState = mscch.cf;
                return true;
            }

            walkDir = Vector3.zero;
            walkLatDir = Vector3.zero;

            return false;
        }

        protected override void Stop()
        {
            mccc.Free (this);
            ms.SkinMove = false;

            mrr.reactable.Hit = null;
        }
        #endregion

        #region skin/animation state main
        void WalkRotation()
        {
            float RotIntent = 0;
            if (rotDir.sqrMagnitude > 0)
                RotIntent = Vecteur.RotDirectionY (Vector3.zero, rotDir);

            // brake turn animation if rotation difference is to high // and is sprinting
            if (state == StateKey.sprint && Mathf.Abs(Mathf.DeltaAngle(ms.rotY.y, RotIntent)) > 120)
                RotationBrake();

            if (state == StateKey.brake && Mathf.Abs(Mathf.DeltaAngle(ms.rotY.y, RotIntent)) > 120)
                RotationBrake();

            ms.rotY = new Vector3(0, Mathf.MoveTowardsAngle(ms.rotY.y, RotIntent, Time.deltaTime * 720), 0);
        }

        void WalkAnimation()
        {
            // idle => run / lateral movement
            if (state == StateKey.idle)
            {
                if (walkDir.sqrMagnitude > 0)
                {
                    ToRun();
                    WalkAnimation();
                    return;
                }
                else if (walkLatDir.sqrMagnitude > 0)
                {
                    ToLateral();
                    WalkAnimation();
                    return;
                }
            }
            // march modulation => idle
            else if (state == StateKey.run || state == StateKey.walk || state == StateKey.sprint || state == StateKey.walk_tired )
            {
                if ( !WalkFactorCorrespondsToState(walkFactor, state) )
                    ToRun();

                if ( walkDir.sqrMagnitude == 0 )
                {
                    if (state == StateKey.sprint || (state == StateKey.run && ms.IsTransitioningFrom(0, AnimationKey.sprint)))
                        Brake();
                    else
                        ToIdle();
                }
            }
            // lateral movement => idle
            else if ( state == StateKey.walk_lateral )
            {
                Vector3 relativeDir = Vecteur.LDir( new Vector3(0, 360 - ms.rotY.y, 0), walkLatDir ).normalized;
                SetAnimationDirectionFloat(relativeDir.x, relativeDir.z);

                if (walkLatDir.sqrMagnitude == 0)
                    ToIdle();
            }
        }

        static bool WalkFactorCorrespondsToState(float factor, SuperKey state)
        {
            return ( factor == WalkFactor.run && state == StateKey.run) || (factor == WalkFactor.walk && state == StateKey.walk) || (factor == WalkFactor.sprint && state == StateKey.sprint) || (factor == WalkFactor.tired && state == StateKey.walk_tired );
        }

        #endregion

        #region ground movement main
        public void FallCheck()
        {

        }

        public void Walk ( Vector3 DirPerS, float WalkFactor = WalkFactor.run )
        {
            if (on)
            {
                // set character physic to move according to desired level
                walkDir += DirPerS;

                if ( walkDir.sqrMagnitude > 0 )
                rotDir = walkDir;
                walkFactor = WalkFactor;

                if (state != StateKey.brake_rotation)
                mccc.dir +=  Time.deltaTime/*a*/ * WalkFactor * SlopeProjection ( DirPerS, mgd.groundNormal );
            }
        }

        public void WalkLateral( Vector3 DirPerSecond)
        {
            if (on)
            {
                mccc.dir += Time.deltaTime/*a*/ * SlopeProjection ( DirPerSecond,mgd.groundNormal );
                walkLatDir += DirPerSecond * 0.5f;
            }
        }
        #endregion

        #region state transition
        void ToLateral ()
        {
            ms.PlayState ( 0, AnimationKey.run_lateral );
            state = StateKey.walk_lateral;
        }

        void ToIdle ()
        {
            ms.PlayState ( 0, tired? AnimationKey.idle_tired: AnimationKey.idle, 0.1f );
            state = StateKey.idle;
        }

        void ToRun ()
        {
            ms.PlayState( 0, (walkFactor == WalkFactor.tired) ? AnimationKey.walk_tired : (walkFactor == WalkFactor.walk) ? AnimationKey.walk : (walkFactor == WalkFactor.run) ? AnimationKey.run : AnimationKey.sprint,0.2f);
            state = (walkFactor == WalkFactor.tired)? StateKey.walk_tired : (walkFactor == WalkFactor.walk) ? StateKey.walk : (walkFactor == WalkFactor.run) ? StateKey.run : StateKey.sprint;
        }
        #endregion

        #region brake move
        void Brake ()
        {
            state = StateKey.brake;
            ms.SkinMove = true;
            ms.SkinDir = ms.Ani.transform.forward.normalized;
            ms.PlayState (0, AnimationKey.sprint_brake, 0.05f, BrakeEnd );
        }

        void BrakeEnd ()
        {
            ms.SkinMove = false;
            ToIdle ();
        }

        void RotationBrake ()
        {
            state = StateKey.brake_rotation;
            ms.SkinMove = false;
            ms.PlayState (0, AnimationKey.rotation_brake, 0.05f, RotationBrakeEnd );
        }

        void RotationBrakeEnd ()
        {
            ToRun ();
        }
        #endregion

        #region lateral movement skin ext
        float dx; float dz;
        public void SetAnimationDirectionFloat ( float _dx,float _dz, float GravityPerSecond = 3 )
        {
            dx = Mathf.MoveTowards ( dx, _dx, GravityPerSecond * Time.deltaTime );
            dz = Mathf.MoveTowards ( dz, _dz, GravityPerSecond * Time.deltaTime );

            ms.Ani.SetFloat (Hash.dx,dx);
            ms.Ani.SetFloat (Hash.dz,dz);
        }
        #endregion
        
        #region jump main
        SuperKey jumpAnimation => (state == StateKey.idle)? AnimationKey.jump : ( (msfi.DominantFoot == m_skin_foot_ik.FootId.left) ? AnimationKey.jump_left_foot : AnimationKey.jump_right_foot );
        SuperKey fallEndAnimation => (state == StateKey.idle)? AnimationKey.fall_end : ( (msfi.DominantFoot == m_skin_foot_ik.FootId.left) ? AnimationKey.fall_end_left_foot : AnimationKey.fall_end_right_foot );
        public void JumpOnce ( float JumpHeight, float WalkLevel )
        {
            if (on)
            {
            mccc.verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y * mccc.m);
            walkFactor = WalkLevel;
            ms.PlayState(0, jumpAnimation, 0.05f );
            mscch.cf.AnimationFallEnd = fallEndAnimation;
            
            state = StateKey.jump;
            }
        }

        public void JumpStep( float UnpreciseRatio )
        {
            if (on)
            {
            mccc.verticalVelocity += Mathf.Sqrt(-2f * Physics.gravity.y * mccc.m) * Time.deltaTime/*a*/ * UnpreciseRatio;
            }
        }

        #endregion


        private static Vector3 SlopeProjection ( Vector3 Dir,Vector3 GroundNormal ) => Vector3.ProjectOnPlane (Dir, GroundNormal).normalized * Dir.magnitude;
    }

    public static class WalkFactor
    {
        public const float idle = 0;
        public const float walk = 0.15f;
        public const float tired = 0.14f;
        public const float run = 1;
        public const float sprint = 2;
    }
}