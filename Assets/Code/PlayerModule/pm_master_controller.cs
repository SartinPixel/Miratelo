using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class pm_master_controller : core
    {
        [Depend]
        m_state_player msp;
        action master;
        public action previousMaster {private set; get;}

        protected override void OnAquire()
        {
        }

        protected override void OnFree()
        {
            if (master != null)
            msp.Free (master);

            master = null;
            previousMaster = null;
        }

        public void SetDefaultMaster (action master, action state)
        {
            if (master != null)
            {
            msp.Free (master);
            previousMaster = master;
            }

            this.master = master;
            msp.SetState ( state );
            msp.Aquire (master);
        }
    }
}