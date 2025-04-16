using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Triheroes.Code
{
    public abstract class Weapon : MonoBehaviour
    {
        public abstract WeaponType WeaponType {get;}
    }
}

public enum WeaponType {Sword, Bow};