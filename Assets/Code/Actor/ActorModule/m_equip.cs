using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    // TODO IMPORTANT don't allow player to attack while drawing weapon
    public sealed class m_equip : module, ICoreFeedback
    {
        [Depend]
        m_state_stack mss;
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

            cdwa = character.ConnectNode ( new c_draw_weapon_animation() );
            crwa = character.ConnectNode ( new c_return_weapon_animation() );
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
            if ( weaponUser == null && !mss.stateIsOn(1) && weapons [id] )
            {
            cdwa.Set ( weapons [id], weapons [id].DefaultDrawAnimation );
            mss.SetState (1,cdwa,true);
            mss.AquireStatePlayer (1,this);
            ptrWeapon = id;
            }
        }

        public void ReturnWeapon ()
        {
            if ( weaponUser != null && !mss.stateIsOn (1) )
            {
                crwa.Set (weapons [ptrWeapon].DefaultReturnAnimation);
                mss.SetState (1,crwa,true);
                mss.AquireStatePlayer (1,this);
            }
        }

        
        public void AquiredNodeStopped(node AquiredNode)
        {
            if ( AquiredNode == mss.GetStatePlayer(1) )
            {
            if (mss.GetState (1) == cdwa)
            StartWeaponUser ();
            else if (mss.GetState (1) == crwa)
            ReturnWeaponDone ();
            }
        }

        
        public void AquiredNodeAborted(node AquiredNode)
        {
            if ( AquiredNode == mss.GetStatePlayer(1) )
            {
            if (mss.GetState (1) == cdwa)
            ptrWeapon = -1;
            }
        }

        public void AquiredNodeFreed(node AquiredNode)
        {
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