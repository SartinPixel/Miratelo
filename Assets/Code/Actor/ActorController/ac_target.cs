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
}
