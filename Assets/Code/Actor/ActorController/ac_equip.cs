using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class ac_equip_weapon : action
    {
        [Depend]
        m_equip me;

        int weaponIndex;

        public ac_equip_weapon ( int id )
        {
            weaponIndex = id;
        }

        protected override void BeginStep()
        {
            me.DrawWeapon ( weaponIndex );
        }

        protected override bool Step()
        {
            if ( me.ptrWeapon == weaponIndex && me.weaponUser != null )
            return true;
            return false;
        }
    }

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
