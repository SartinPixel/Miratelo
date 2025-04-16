using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class APhysic : CharacterData
    { 
        /// inspector paramater
        public float m;
        public float h;
        public float r;
        
        public bool Gravity = true;

        #if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + r / 2 * Vector3.up, r);
            Gizmos.DrawWireSphere(transform.position + ((h - r / 2) * Vector3.up), r);
            Gizmos.DrawLine(transform.position + r / 2 * Vector3.up + (r * Vector3.left), transform.position + ((h - r / 2) * Vector3.up) + (r * Vector3.left));
            Gizmos.DrawLine(transform.position + r / 2 * Vector3.up + (r * Vector3.right), transform.position + ((h - r / 2) * Vector3.up) + (r * Vector3.right));
            Gizmos.DrawLine(transform.position + r / 2 * Vector3.up + (r * Vector3.forward), transform.position + ((h - r / 2) * Vector3.up) + (r * Vector3.forward));
            Gizmos.DrawLine(transform.position + r / 2 * Vector3.up + (r * Vector3.back), transform.position + ((h - r / 2) * Vector3.up) + (r * Vector3.back));
        }
        #endif
    }
}