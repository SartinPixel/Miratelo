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

        int ptrWeapon;
        bool pendingSwap;

        protected override bool Step()
        {
            if (Player.GetButtonDown(BoutonId.A))
            {
                if (me.ptrWeapon == -1)
                    me.DrawWeapon(ptrWeapon);
                else
                    {
                    me.ReturnWeapon();
                    pendingSwap = true;
                    }
            }

            if ( pendingSwap && me.weaponUser == null)
            {
                pendingSwap = false;
                ptrWeapon ++;
                if ( ptrWeapon == me.weapons.Count)
                ptrWeapon = 0;
                me.DrawWeapon(ptrWeapon);
            }

            return false;
        }
    }
}