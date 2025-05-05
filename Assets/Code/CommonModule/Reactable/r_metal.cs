using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Triheroes.Code
{
    public class r_metal : reactable
    {
        public override void Clash(Force force, reactable from)
        {
            if ( force.type == ForceType.perce_parry )
            {
                Reaction.sp_impact.Emit ( force.impactPoint );
                Parry?.Invoke ( force );
                return;
            }
        }
    }
}