using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class pc_sword : action
    {
        [Depend]
        pm_master_controller pmc;
        [Depend]
        m_trajectile_alert mta;

        ac_slash as1;
        ac_slash_parry_trajectile aspt;

        public override void Create()
        {
            as1 = pmc.character.ConnectAction ( new ac_slash () );
            aspt = pmc.character.ConnectAction ( new ac_slash_parry_trajectile () );
        }

        protected override void BeginStep()
        {
            mta.Aquire (this);
        }

        protected override bool Step()
        {
            if ( Player.GetButtonDown (BoutonId.Fire1) )
                {
                if ( pmc.overrideMaster == null )
                pmc.OverrideMaster (this, as1);
                else if (as1.on)
                as1.ComboAppend ();
                }

            if ( Player.GetButtonDown (BoutonId.F) )
            {
                if ( mta.Alert && pmc.overrideMaster == null )
                {
                aspt.SetTrajectile ( mta.incomingTrajectile );
                pmc.OverrideMaster ( this, aspt );
                }
            }
                
            return false;
        }

        protected override void Stop()
        {
            mta.Free (this);
        }
    }
}