using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Triheroes.Code
{
    //TODO: not use monobehavior for main camera, use character module instead
    // monobehavior for fast testing purpose only
    public class MainCamera : MonoBehaviour
    {
        public static MainCamera o;

        public Transform CameraPivot;
        public Camera Cam;

        Animator Ani;
        Transform Coord;

        // main target character // TODO: refractor, should not be asigned manually
        public Transform C;
        // TODO: should not be asigned manually
        public float Distance, Height;

        [HideInInspector]
        [NonSerialized]
        public Vector3 RotY;

        Ray ScreenRay;

        void Awake ()
        {
            o = this;
            Ani = GetComponent <Animator> ();
            Coord = transform;
        }

        
        public void LateUpdate()
        {
            if (C)
            {
                //rotate according to mouse
                RotY.y += Player.DeltaMouse().x * 3;
                RotY.x -= Player.DeltaMouse().y * 3;
                RotY.x = Mathf.Clamp(RotY.x, -65, 65);
                TpsRotYSmooth();

                // Update ScreenRay
                ScreenRay.origin = Coord.position;
                ScreenRay.direction = Coord.forward;
            }
        }

        void TpsRotYSmooth()
        {
            float RayDistance = Distance;

            if (Physics.Raycast(Coord.position, CameraPivot.TransformDirection(Vector3.back), out RaycastHit hit, Distance, Vecteur.Solid))
                RayDistance = hit.distance - 0.25f;

            Coord.position = C.transform.position + Height * Vector3.up;
            Coord.rotation = Quaternion.Euler(RotY);
            CameraPivot.transform.localPosition = Vector3.back * RayDistance;
        }

        // public methods
        public static Vector3 PointScreenCenter(Transform Exclude)
        {
            int d = Exclude.gameObject.layer;
            Exclude.gameObject.layer = 0;
            RaycastHit hit;

            bool HasHitSomething = Physics.Raycast(o.ScreenRay, out hit, 256, Vecteur.SolidCharacter);

            Exclude.gameObject.layer = d;

            if (HasHitSomething)
                return hit.point;
            else
                return MainCamera.o.transform.position + MainCamera.o.transform.forward * 256;
        }

    }
}