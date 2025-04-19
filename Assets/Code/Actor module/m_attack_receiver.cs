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

        public override void Create1()
        {
            mr.Clash = Clash;
        }

        public void Clash ( Force force )
        {
            Debug.Log ("hit");
        }
    }
}
