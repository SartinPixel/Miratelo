using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class ac_hit_cgm : action
    {
        [Depend]
        m_capsule_character_controller mccc;
        [Depend]
        m_skin ms;

        protected override void BeginStep()
        {
            mccc.Aquire (this);
            ms.PlayState (0, AnimationKey.hit, 0.1f, AppendStop);
            // mccc.velocityDir += ( ms.Coord.position - LastAttack.impactPoint).normalized * 0.1f;
        }

        protected override void Stop()
        {
            mccc.Free (this);
        }

    }
}
