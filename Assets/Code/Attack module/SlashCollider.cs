using System;
using System.Collections.Generic;
using UnityEngine;

namespace Triheroes.Code
{
    public class SlashAttackCollider : MonoBehaviour
    {

        public BoxCollider Collider;
        public float Duration = 0.2f;
        public Action<Collision> OnCollisionDetected;

        void Awake()
        {
            Collider = gameObject.AddComponent<BoxCollider>();
            gameObject.AddComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            gameObject.layer = Vecteur.ATTACK;
        }

        void OnCollisionEnter(Collision Col)
        {
            if (gameObject.activeInHierarchy)
                OnCollisionDetected(Col);
        }

    }


    [Serializable]
    public struct SlashAttackSize
    {
        public float zOffset;
        public Vector3 Size;
    }
}
