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
        public m_reaction_receiver mrr;
        [Depend]
        public m_character_controller mcc;
        [Depend]
        public m_state_stack mss;
        
        public ac_hit ah;

        public override void Create1()
        {
            mrr.Clash = Clash;
            ah = character.ConnectNode ( new ac_hit() );
        }

        public void Clash ( Force force )
        {
            if ( mss.SetState ( 1, ah ) )
            mss.AquireStatePlayer ( 1, this );
            //mcc.OverrideFocus ( ControllerKey.hit_normal );
        }
    }
}
