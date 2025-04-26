using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class ac_dash : action
    {
        static SuperKey DashAnimation (direction direction) => (direction == direction.forward)? AnimationKey.dash_forward : (direction == direction.right)? AnimationKey.dash_right:AnimationKey.dash_left;
        static Vector3 Direction ( direction direction ) => (direction == direction.forward)? Vector3.forward : (direction == direction.back)? Vector3.back:(direction == direction.right)? Vector3.right:Vector3.left;

        [Depend]
        m_capsule_character_controller mccc;
        [Depend]
        m_skin ms;

        public direction dashDirection;

        public ac_dash ( direction dashDirection )
        {
            this.dashDirection = dashDirection;
        }

        protected override void BeginStep()
        {
            mccc.Aquire ( this );

            ms.PlayState (0, DashAnimation (dashDirection),0.1f, DashEnd);
            ms.SkinMove = true;
            ms.SkinDir = Vecteur.LDir (ms.rotY, Direction (dashDirection));
        }

        protected override void Stop()
        {
            ms.SkinMove = false;
            mccc.Free ( this );
        }

        void DashEnd ()
        {
            AppendStop ();
        }

    }
}
