using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class m_equip : module, ICoreReceptor
    {
        [Depend]
        m_arm_state mas;
        [Depend]
        m_sword_user msu;

        List <Weapon> weapons;
        int ptrWeapon = -1;
        public m_weapon_user weaponUser;

        public Transform SwordPlace;
        public Transform BowPlace;

        public ac_draw_weapon adw;

        public override void Create()
        {
            InitialSetting ();

            adw = character.ConnectAction ( new ac_draw_weapon() );
        }

        void AquireWeapon ( Weapon weapon )
        {
            weapons.Add ( weapon );

            if (weapon is Sword)
            weapon.transform.SetParent (SwordPlace);
            if (weapon is Bow)
            weapon.transform.SetParent (BowPlace);
            
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
        }

        void InitialSetting ()
        {
            ABase aBase = character.GetComponent <ABase> ();
            SwordPlace = aBase.Skin.SwordPlace;
            BowPlace = aBase.Skin.BowPlace;

            weapons = new List<Weapon> ();
            if ( aBase.AttachedWeapon[0] )
            AquireWeapon ( aBase.AttachedWeapon[0] );
            if ( aBase.AttachedWeapon[1] )
            AquireWeapon ( aBase.AttachedWeapon[1] );
        }
        
        public void DrawWeapon ( int id )
        {
            if ( weaponUser == null && !adw.on && weapons [id] )
            {
            adw.Set ( weapons [id], weapons [id].DefaultDrawAnimation );
            mas.SetState (adw);
            mas.Aquire (this);
            ptrWeapon = id;
            }
        }

        void StartWeaponUser ()
        {
            weaponUser =  GetCorrespondingWeaponUser ( weapons [ptrWeapon].WeaponType );
            weaponUser.SetWeaponBase ( weapons [ptrWeapon] );
            weaponUser.Aquire (this);
        }

        public void SelfFreed(node AquiredNode)
        {
            if (AquiredNode == mas && mas.state == adw)
            StartWeaponUser ();
        }

        m_weapon_user GetCorrespondingWeaponUser ( WeaponType Wt )
        {
            switch (Wt)
            {
                case WeaponType.Sword: return msu;
            }
            return null;
        }
    }
}