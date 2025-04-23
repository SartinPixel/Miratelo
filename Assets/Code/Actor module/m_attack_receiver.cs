using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

// TO DO separate character and the attack receiver module to have their own collider
namespace Triheroes.Code
{
    public class m_attack_receiver : indexedmodule <m_attack_receiver>
    {
        [Depend]
        public m_actor ma;
        [Depend]
        public m_reactable mr;
        [Depend]
        public m_character_controller mcc;
        
        public ac_hit ah;

        public override void Create1()
        {
            mr.Clash = Clash;
            ah = character.ConnectAction ( new ac_hit() );
        }

        public void Clash ( Force force )
        {
            mcc.OverrideFocus ( ControllerKey.hit_normal );
        }
    }
}
