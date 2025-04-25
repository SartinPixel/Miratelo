using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class s_skin_produceral_bow : CoreSystem<m_skin_produceral_bow>
    {
        protected override void Main(m_skin_produceral_bow o)
        {
            if ( o.isRotatingWithBow == m_skin_produceral_bow.isRotatingWithBowState.disabled )
            return;

            o.TargetRotY.z = o.bow.eulerAngles.z;

            Quaternion Diff = Quaternion.Euler(o.TargetRotY) * Quaternion.Inverse(o.bow.rotation);

            if ( o.isRotatingWithBow == m_skin_produceral_bow.isRotatingWithBowState.running )
            o.weakDiff = Quaternion.RotateTowards ( o.weakDiff, Diff, 360 * Time.deltaTime );

            o.Spine.rotation = Quaternion.Slerp(Quaternion.identity, o.weakDiff, 1) * o.Spine.rotation;
        }
    }
}
