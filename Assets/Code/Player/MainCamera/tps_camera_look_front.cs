using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Triheroes.Code
{
    public class tps_camera_look_front : pc_camera_tps_controller
    {
        public override void Default()
        {
            UpdateFromC ();
            rotY = c.C.ms.rotY;
        }
    }
}