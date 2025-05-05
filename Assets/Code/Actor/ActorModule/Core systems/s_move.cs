using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class s_move : CoreSystem<m_capsule_character_controller>
    {
        protected override void Main(m_capsule_character_controller o)
        {
            if (o.ms.SkinMove)
            o.dir += o.ms.GetSpdCurves() * o.ms.SkinDir * Time.deltaTime/*a*/;

            if (o.velocityDir.sqrMagnitude > 0)
                o.velocityDir = Vector3.MoveTowards(o.velocityDir, Vector3.zero, Vecteur.Drag * Time.deltaTime/*a*/);

            Physics.IgnoreLayerCollision(o.Coord.gameObject.layer, Vecteur.ATTACK, true);
            o.CCA.Move(o.dir + o.velocityDir);
            Physics.IgnoreLayerCollision(o.Coord.gameObject.layer, Vecteur.ATTACK, false);

            o.dir = Vector3.zero;
        }
    }
}
