using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class pm_master_controller : core
    {
        [Depend]
        m_state_stack mss;
        public action defaultMaster { private set; get; }
        public action defaultState { private set; get; }
        public action overrideMaster { private set; get; }

        protected override void OnFree()
        {
            if (defaultMaster != null && overrideMaster == null)
                mss.FreeMainStatePlayer(defaultMaster);
            if (overrideMaster != null)
                mss.FreeMainStatePlayer(overrideMaster);

            defaultMaster = null;
            defaultState = null;
            overrideMaster = null;
        }

        public void SetDefaultMaster(action newMaster, action state)
        {
            if (on)
            {
                if (overrideMaster == null)
                {
                    if (defaultMaster != null)
                    {
                        if (mss.stateIsOn(0))
                            mss.FreeMainStatePlayer(defaultMaster);
                    }
                }

                defaultMaster = newMaster;
                defaultState = state;

                if (overrideMaster == null)
                {
                    mss.SetMainState(state, true);
                    mss.AquireMainStatePlayer(defaultMaster);
                }
            }
        }

        public void Update()
        {
            if (overrideMaster != null && !mss.stateIsOn(0))
            {
                mss.SetMainState(defaultState,true);
                mss.AquireMainStatePlayer(defaultMaster);
                overrideMaster = null;
            }
        }

        public void OverrideMaster(action newOverrideMaster, action state, bool CanAbort = false)
        {
            if (on && overrideMaster == null)
            {
                mss.FreeMainStatePlayer(defaultMaster);

                overrideMaster = newOverrideMaster;

                mss.SetMainState(state, CanAbort);
                mss.AquireMainStatePlayer(newOverrideMaster);
            }
        }
    }
}