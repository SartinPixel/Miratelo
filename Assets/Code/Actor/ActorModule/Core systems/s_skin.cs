using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class s_skin : CoreSystem <m_skin>
    {
        protected override void Main(m_skin o)
        {
            o.Update ();
            o.Ani.transform.position = o.Coord.position + o.PosY;
            o.Ani.transform.rotation = Quaternion.Euler ( 0,o.RotY.y,0 );
        }
    }
}
