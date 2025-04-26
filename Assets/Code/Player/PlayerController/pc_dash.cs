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
        [Depend]
        m_actor ma;

        ac_dash ad;
        public override void Create()
        {
            ad = pmc.character.ConnectNode ( new ac_dash (direction.forward) );
        }

        protected override bool Step()
        {
            if ( Player.GetButtonDown (BoutonId.Fire2) &&!ad.on )
            {
                ad.dashDirection = direction.forward;

                if (ma.target != null)
                {
                    Vector3 InputAxis;
                    InputAxis = Player.GetAxis3().normalized;

                    if (Mathf.Abs(InputAxis.x) > Mathf.Abs(InputAxis.z))
                    {
                        if (InputAxis.x < 0)
                            ad.dashDirection = direction.left;
                        else
                            ad.dashDirection = direction.right;
                    }
                    else if (InputAxis.z < 0)
                        ad.dashDirection = direction.back;
                }

                pmc.OverrideMaster (this, ad);
            }
            return false;
        }
    }
}
