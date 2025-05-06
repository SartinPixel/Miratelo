using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixify;

namespace Triheroes.Code
{
    public class s_capsule_character_controller_ground_data : CoreSystem<m_capsule_character_controller>
    {
        protected override void Main(m_capsule_character_controller o)
        {
            if (!o.gravity)
                return;

            o.mgd.onGroundAbs = false;
            o.mgd.onGround = Physics.SphereCast(o.Coord.position + new Vector3(0, o.CCA.radius + 0.1f, 0), o.CCA.radius, Vector3.down, out RaycastHit hit, 0.3f, Vecteur.Solid);

            if (o.mgd.onGround)
            {
                o.mgd.groundNormal = hit.normal;
                o.mgd.onGroundAbs = hit.distance <= 0.2f;
            }
        }
    }
}
