using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public abstract class m_weapon_user : core
    {
        public abstract Weapon WeaponBase { get; }
    }

    public abstract class m_weapon_user<T> : m_weapon_user where T : Weapon
    {

        [Depend]
        protected m_skin ms;

        public T Weapon { get; protected set; }
        public override Weapon WeaponBase => Weapon;

        public void SetWeapon(T weapon)
        {
            if (on)
            {
                OnFree();
                Weapon = weapon;
                OnAquire();
            }
            else
                Weapon = weapon;
        }
    }

    public abstract class m_weapon_user_standard<T> : m_weapon_user<T> where T : Weapon
    {
        Transform[] Hand;
        protected abstract int HandIndex { get; }
        protected abstract int AniLayer { get; }
        protected abstract Quaternion DefaultRotation { get; }

        public sealed override void Create()
        {
            Hand = character.GetComponent<ABase>().Skin.Hand;
        }

        protected sealed override void OnAquire()
        {
            if (Weapon)
            {
                Weapon.transform.SetParent( Hand[HandIndex] );
                Weapon.transform.localPosition = Vector3.zero;
                Weapon.transform.localRotation = DefaultRotation;
                // TODO Add user actor
                ms.ActivateSyncLayer(AniLayer);
                OnAquire1();
            }
            else
                Debug.LogError("Weapon user activated but no weapon attached to it");
        }

        protected sealed override void OnFree()
        {
            if (Weapon)
            {
                Weapon.transform.SetParent(null);
                // TODO remove user actor
                Weapon = null;
                ms.DisableSyncLayer(AniLayer);
            }
        }

        protected virtual void OnAquire1()
        { }
    }
}
