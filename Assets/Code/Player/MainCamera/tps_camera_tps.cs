using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class tps_camera : pc_camera_tps_controller
    {
        bool lookForward;
        Vector3 lookForwardTarget;

        public override void Default()
        {
            UpdateFromC ();

            m_black_bar.o.Push ( 0 );

            offset = Vector3.zero;
            lookForward = false;
        }

        public override void Update()
        {
            height = c.C.md.h * 1.25f;
            distance = 7;

            //rotate according to mouse
            rotY.y += Player.DeltaMouse().x * 3;
            rotY.x -= Player.DeltaMouse().y * 3;
            rotY.x = Mathf.Clamp( rotY.x, -65, 65 );

            if ( Player.GetButtonDown ( BoutonId.R ) )
            {
                lookForward = true;
                lookForwardTarget = c.C.ms.rotY;
            }

            if ( Player.GetButtonUp ( BoutonId.R ) || Player.DeltaMouse().x > 10 || Player.DeltaMouse().y > 10 )
                lookForward = false;

            if ( lookForward )
            {
                rotY.y = Mathf.LerpAngle ( rotY.y, lookForwardTarget.y, .1f );
                rotY.x = Mathf.LerpAngle ( rotY.x, lookForwardTarget.x, .1f );
            }
        }
    }
}
