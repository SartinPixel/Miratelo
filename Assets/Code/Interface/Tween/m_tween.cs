using System;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public abstract class m_tween : core
    {
        public abstract void Main ();
    }

    public class s_tween : CoreSystem<m_tween>
    {
        protected override void Main(m_tween o)
        {
            o.Main ();
        }
    }

    [RegisterAsBase]
    public class mt_push : m_tween
    {
        Func <float> GetX;
        Action <float> GiveX;
        float targetX;

        protected override void OnAquire()
        {
            enabled = false;
        }

        public void Set ( float target,  Func <float> GetX, Action <float> GiveX )
        {
            enabled = true;
            targetX = target;
            this.GetX = GetX;
            this.GiveX = GiveX;
        }

        // TODO make independent of framerate
        public override void Main ()
        {
            GiveX (Mathf.Lerp ( GetX() ,targetX ,.1f ));
            if ( Mathf.Abs (GetX() - targetX) < 0.01f )
            enabled = false;
        }
    }
}
