using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class Sword : Weapon
    {
        public float Lenght = 10;

        public m_reaction_receiver mrr;

        // setting
        public override WeaponType WeaponType => WeaponType.Sword;
        public override SuperKey DefaultDrawAnimation => AnimationKey.take_sword;
        public override SuperKey DefaultReturnAnimation => AnimationKey.return_sword;

        public SlashAttackSize slashSize;

        // TODO set this in editor
        void Awake ()
        {
            mrr = new m_reaction_receiver ();
            mrr.reactable = new r_metal ();
        }

        #if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine ( transform.position, transform.position + transform.TransformDirection ( Lenght * Vector3.forward ) );
            Matrix4x4 temp = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube ( new Vector3 (0,0,slashSize.zOffset),slashSize.Size );
            Gizmos.matrix = temp;
        }
        #endif
    }
}
