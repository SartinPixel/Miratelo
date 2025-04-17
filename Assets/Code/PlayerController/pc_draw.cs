using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class pc_draw : action
    {
        [Depend]
        m_equip me;

        protected override void BeginStep()
        {
            
        }

        protected override bool Step()
        {
           if (Player.GetButtonDown (BoutonId.A))
            me.DrawWeapon ( 0 );

            return false;
        }
    }
}