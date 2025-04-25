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
        m_state_player msp;

        protected override void BeginStep()
        {
            msp.SetState (mscch.ss);
            msp.Aquire (this);
        }

        protected override void Stop()
        {
            msp.Free (this);
        }
    }

}
