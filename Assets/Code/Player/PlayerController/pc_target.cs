using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class pc_target : action
    {
        [Depend]
        m_actor ma;
        public float Distance = 10;

        public pc_target ( float distance )
        {
            Distance = distance;
        }

        protected override bool Step()
        {
            if (Player.GetButtonDown (BoutonId.R))
            {
                ma.UnlockTarget ();
                ma.LockATarget ( ma.GetNearestFacedFoe ( Distance ) );
                
                if ( ma.target != null )
                m_camera.o.mcts.ChangeToTarget ();
            }
            
            if (Player.GetButtonDown (BoutonId.E))
            ma.UnlockTarget ();
            return false;
        }
    }
}