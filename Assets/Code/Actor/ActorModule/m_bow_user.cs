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

        public c_aim ca {private set; get;}

        public Vector3 rotY;
        public SuperKey state = StateKey.zero;

        protected override void Create1()
        {
            ca = character.ConnectNode (new c_aim());
        }
        protected override void OnAquire1()
        {
            state = StateKey.zero;
        }
    }
}