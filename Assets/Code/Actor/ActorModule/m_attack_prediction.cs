using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixify;

namespace Triheroes.Code
{
    public class m_trajectile_alert : indexedcore <m_trajectile_alert>
    {
        public bool Alert {get; private set;}
        public d_trajectile incomingTrajectile {get; private set;}
        float distance;

        public void AlertIncomingTrajectile ( d_trajectile t, float distance )
        {
            if ( on && this.distance > distance )
            {
            Alert = true;
            incomingTrajectile = t;
            this.distance = distance;
            }
        }
        
        protected override void OnAquire1()
        {
            Clear();
        }

        protected override void OnFree1()
        {
            Clear();
        }

        bool done;
        void Main ()
        {
            if (done)
            Clear ();
            
            if (Alert)
            done = true;
        }

        void Clear ()
        {
            done = false;
            Alert = false;
            incomingTrajectile = null;
            distance = Mathf.Infinity;
        }

        public class s_trajectile_alert : CoreSystem<m_trajectile_alert>
    {
        protected override void Main(m_trajectile_alert o)
        {
            o.Main ();
        }
    }
    }
}