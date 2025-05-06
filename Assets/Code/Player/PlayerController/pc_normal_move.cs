using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class pc_normal_move : action
    {
        [Depend]
        pm_master_controller pmc;
        [Depend]
        m_standard_character_controller_host mscch;
        [Depend]
        m_ground_data mgd;
        [Depend]
        m_state_energy_auto msea;
        [Depend]
        m_stat_health msh;

        protected override void BeginStep()
        {
            pmc.SetDefaultMaster(this, mscch.ss);
        }

        protected override bool Step()
        {
            Vector3 InputAxis;
            InputAxis = Player.GetAxis3();

            mscch.cgm.tired = (msea.energy == 0);
            InputAxis = Vecteur.LDir(new Vector3(0, m_camera.o.mcts.rotY.y, 0), InputAxis) * 8f;

            UIDebug.PushText ( msh.HP.ToString () );

            if (mgd.onGround)
            {
                if (!mscch.cgm.tired)
                {
                    if (Player.GetButton(BoutonId.Fire2))
                        msea.energy -= Time.deltaTime;

                    mscch.cgm.Walk(InputAxis, Player.GetButton(BoutonId.Fire2) ? WalkFactor.sprint : Input.GetKey(KeyCode.X) ? WalkFactor.walk : WalkFactor.run);
                }
                else mscch.cgm.Walk(InputAxis, WalkFactor.tired);
            }
            else mscch.cf.MoveAir(InputAxis);

            return false;
        }
    }
}
