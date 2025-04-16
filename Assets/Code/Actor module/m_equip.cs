using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    /*
    public class m_arm : state_box
    {
        List <Weapon> weapons;
        public Transform SwordPlace;
        public Transform BowPlace;

        public override void Create()
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
    }*/
}