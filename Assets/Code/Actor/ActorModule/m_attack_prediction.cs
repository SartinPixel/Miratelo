using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixify;
using System;

namespace Triheroes.Code
{
    public class m_trajectile_alert : indexedcore<m_trajectile_alert>
    {
        public bool Alert { get; private set; }
        public d_trajectile incomingTrajectile { get; private set; }
        public float distance {private set; get;}

        public void AlertIncomingTrajectile(d_trajectile t, float distance)
        {
            if (on && this.distance > distance)
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
        void Main()
        {
            if (done)
                Clear();

            if (Alert)
                done = true;
        }

        void Clear()
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
                o.Main();
            }
        }
    }

    public class m_slash_alert : indexedcore <m_slash_alert>
    {
        public bool Alert;
        public float timeLeft {private set; get;}
        public SuperKey parryKey {private set; get;}

        public void AlertSlash ( float timeLeft, SuperKey parryIdFor )
        {
            if ( on && this.timeLeft > timeLeft)
            {
                Alert = true;
                this.timeLeft = timeLeft;
                parryKey = parryIdFor;
            }
        }

        protected override void OnAquire1()
        {
            Clear ();
        }

        protected override void OnFree1()
        {
            Clear ();
        }

        void Clear ()
        {
            Alert = false;
            timeLeft = Mathf.Infinity;
            parryKey = StateKey.zero;
        }

        void Main ()
        {
            if (Alert)
            {
                timeLeft -= Time.deltaTime;

                if (timeLeft <= 0)
                Clear ();
            }
        }

        public class s_slash_alert : CoreSystem<m_slash_alert>
        {
            protected override void Main(m_slash_alert o)
            {
                o.Main ();
            }
        }
    }
}