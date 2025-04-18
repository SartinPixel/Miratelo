using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class ac_has_active_weapon : condition
    {
        [Depend]
        m_equip me;
        protected override bool OnCheck()
        {
            return me.weaponUser != null;
        }
    }

    public class ac_check_active_weapon_type : condition
    {
        
        [Depend]
        m_equip me;

        WeaponType type;

        public ac_check_active_weapon_type ( WeaponType type )
        {
            this.type = type;
        }

        protected override bool OnCheck()
        {
            if ( me.weaponUser != null )
            {
                return me.weaponUser.WeaponBase.WeaponType == type;
            }
            return false;
        }
    }
}
