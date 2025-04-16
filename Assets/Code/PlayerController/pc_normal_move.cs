using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class pc_normal_move : action
    {
        [Depend]
        m_state_player msp;
        [Depend]
        m_standard_character_controller_host mscch;
        [Depend]
        m_ground_data mgd;

        protected override void BeginStep ()
        {
            msp.SetState (mscch.ss);
            msp.Aquire (this);
        }

        protected override bool Step()
        {
            Vector3 InputAxis;
            InputAxis = Player.GetAxis3();
            InputAxis = Vecteur.LDir (new Vector3(0, MainCamera.o.RotY.y, 0),InputAxis) * 6f;
            if (mgd.onGround)
            mscch.agm.Walk ( InputAxis, Player.GetButton (BoutonId.Fire2) ? WalkFactor.sprint : Input.GetKey (KeyCode.X) ? WalkFactor.walk : WalkFactor.run );
            else
            mscch.af.MoveAir ( InputAxis );

            return false;
        }

        protected override void Stop()
        {
            msp.Free (this);
        }
    }
}
