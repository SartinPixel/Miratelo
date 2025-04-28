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

        protected override void BeginStep ()
        {
            pmc.SetDefaultMaster (this, mscch.ss);
        }

        protected override bool Step()
        {
            Vector3 InputAxis;
            InputAxis = Player.GetAxis3();
            InputAxis = Vecteur.LDir (new Vector3(0, m_camera.o.mcts.rotY.y, 0),InputAxis) * 6f;
            if (mgd.onGround)
            mscch.cgm.Walk ( InputAxis, Player.GetButton (BoutonId.Fire2) ? WalkFactor.sprint : Input.GetKey (KeyCode.X) ? WalkFactor.walk : WalkFactor.run );
            else
            mscch.cf.MoveAir ( InputAxis );

            return false;
        }
    }
}
