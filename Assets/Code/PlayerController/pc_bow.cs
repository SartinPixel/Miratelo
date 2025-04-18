using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;
using UnityEngine.UI;

namespace Triheroes.Code
{
    public class pc_bow : action
    {
        [Depend]
        m_arm_state mas;
        [Depend]
        m_bow_user mbu;

        protected override bool Step()
        {
            if (mbu.ca.on)
            {
                Vector3 RotDirection = Vecteur.RotDirection ( mbu.Weapon.BowString.position, MainCamera.PointScreenCenter( mbu.character.transform ) );
                mbu.ca.Aim ( RotDirection );
                
                mbu.ca.TurnToTargetRot ( RotDirection );

                if (Player.GetButtonDown (BoutonId.Fire3))
                {
                mbu.ca.EndAim ();
                return false;
                }
                else if (Player.GetButtonDown (BoutonId.Fire1))
                mbu.ca.StartShoot ();
            }
            else if ( !mas.on && Player.GetButtonDown (BoutonId.Fire3) )
            {
                mas.SetState ( mbu.ca );
                mas.Aquire (this);
            }
            return false;
        }

        protected override void Stop()
        {
            // TODO check if mas will be stopped automatically or need manual stop
        }
    }
}