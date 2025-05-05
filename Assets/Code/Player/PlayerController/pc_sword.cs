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
        [Depend]
        m_slash_alert msa;

        ac_slash as1;
        ac_slash_parry_trajectile aspt;
        ac_slash_parry asp;

        public override void Create()
        {
            as1 = pmc.character.ConnectNode(new ac_slash());
            aspt = pmc.character.ConnectNode(new ac_slash_parry_trajectile());
            asp = pmc.character.ConnectNode(new ac_slash_parry());
        }

        protected override void BeginStep()
        {
            mta.Aquire(this);
            msa.Aquire(this);
        }

        protected override bool Step()
        {
            if (Player.GetButtonDown(BoutonId.Fire1))
            {
                if (pmc.overrideMaster == null)
                    pmc.OverrideMaster(this, as1);
                else if (as1.on)
                    as1.ComboAppend();
            }

            if (Player.GetButtonDown(BoutonId.F) && pmc.overrideMaster == null)
            {
                if (mta.Alert && !msa.Alert)
                trajectileParry ();

                else if (msa.Alert && !mta.Alert)
                parry ();

                else if (msa.Alert && mta.Alert)
                {
                    if (msa.timeLeft < mta.distance)
                        parry ();
                    else
                    trajectileParry ();
                }
            }
            return false;
        }

        void parry()
        {
            asp.Set(msa.parryKey);
            pmc.OverrideMaster(this, asp);
        }

        void trajectileParry()
        {
            aspt.Set(mta.incomingTrajectile);
            pmc.OverrideMaster(this, aspt);
        }

        protected override void Stop()
        {
            mta.Free(this);
            msa.Free(this);
        }
    }
}