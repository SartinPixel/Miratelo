using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class m_sword_user : m_weapon_user_standard<Sword>
    {
        protected override int HandIndex => 1;
        protected override int AniLayer => ms.sword;
        protected override Quaternion DefaultRotation => Const.SwordDefaultRotation;

        public SuperKey state = StateKey.zero;

        // Sword user slashes
        public Quaternion[] SlashRot;
        public Vector3[] SlashPos;

        
        public static readonly SuperKey[] SlashKeys = { AnimationKey.slash_0, AnimationKey.slash_1, AnimationKey.slash_2 };

        protected override void Create1()
        {   
            var A = character.GetComponent<ABase>();
            var AniExt = A.Skin.AniExt;
            var AniExtStatesExt = A.Skin.StateExt;

            SlashRot = new Quaternion[3];
            SlashPos = new Vector3[3];

            for (int i = 0; i < SlashPos.Length; i++)
            {
                SlashPos [i] = A.Skin.AniExt.StatesExt.ForceGet ( SlashKeys[i] ).v3 + AniExtStatesExt.Find ( x => x.Key == SlashKeys[i]).v3;
                SlashRot [i] = A.Skin.AniExt.StatesExt.ForceGet ( SlashKeys[i] ).q;
            }
        }

        protected sealed override void OnAquire1 ()
        {
            state = StateKey.zero;
        }
    }
}
