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

        public void set (bool repeat = true, bool reset = true)
        {
            this.repeat = repeat;
            this.reset = reset;
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
            if (o[0].on)
                o[0].iAbort();
        }
    }

    
    public sealed class parallel : neuro
    {
        public bool StopWhenFirstNodeStopped;

        public parallel ( params action [] o )
        {
            this.o = o;
        }

        public void Set (bool StopWhenFirstNodeStopped)
        {
            this.StopWhenFirstNodeStopped = StopWhenFirstNodeStopped;
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
}
