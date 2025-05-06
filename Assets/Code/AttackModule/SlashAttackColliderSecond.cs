using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Triheroes.Code
{
    public class SlashAttackColliderSecond : MonoBehaviour
    {
        public BoxCollider Collider;

        void Awake()
        {
            Collider = gameObject.AddComponent<BoxCollider>();
            gameObject.AddComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            gameObject.layer = Vecteur.ATTACK;
        }
    }
}
