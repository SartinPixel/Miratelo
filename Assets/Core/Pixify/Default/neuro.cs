using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pixify
{

    public sealed class sequence : neuro
    {
        int ptr;

        public bool repeat = true;
        public bool reset = true;

        public sequence(params action[] o)
        {
            this.o = o;
        }

        public sequence set (bool repeat = true, bool reset = true)
        {
            this.repeat = repeat;
            this.reset = reset;
            return this;
        }

        protected sealed override void BeginStep()
        {
            if (reset)
                ptr = 0;
            o[ptr].iStart();
        }

        protected sealed override bool Step()
        {
            if (o[ptr].on)
                o[ptr].iExecute();

            if (!o[ptr].on)
            {
                ptr++;

                if (ptr >= o.Length)
                {
                    ptr = 0;
                    if (!repeat)
                        return true;
                }
                o[ptr].iStart();
            }
            return false;
        }

        protected override void Abort()
        {
            if (o[ptr].on)   
            o[ptr].iAbort();
        }
    }

    
    public sealed class parallel : neuro
    {
        public bool StopWhenFirstNodeStopped;

        public parallel ( params action [] o )
        {
            this.o = o;
        }

        public parallel Set (bool StopWhenFirstNodeStopped)
        {
            this.StopWhenFirstNodeStopped = StopWhenFirstNodeStopped;
            return this;
        }

        protected override void BeginStep()
        {
            foreach (var n in o)
                n.iStart();
        }

        protected override bool Step()
        {
            bool Continue = false;

            foreach (var n in o)
            {
                if (n.on)
                {
                    Continue = true;
                    break;
                }
            }

            if (StopWhenFirstNodeStopped)
            {
                if (!o[0].on)
                    return true;
            }
            else if (Continue == false)
            {
                return true;
            }

            foreach (var n in o)
            {
                if (n.on)
                {
                    n.iExecute();
                }
            }

            return false;
        }

        protected override void Stop()
        {
            foreach (var n in o)
            {
                if (n.on)
                    n.iAbort();
            }
        }
    }

    
    public class spawner : neuro
    {
        public bool IntervalFirst;
        public int SpamCount = 1;
        public float TimeInterval = 1;

        int spamCounter;
        float time;
        bool isRunning;

        public spawner ( params action[] o)
        {
            this.o = o;
        }

        public spawner Set ( bool intervalFirst, int spamCount, float TimeIntervalSec )
        {
            IntervalFirst = intervalFirst;
            SpamCount = spamCount;
            TimeInterval = TimeIntervalSec;
            return this;
        } 

        protected override void BeginStep()
        {
            spamCounter = 0;
            time = 0;
            if (!IntervalFirst)
            {
                isRunning = true;
                o[0].iStart();
            }
        }

        protected override bool Step()
        {
            if (isRunning)
            {
                if (!o[0].on)
                {
                    isRunning = false;  // Stop running and start the timer
                    time = 0f;
                    spamCounter++;
                    if (spamCounter >= SpamCount)
                        return true;  // Stop the node if spam count is reached
                }
                else
                {
                    o[0].iExecute();  // Continue updating the child if it's still running
                }
            }
            else
            {
                time += Time.deltaTime;
                if (time >= TimeInterval)
                {
                    isRunning = true;
                    o[0].iStart();  // Restart the child after the interval
                }
            }

            return false;  // Node is not yet finished
        }

        protected override void Abort()
        {
            if (o[0].on)
                o[0].iAbort();
        }
    }

}
