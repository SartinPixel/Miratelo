using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pixify
{
    public abstract class core : module
    {
        public bool on {get; private set;}

        public core () { CoreEngine.o.Register (this); }

        node host;
        public void Aquire (node host)
        {
            if (!on)
            {
            on = true;
            this.host = host;
            OnAquire ();
            }
            else
                throw new InvalidOperationException("cannot aquire, make sure to free it before aquiring");
        }

        protected void SelfFree ()
        {
            if (on)
            {
                if (host is ICoreReceptor h)
                h.SelfFreed (this);
                Free (host);
            }
            else
            throw new InvalidOperationException("Self free of inaquired node");
        }

        protected virtual void OnAquire () {}
        protected virtual void OnFree () {}

        public void Free (node host)
        {
            if ( on && this.host == host)
            {
                on = false;
                OnFree ();
                this.host = null;
            }
            else
                throw new InvalidOperationException("cannot free things this node doesn't host, or the node is no longuer aquired in the first place");
        }
    }

    public interface ICoreReceptor
    {
        public void SelfFreed ( node AquiredNode );
    }
}