using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class pc_dash : action, ICoreReceptor
    {
        [Depend]
        pm_master_controller pmc;

        ac_dash ad;
        public override void Create()
        {
            ad = pmc.character.ConnectAction ( new ac_dash (direction.forward) );
        }

        protected override bool Step()
        {
            if ( Player.GetButtonDown (BoutonId.Fire2) )
            {
                if ( !ad.on )
                pmc.SetMaster (this, ad);
            }
            return false;
        }

        public void SelfFreed(node AquiredNode)
        {
            pmc.SetMaster ( pmc.previousMaster, pmc.previousState );
        }
    }
}
