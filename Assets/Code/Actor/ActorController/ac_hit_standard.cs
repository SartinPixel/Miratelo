using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class ac_ready_hit_standard : action
    {
        [Depend]
        m_reaction_receiver mrr;
        [Depend]
        m_hit mh;

        protected override void BeginStep()
        {
            mrr.reactable.Knocked = mh.HitKnocked;
        }

        protected override void Stop()
        {
            mrr.reactable.Knocked = null;
        }
    }
}