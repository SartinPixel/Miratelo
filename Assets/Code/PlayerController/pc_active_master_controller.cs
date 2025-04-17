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

        protected override void BeginStep()
        {
            pmc.Aquire (this);
        }

        protected override void Stop()
        {
            pmc.Free (this);
        }

    }
}