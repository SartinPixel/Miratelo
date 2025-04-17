using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class pc_sword : action, ICoreReceptor
    {
        [Depend]
        pm_master_controller pmc;

        ac_slash as1;

        public override void Create()
        {
            as1 = pmc.character.ConnectAction ( new ac_slash () );
        }


        protected override bool Step()
        {
            if ( Player.GetButtonDown (BoutonId.Fire1) )
                {
                if ( !as1.on )
                pmc.SetMaster (this, as1);
                else
                as1.ComboAppend ();
                }

            return false;
        }

        
        public void SelfFreed(node AquiredNode)
        {
            pmc.SetMaster ( pmc.previousMaster, pmc.previousState );
        }
    }
}