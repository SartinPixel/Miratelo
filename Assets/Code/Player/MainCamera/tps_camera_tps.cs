using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class tps_camera : pc_camera_tps_controller
    {

        public override void Default()
        {
            UpdateFromC ();
            offset = Vector3.zero;
        }

        public override void Update()
        {
            height = c.C.md.h * 1.25f;
            distance = 7;
            //rotate according to mouse
            rotY.y += Player.DeltaMouse().x * 3;
            rotY.x -= Player.DeltaMouse().y * 3;
            rotY.x = Mathf.Clamp( rotY.x, -65, 65 );
        }
    }
}
