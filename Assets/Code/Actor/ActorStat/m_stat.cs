using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public abstract class m_stat_auto : core
    {
        public abstract void Main ();
    }

    public sealed class s_state_auto : CoreSystem<m_stat_auto>
    {
        protected override void Main(m_stat_auto o)
        {
            o.Main ();
        }
    }
}
