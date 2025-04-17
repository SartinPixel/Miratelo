using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class ac_return_weapon : action
    {

        [Depend]
        m_skin ms;

        SuperKey ReturnAnimation;

        protected override void BeginStep()
        {
            ms.PlayState ( ms.r_arm, ReturnAnimation, 0.1f, null, null, done );
        }

        public void Set ( SuperKey returnAnimation )
        {
            ReturnAnimation = returnAnimation;
        }

        void done ()
        {
            AppendStop ();
        }

    }
}