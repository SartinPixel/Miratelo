using System.Collections;
using System.Collections.Generic;
using Pixify;
using Pixify.Spirit;
using UnityEngine;

namespace Triheroes.Code
{
    // core command between idle - walk lateral
    // uses CharacterController physics
    // manages animations
    public class ac_ground_movement_lateral : motor
    {
        public override int Priority => Pri.def2nd;
        public override bool AcceptSecondState => true;

        [Depend]
        s_capsule_character_controller sccc; int key_ccc;
        [Depend]
        s_gravity_ccc sgc; int key_gc;

        [Depend]
        s_skin ss;
        [Depend]
        d_ground_data dgd;

        public term state;
        public Vector3 lateralDir;
        public Vector3 rotDir;

        bool firstFrame;

        protected override void Start()
        {
            if (firstFrame == true)
            {
                lateralDir = Vector3.zero;
                rotDir = Vecteur.LDir ( ss.rotY, Vector3.forward );
                ToIdle ();
                firstFrame = false;
            }
            else
            {
                if (state == StateKey.idle)
                ToIdle ();
                else ToLateral ();
            }

             key_ccc = Stage.Start ( sccc );
             key_gc = Stage.Start ( sgc );
        }

        protected override void Step()
        {
            Animation ();
            Rotation ();
            lateralDir = Vector3.zero;
        }

        protected override void Stop()
        {
            Stage.Stop (key_ccc);
            Stage.Stop (key_gc);
        }

        void Animation ()
        {
            // idle => lateral
            if ( state == StateKey.idle && lateralDir.sqrMagnitude >= 0.01f)
            {
                ToLateral ();
                Animation ();
                return;
            }
            // lateral => idle // lateral animation
            else if ( state == StateKey.run_lateral )
            {
                Vector3 relativeDir = Vecteur.LDir( new Vector3(0, 360 - ss.rotY.y, 0), lateralDir ).normalized;
                SetAnimationDirectionFloat(relativeDir.x, relativeDir.z);

                if (lateralDir.sqrMagnitude < 0.01f)
                    ToIdle();
            }
        }

        void Rotation ()
        {
            float RotYTarget = 0;
            if (rotDir.magnitude > 0)
                RotYTarget =  Vecteur.RotDirectionY ( Vector3.zero, rotDir);

            ss.rotY = new Vector3(0, Mathf.MoveTowardsAngle(ss.rotY.y, RotYTarget, Time.deltaTime * 720), 0);
        }

        void ToLateral ()
        {
            ss.PlayState ( 0, AnimationKey.run_lateral );
            state = StateKey.run_lateral;
        }

        void ToIdle ()
        {
            ss.PlayState (0, AnimationKey.idle,0.1f);
            state = StateKey.idle;
        }

        float dx; float dz;
        public void SetAnimationDirectionFloat ( float _dx,float _dz, float GravityPerSecond = 3 )
        {
            dx = Mathf.MoveTowards ( dx, _dx, GravityPerSecond * Time.deltaTime );
            dz = Mathf.MoveTowards ( dz, _dz, GravityPerSecond * Time.deltaTime );

            ss.Ani.SetFloat (Hash.dx,dx);
            ss.Ani.SetFloat (Hash.dz,dz);
        }

        public void WalkLateral ( Vector3 DirPerSecond )
        {
            if (on)
            {
                sccc.dir += Time.deltaTime * ground_movement.SlopeProjection (DirPerSecond, dgd.groundNormal);
                lateralDir += DirPerSecond;
            }
        }
    }
}
