using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public abstract class Weapon : MonoBehaviour
    {
        public abstract WeaponType WeaponType {get;}
        public abstract SuperKey DefaultDrawAnimation {get;}
        public abstract SuperKey DefaultReturnAnimation {get;}
    }
}

public enum WeaponType {Sword, Bow};