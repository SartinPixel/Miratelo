using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class tps_camera_target : pc_camera_tps_controller
    {
        public Vector3 rotYOffset;
        Vector3 rotYToTarget;

        Vector3 mpos => c.C.md.position;
        Vector3 tpos => c.C.target.md.position;

        public override void Default()
        {
            UpdateFromC ();

            CalculateOffest ();
            rotYToTarget = Vecteur.RotDirection (mpos, tpos);
            rotY = rotYToTarget;
        }

        public override void Update()
        {
            CalculateOffest ();

            // rotate offset according to mouse
            rotYOffset.y += Player.DeltaMouse().x * 3;
            rotYOffset.x -= Player.DeltaMouse().y * 3;

            var a = Vecteur.RotDirection (mpos, tpos);
            rotYToTarget.x = Mathf.MoveTowardsAngle ( rotYToTarget.x, a.x, 180*Time.unscaledDeltaTime );
            rotYToTarget.y = Mathf.MoveTowardsAngle ( rotYToTarget.y, a.y, 180*Time.unscaledDeltaTime );

            rotY = rotYOffset + rotYToTarget;

            rotY.x = Mathf.Clamp( rotY.x, -65, 65 );

            return;
        }

        void CalculateOffest ()
        {
            float dist = Vector3.Distance ( mpos, tpos );
            float distRatio = Mathf.Abs (Mathf.DeltaAngle ( Vecteur.RotDirection (mpos, tpos).y, rotY.y )) / 180;
            distance = 7 + (dist * distRatio / 2);
            offset = Vecteur.LDir ( Vecteur.RotDirection (mpos, tpos), Vector3.forward * (dist * distRatio / 2) );
        }

    }
}