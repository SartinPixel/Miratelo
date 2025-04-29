using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pixify
{
    public abstract class core : module
    {
        bool _on;
        protected bool enabled;
        public bool on { get {return enabled && _on;} }
        public bool aquired => on;

        public core () { CoreEngine.o.Register (this); }

        node host;
        public void Aquire (node host)
        {
            if (!_on)
            {
            _on = true;
            enabled = true;
            this.host = host;
            OnAquire ();
            }
            else
                throw new InvalidOperationException("cannot aquire, make sure to free it before aquiring");
        }

        protected void SelfFree ()
        {
            if (_on)
            {
                if (host is ICoreFeedbackSimple h)
                {
                Free (host);
                h.AquiredNodeStopped (this);
                h.AquiredNodeFreed (this);
                }
                else
                Free (host);
            }
            else
            throw new InvalidOperationException("Self free of inaquired node");
        }

        protected void SelfAbort ()
        {
            if (_on)
            {
                if (host is ICoreFeedback h)
                {
                Free (host);
                h.AquiredNodeAborted (this);
                h.AquiredNodeFreed (this);
                }
                else
                Free (host);
            }
            else
            throw new InvalidOperationException("Self Abort of inaquired node");
        }

        protected virtual void OnAquire () {}
        protected virtual void OnFree () {}

        public void Free (node host)
        {
            if ( _on && this.host == host)
            {
                _on = false;
                enabled = false;
                OnFree ();
                this.host = null;
            }
            else
                throw new InvalidOperationException("cannot free things this node doesn't host, or the node is no longuer aquired in the first place");
        }
    }

    public interface ICoreFeedbackSimple
    {
        public void AquiredNodeFreed ( node AquiredNode );
        public void AquiredNodeStopped ( node AquiredNode );
    }

    public interface ICoreFeedback : ICoreFeedbackSimple
    {
        public void AquiredNodeAborted ( node AquiredNode );
    }
}