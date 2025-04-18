using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class c_aim : action
    {
        [Depend]
        m_skin ms;
        [Depend]
        m_skin_produceral_bow mspb;
        [Depend]
        m_bow_user mbu;
        [Depend]
        m_standard_character_controller_host mscch;

        public bool isShooting {get; private set;}

        protected override void BeginStep()
        {
            if (!mbu.on)
            AppendStop ();

            mspb.Aquire ( this );

            if ( mbu.state == StateKey.zero )
            BeginAim ();
            else
            AppendStop ();
        }

        protected override void Stop()
        {
            if ( mbu.state == StateKey.aim )
            mbu.state = StateKey.zero;

            isShooting = false;
            
            ms.ControlledStop (ms.upper);
            mspb.isRotatingWithBow = m_skin_produceral_bow.isRotatingWithBowState.disabled;
            mspb.Free (this);
        }

        void BeginAim ()
        {
            mbu.state = StateKey.aim;
            ms.HoldState ( ms.upper, AnimationKey.begin_aim , 0.1f);
            mspb.isRotatingWithBow = m_skin_produceral_bow.isRotatingWithBowState.running;
            Aim ( ms.RotY );
        }

        public void Aim ( Vector3 Rotation )
        {
            mbu.RotY = Rotation;
            mspb.TargetRotY = Rotation;
        }

        void ReAim ()
        {
        ms.HoldState ( ms.upper, AnimationKey.begin_aim , 0.1f);
        mspb.isRotatingWithBow = m_skin_produceral_bow.isRotatingWithBowState.running;
        }

        void Shoot ()
        {
            mspb.isRotatingWithBow = m_skin_produceral_bow.isRotatingWithBowState.hold;
            isShooting = false;
        }

        public void StartShoot ()
        {
            if (on)
            {
            ms.PlayState(ms.upper, AnimationKey.start_shoot, 0.1f, ReAim, null, Shoot);
            isShooting = true;
            }
        }

        public void TurnToTargetRot ( Vector3 TargetRotDirection)
        {
            if (on)
            {
                if ( mscch.cgm.state != StateKey.idle || Mathf.Abs (Mathf.DeltaAngle(TargetRotDirection.y , ms.RotY.y)) > 20 )
                mscch.cgm.rotDir = Vecteur.LDir ( Vector3.up * TargetRotDirection.y, Vector3.forward);
            }
        }

        public void EndAim ()
        {
            AppendStop ();
        }
    }
}