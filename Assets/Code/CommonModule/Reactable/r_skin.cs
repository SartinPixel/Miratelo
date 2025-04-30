using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Triheroes.Code
{

    public class r_skin : reactable
    {
        public override void Clash(Force force, reactable from)
        {
            Reaction.sp_blow.Emit ( force.impactPoint );
        }
    }
    
}
