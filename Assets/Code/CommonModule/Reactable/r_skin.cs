using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class r_skin : reactable
    {
        [Depend]
        m_stat_health msh;

        public override void Clash(Force force, reactable from)
        {
            msh.HP -= force.raw;
            Hit?.Invoke (force);
            Reaction.sp_blow.Emit ( force.impactPoint );
        }
    }
}