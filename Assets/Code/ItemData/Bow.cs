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
        public override SuperKey DefaultReturnAnimation => AnimationKey.return_bow;

        public Transform BowString;
        public TrajectilePaper TrajectilePaper;
        public float Speed = 30;
        public float XPower;
    }
}