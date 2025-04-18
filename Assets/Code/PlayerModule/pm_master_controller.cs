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
        public action defaultMaster {private set; get;}
        public action defaultState {private set; get;}
        public action overrideMaster {private set; get;}

        protected override void OnFree()
        {
            if (defaultMaster != null && overrideMaster == null)
            msp.Free (defaultMaster);
            if ( overrideMaster != null )
            msp.Free (overrideMaster);

            defaultMaster = null;
            defaultState = null;
            overrideMaster = null;
        }

        public void SetDefaultMaster (action newMaster, action state)
        {
            if (on)
            {
                if ( overrideMaster == null )
                {
                    if (defaultMaster != null)
                    {
                    if (msp.on)
                    msp.Free (defaultMaster);
                    }
                }

                defaultMaster = newMaster;
                defaultState = state;

                if ( overrideMaster == null )
                {
                msp.SetState ( state );
                msp.Aquire ( defaultMaster );
                }
            }
        }

        public void Update ()
        {
            if ( overrideMaster != null && !msp.on)
            {
                msp.SetState ( defaultState );
                msp.Aquire ( defaultMaster );
                overrideMaster = null;
            }
        }

        public void OverrideMaster (action newOverrideMaster, action state)
        {
            if (on && overrideMaster == null )
            {
            msp.Free (defaultMaster);

            overrideMaster = newOverrideMaster;

            msp.SetState (state);
            msp.Aquire ( newOverrideMaster );
            }
        }
    }
}