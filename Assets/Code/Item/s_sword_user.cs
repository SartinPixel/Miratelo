using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixify;

namespace Triheroes.Code
{
    public class s_sword_user : s_weapon_user_standard<Sword>
    {
        public override term key => new term ("ssu");
        protected override int HandIndex => 1;
        protected override int AniLayer => ss.sword;
        protected override Quaternion DefaultRotation => Const.SwordDefaultRotation;
    }
}
