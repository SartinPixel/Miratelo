using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class pc_lateral_move : action
    {
        [Depend]
        pm_master_controller pmc;
        [Depend]
        m_standard_character_controller_host mscch;
        [Depend]
        m_ground_data mgd;

        protected override void BeginStep()
        {
            pmc.SetDefaultMaster (this, mscch.ss);
        }

        protected override bool Step()
        {
            Vector3 InputAxis;
            InputAxis = Player.GetAxis3();
            InputAxis = Vecteur.LDir ( m_camera.o.mcts.rotY.OnlyY (),InputAxis) * 6f;

            if (mgd.onGround)
            mscch.cgm.WalkLateral ( InputAxis );
            else
            mscch.cf.MoveAir ( InputAxis );

            return false;
        }
    }
}
