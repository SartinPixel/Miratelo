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
        public action previousState {private set; get;}

        protected override void OnAquire()
        {
        }

        protected override void OnFree()
        {
            if (master != null)
            msp.Free (master);

            master = null;
            previousMaster = null;
            previousState = null;
        }

        public void SetMaster (action master, action state)
        {
            if (on)
            {
                if (this.master != null)
                {
                previousMaster = master;
                previousState = msp.state;
                msp.Free (master);
                }

                this.master = master;
                msp.SetState ( state );
                msp.Aquire (master);
            }
        }
    }
}