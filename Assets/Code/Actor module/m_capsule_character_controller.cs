using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixify;

namespace Triheroes.Code
{
    public class m_capsule_character_controller : core
    {
        [Depend]
        public m_ground_data mgd;
        [Depend]
        m_dimension md;

        public bool gravity;
        public float verticalVelocity;
        public Vector3 velocityDir;
        public Vector3 dir;
        public float m;


        public Transform Coord;
        public CharacterController CCA;
        

        public sealed override void Create()
        {
            Coord = character.transform;
            CCA = character.gameObject.AddComponent <CharacterController> ();

            APhysic P = character.GetComponent <APhysic> ();
            if (P)
            {
                CCA.skinWidth = 0.01f;
                CCA.radius = P.r;
                CCA.height = P.h;
                CCA.center = new Vector3(0, P.h / 2);
                gravity = P.Gravity;
                m = P.m;
            }
            else
            {
                Debug.LogWarning ("No CPhysic Data on this character, m_capsule_character_controller needs it", character.gameObject);
            }
        }

        protected override void OnAquire()
        {
            md.r = CCA.radius;
            md.h = CCA.height;
        }

        protected override void OnFree()
        {
            verticalVelocity = 0;
            velocityDir = Vector3.zero;
            dir = Vector3.zero;
        }
    }
}