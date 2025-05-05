using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class m_hit : module
    {
        [Depend]
        public m_character_controller mcc;
        [Depend]
        public m_state_stack mss;
        
        ac_hit ah;
        public Force LastAttack;

        public override void Create()
        {
            ah = character.ConnectNode ( new ac_hit() );
        }

        public void HitSimple ( Force force )
        {
            LastAttack = force;
            mss.SetState (1,ah);
            mss.AquireStatePlayer (1, this);
        }

        public void HitKnocked ( Force force )
        {
            LastAttack = force;
            mcc.OverrideFocus ( ControllerKey.hit_knocked_out );
        }
    }
}