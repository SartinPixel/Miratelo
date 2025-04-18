using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public sealed class m_equip : module, ICoreReceptor
    {
        [Depend]
        m_arm_state mas;
        [Depend]
        m_sword_user msu;
        [Depend]
        m_bow_user mbu;

        public List <Weapon> weapons {private set; get;}
        public int ptrWeapon {private set; get;} = -1;
        public m_weapon_user weaponUser;

        public Transform SwordPlace;
        public Transform BowPlace;

        public c_draw_weapon_animation cdwa;
        public c_return_weapon_animation crwa;

        public override void Create()
        {
            InitialSetting ();

            cdwa = character.ConnectAction ( new c_draw_weapon_animation() );
            crwa = character.ConnectAction ( new c_return_weapon_animation() );
        }

        void AquireWeapon ( Weapon weapon )
        {
            weapons.Add ( weapon );
            AttachWeapon ( weapon );
        }

        void AttachWeapon (Weapon weapon)
        {
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
            if ( weaponUser == null && !mas.on && weapons [id] )
            {
            cdwa.Set ( weapons [id], weapons [id].DefaultDrawAnimation );
            mas.SetState (cdwa);
            mas.Aquire (this);
            ptrWeapon = id;
            }
        }

        public void ReturnWeapon ()
        {
            if ( weaponUser != null && !mas.on )
            {
                crwa.Set (weapons [ptrWeapon].DefaultReturnAnimation);
                mas.SetState (crwa);
                mas.Aquire (this);
            }
        }

        public void SelfFreed(node AquiredNode)
        {
            if (AquiredNode == mas)
            {
            if (mas.state == cdwa)
            StartWeaponUser ();
            else if (mas.state == crwa)
            ReturnWeaponDone ();
            }
        }

        void ReturnWeaponDone ()
        {
            StopWeaponUser ();
            AttachWeapon ( weapons [ptrWeapon] );
            ptrWeapon = -1;
        }

        void StartWeaponUser ()
        {
            weaponUser =  GetCorrespondingWeaponUser ( weapons [ptrWeapon].WeaponType );
            weaponUser.SetWeaponBase ( weapons [ptrWeapon] );
            weaponUser.Aquire (this);
        }

        void StopWeaponUser ()
        {
            weaponUser.Free (this);
            weaponUser = null;
        }

        m_weapon_user GetCorrespondingWeaponUser ( WeaponType Wt )
        {
            switch (Wt)
            {
                case WeaponType.Sword: return msu;
                case WeaponType.Bow: return mbu;
            }
            return null;
        }
    }
}