using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixify;

namespace Triheroes.Code
{
    public class ac_bow_shoot : action
    {
        [Depend]
        m_bow_user mbu;

        protected override void BeginStep()
        {
            mbu.ca.StartShoot ();
        }

        protected override bool Step()
        {
            return ! mbu.ca.isShooting;
        }
    }

    public class ac_aim_target : action
    {
        [Depend]
        m_arm_state mas;
        [Depend]
        m_bow_user mbu;
        [Depend]
        m_actor ma;
        [Depend]
        m_skin ms;

        public float DegreeDeltaPerSecond = 360;
        Quaternion rotDir;

        protected override void BeginStep()
        {
            mas.SetState ( mbu.ca );
            mas.Aquire (this);

            rotDir = Quaternion.Euler(ms.RotY);
        }

        protected override bool Step()
        {
            if (ma.target == null)
                return true;

            if (mas.state != mbu.ca)
                return true;

            Vector3 TargetPosition = ma.target.md.position + Vector3.up * (ma.target.md.h * 0.5f);
            rotDir = Quaternion.RotateTowards(rotDir, Vecteur.RotDirectionQuaternion(mbu.Weapon.BowString.position, TargetPosition), DegreeDeltaPerSecond * Time.deltaTime);

            mbu.ca.TurnToTargetRot(rotDir.eulerAngles);
            mbu.ca.Aim(rotDir.eulerAngles);
            return false;
        }

        protected override void Abort()
        {
            // TODO check if mas will be stopped automatically or need manual stop
            mas.Free (this);
        }
    }


    public class ac_bow_is_aiming : condition
    {
        [Depend]
        m_bow_user mbu;

        protected override bool OnCheck()
        {
            return mbu.state == StateKey.aim;
        }
    }
}
