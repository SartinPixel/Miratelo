using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixify;

namespace Triheroes.Code
{
    public class pc_active_master_controller : action
    {

        [Depend]
        pm_master_controller pmc;
        [Depend]
        m_actor ma;

        protected override void BeginStep()
        {
            pmc.Aquire (this);
            //TODO remove this and let gamemaster manage camera actors
            m_camera.o.mcts.C = ma;
        }

        protected override bool Step()
        {
            pmc.Update ();
            return false;
        }

        protected override void Stop()
        {
            pmc.Free (this);
        }

    }
}