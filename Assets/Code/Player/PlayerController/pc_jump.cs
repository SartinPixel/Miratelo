using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class pc_jump : action
    {
        [Depend]
        m_standard_character_controller_host mscch;
        [Depend]
        m_ground_data mgd;

        bool IsJumping;
        float JumpTimeHeld;

        protected override bool Step()
        {
            if (Player.GetButtonDown(BoutonId.Jump) && IsJumping == false && mgd.onGround)
            {
                IsJumping = true;
                JumpTimeHeld = 1;
                mscch.cgm.JumpOnce(3, Player.GetButton(BoutonId.Fire2) ? WalkFactor.sprint : WalkFactor.run);
            }
            if (IsJumping == true && JumpTimeHeld >= 0)
            {
                JumpTimeHeld -= Time.deltaTime;
                mscch.cgm.JumpStep(1.5f);
            }
            if (Player.GetButtonUp(BoutonId.Jump) && IsJumping)
            {
                IsJumping = false;
            }
            return false;
        }
    }
}
