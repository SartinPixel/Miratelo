using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixify;

namespace Triheroes.Code
{
    // core to command between idle - walk - run - sprint - brake - brake rotation
    // uses CharacterController physics
    // need ground data for normal projection
    // also manage animations
    // does not change to fall movement when not on ground, this must be manually added in behavior trees
    [RegisterAsBase]
    public class c_ground_movement_complex : controller
    {
        [Depend]
        m_capsule_character_controller mccc;
        [Depend]
        m_skin ms;
        [Depend]
        m_ground_data mgd;

        public SuperKey state;
        /// <summary>
        /// composite walk direction for the character
        /// </summary>
        public Vector3 walkDir;
        /// <summary>
        /// direction the character is commanded to face
        /// </summary>
        public Vector3 rotDir;
        /// <summary>
        /// walk factor of the movement, used to interpolate between idle and walk, the character's speed is multiplied by this factor
        /// </summary>
        public float walkFactor;

        protected override void OnAquire()
        {
            walkDir = Vector3.zero;
            rotDir = Vecteur.LDir ( ms.rotY, Vector3.forward );
            ToIdle ();
            
            mccc.Aquire (this);
        }

        public override void Main()
        {
            Animation ();
            Rotation ();
            ResetDir ();
        }

        void ResetDir () => walkDir = Vector3.zero;

        protected override void OnFree()
        {
            mccc.Free (this);
            ms.allowMoving = false;
        }

        void Animation()
        {
            // idle => run
            if (state == StateKey.idle && (walkDir.magnitude > 0.01f))
                {
                    ToRun();
                    Animation();
                    return;
                }
            // run modulation ( walk - run - sprint ) => idle
            else if (state == StateKey.run || state == StateKey.sprint || state == StateKey.walk)
            {
                if (!FactorCorrespondToState(walkFactor, state))
                ToRun();

                if (walkDir.magnitude < 0.01f)
                {
                    if (state == StateKey.sprint || (state == StateKey.run && ms.IsTransitioningFrom(0, AnimationKey.sprint)))
                    Brake ();
                    else
                    ToIdle ();
                }
            }
        }

        void ToIdle ()
        {
            ms.PlayState (0, AnimationKey.idle,0.1f);
            state = StateKey.idle;
        }

        void ToRun ()
        {
            ms.PlayState (0, (walkFactor == WalkFactor.walk) ? AnimationKey.walk : (walkFactor == WalkFactor.run) ? AnimationKey.run : AnimationKey.sprint,0.2f);

            state =  (walkFactor == WalkFactor.walk) ? StateKey.walk : (walkFactor == WalkFactor.run) ? StateKey.run : StateKey.sprint;
        }

        void Brake ()
        {
            ms.PlayState (0, AnimationKey.sprint_brake,0.05f, BrakeEnd);
            state = StateKey.brake;
            ms.allowMoving = true;
            ms.SkinDir = ms.Ani.transform.forward.normalized;
        }

        void BrakeEnd ()
        {
            ms.allowMoving = false;
            ToIdle ();
        }

        static bool FactorCorrespondToState(float factor, SuperKey state)
        {
            return (factor == WalkFactor.walk && state == StateKey.walk) || (factor == WalkFactor.run && state == StateKey.run) || (factor == WalkFactor.sprint && state == StateKey.sprint);
        }

        void Rotation ()
        {
            float RotYTarget = 0;
            if (rotDir.magnitude > 0)
                RotYTarget =  Vecteur.RotDirectionY ( Vector3.zero, rotDir);

            // brake turn animation if rotation difference is too high // and is sprinting
            if (state == StateKey.sprint && Mathf.Abs(Mathf.DeltaAngle(ms.rotY.y, RotYTarget)) > 120)
                RotationBrake();

            if (state == StateKey.brake && Mathf.Abs(Mathf.DeltaAngle(ms.rotY.y, RotYTarget)) > 120)
                RotationBrake();

            ms.rotY = new Vector3(0, Mathf.MoveTowardsAngle(ms.rotY.y, RotYTarget, Time.deltaTime * 720), 0);
        }

        void RotationBrake()
        {
            ms.PlayState (0, AnimationKey.rotation_brake,0.05f, RotationBrakeEnd);
            state = StateKey.brake_rotation;
            ms.allowMoving = false;
        }

        void RotationBrakeEnd()
        {
            ToRun ();
        }

        private Vector3 SlopeProjection ( Vector3 Dir,Vector3 GroundNormal ) => Vector3.ProjectOnPlane (Dir, GroundNormal).normalized * Dir.magnitude;

        #region public methods

        public void Walk (Vector3 DirPerSecond, float WalkFactor = WalkFactor.run)
        {
            if (on)
            {
                walkDir += DirPerSecond;
                walkFactor = WalkFactor;

                if (walkDir.magnitude > 0.01f)
                rotDir = walkDir.normalized;

                if (state!=StateKey.brake_rotation)
                mccc.dir += Time.deltaTime * walkFactor * SlopeProjection (DirPerSecond, mgd.groundNormal);
            }
        }
        #endregion

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
