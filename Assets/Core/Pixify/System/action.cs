using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pixify
{

    public abstract class action : node
    {
        public script script;

        public bool on;

        public void iStart ()
        {
            if (!on)
            {
                stopPending = false;
                on = true;
                BeginStep ();
            }
            else
            throw new InvalidOperationException("cannot start node that is already started");
        }

        
        /// <summary>
        /// don't call from inside, use append stop instead if inside
        /// </summary>
        public void iAbort()
        {
            if (on)
            {
                Abort ();
                on = false;
                stopPending = false;
            }
            else
            throw new InvalidOperationException("cannot abort node that is already stopped");
        }

        /// <summary>
        /// don't call from inside, use append stop instead if inside
        /// </summary>
        public void iStop()
        {
            if (on)
            {
                Stop ();
                on = false;
                stopPending = false;
            }
            else
            throw new InvalidOperationException("cannot stop node: that is already stopped");
        }

        bool stopPending;
        protected void AppendStop ()
        {
            stopPending = true;
        }

        public void iExecute()
        {
            if (on)
            {
                if ( Step() || stopPending )
                {
                iStop ();
                stopPending = false;
                }
            }
            else
            throw new InvalidOperationException("cannot execute node: that is already stopped");
        }

        /// <summary>
        /// called when on the first frame, when node switch to active
        /// </summary>
        protected virtual void BeginStep () {}
        /// <summary>
        /// called every frame when the node is active
        /// </summary>
        protected virtual bool Step () {return false;}
        /// <summary>
        /// called when the node is stopped from inside
        /// </summary>
        protected virtual void Stop () {}
        /// <summary>
        /// called when the node is stopped from outside
        /// </summary>
        protected virtual void Abort () { Stop(); }
    }

    public abstract class neuro : action {
        public action [] o {protected set; get;}
    }

}