using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class m_bow_user : m_weapon_user_standard<Bow>
    {
        protected override int HandIndex => 0;
        protected override int AniLayer => ms.bow;
        protected override Quaternion DefaultRotation => Const.BowDefaultRotation;

        public ac_aim aa {private set; get;}

        public Vector3 RotY;
        public SuperKey state = StateKey.zero;

        protected override void Create1()
        {
            aa = character.ConnectAction (new ac_aim());
        }
        protected override void OnAquire1()
        {
            state = StateKey.zero;
        }
    }
}