using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class ac_get_a_target : action
    {
        [Depend]
        m_actor ma;

        public float Distance = 30;

        public ac_get_a_target (float distance)
        {
            Distance = distance;
        }

        protected override bool Step()
        {
            ma.LockATarget ( ma.GetNearestFacedFoe ( Distance ) );
            if ( ma.target != null )
            return true;
            return false;
        }
    }

    public class ac_have_target : condition
    {
        [Depend]
        m_actor ma;

        protected override bool OnCheck()
        {
            return !( ma.target == null );
        }
    }

    public class ac_target_distance : condition
    {
        [Depend]
        m_actor ma;

        public float distance;
        public Comparator comparer;

        public ac_target_distance ( float distance, Comparator comparer )
        {
            this.distance = distance;
            this.comparer = comparer;
        }

        protected override bool OnCheck()
        {
            if (ma.target != null)
            return Loader.Compare ( Vector3.Distance (ma.md.position, ma.target.md.position), distance + ma.target.md.r + ma.md.r, comparer );
            return false;
        }
    }
}
