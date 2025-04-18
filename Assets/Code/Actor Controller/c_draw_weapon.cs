using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class c_draw_weapon : action
    {
        [Depend]
        m_skin ms;
        Weapon weapon;
        SuperKey DrawAnimation;

        protected override void BeginStep()
        {
            ms.PlayState ( ms.r_arm, DrawAnimation, 0.1f, null, null, done );
        }

        public void Set ( Weapon weapon, SuperKey drawAnimation )
        {
            this.weapon = weapon;
            DrawAnimation = drawAnimation;
        }

        void done ()
        {
            AppendStop ();
        }
    }
}
