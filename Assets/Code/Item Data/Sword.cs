using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Triheroes.Code
{
    public class Sword : Weapon
    {
        public float Lenght = 10;
        public override WeaponType WeaponType => WeaponType.Sword;
        public SlashAttackSize slashSize;

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
