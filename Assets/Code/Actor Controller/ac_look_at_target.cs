using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class ac_look_at_target : action
    {
        [Depend]
        m_actor ma;
        [Depend]
        m_standard_character_controller_host mscch;

        protected override bool Step()
        {
            if (ma.target != null)
            mscch.cgm.rotDir = ma.target.ms.Coord.position - ma.ms.Coord.position;

            return false;
        }
    }
}
