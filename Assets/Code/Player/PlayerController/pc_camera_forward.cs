using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class pc_camera_forward : action
    {
        
        [Depend]
        m_actor ma;
        protected override bool Step()
        {
            if ( Player.GetButtonDown (BoutonId.R) )
            {
                if ( ma.target == null )
                m_camera.o.mcts.ChangeToLookFront ();
            }
            
            if ( Player.GetButtonUp (BoutonId.R) )
            {
                m_camera.o.mcts.ChangeToTps ();
            }

            return false;
        }
    }
}
