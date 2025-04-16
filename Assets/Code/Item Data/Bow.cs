using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class Bow : Weapon
    {
        public override WeaponType WeaponType => WeaponType.Bow;

        public override SuperKey DefaultDrawAnimation => AnimationKey.take_bow;
    }
}
