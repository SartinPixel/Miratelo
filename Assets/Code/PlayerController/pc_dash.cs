using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class pc_dash : action
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
                pmc.OverrideMaster (this, ad);
            }
            return false;
        }
    }
}
