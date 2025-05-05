using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixify;

namespace Triheroes.Code
{
    public class ac_idle : action
    {
        [Depend]
        m_standard_character_controller_host mscch;
        [Depend]
        m_state_stack mss;

        protected override void BeginStep()
        {
            mss.SetMainState ( mscch.ss );
            mss.AquireMainStatePlayer (this);
        }

        protected override void Stop()
        {
            mss.FreeMainStatePlayer (this);
        }
    }

}
