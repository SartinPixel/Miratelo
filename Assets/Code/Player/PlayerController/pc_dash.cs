using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;
using UnityEngine.UI;

namespace Triheroes.Code
{
    public class pc_dash : action
    {
        [Depend]
        pm_master_controller pmc;
        [Depend]
        m_actor ma;
        [Depend]
        m_skin ms;

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
                    InputAxis = Vecteur.LDir ( Mathf.DeltaAngle ( ms.rotY.y, m_camera.o.mcts.rotY.y ) * Vector3.up, InputAxis );

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

                pmc.OverrideMaster ( this, ad );
                return false;
            }

            if ( ad.on )
            {
                if (ma.target != null)
                {
                if ( Mathf.Abs (Mathf.DeltaAngle ( ms.rotY.y ,Vecteur.RotDirectionY ( ms.Coord.position, ma.target.md.position ))) < 90 )
                ms.rotY = new Vector3(0, Mathf.MoveTowardsAngle(ms.rotY.y, Vecteur.RotDirectionY ( ms.Coord.position, ma.target.md.position ), Time.deltaTime * 720), 0);
                ms.SkinDir = Vecteur.LDir ( ms.rotY , ac_dash.Direction (ad.dashDirection));
                }
                if (Player.GetButtonUp ( BoutonId.Fire2 ))
                ad.DashEnd ();
            }
            return false;
        }
    }
}
