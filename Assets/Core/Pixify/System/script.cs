using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pixify
{

    public sealed class script
    {
        m_character_controller host;
        List <action> actions;
        neuro root;
        
        public script ( List <action> ns, neuro root )
        {
            actions = ns;
            this.root = root;
        }

        public void RegisterScript ( m_character_controller host )
        {
            this.host = host;

            foreach (action n in actions)
            {
                n.script = this;
                host.character.ConnectAction (n);
            }
        }

        public bool on { private set; get;}        
        public void Start ()
        {
            if (!on)
            {
                on = true;
            }
            else
            throw new InvalidOperationException("cannot start a script that is already live");
        }

        public void Execute ()
        {
            if (on)
            {
                TickRoot ();
                if (!root.on)
                on = false;
            }
            else
            throw new InvalidOperationException("cannot execute a stopped script");
        }

        void TickRoot ()
        {
            neuro temp = root;
            if (root.on)
            root.iExecute();
            else
            temp.iStart ();
            if (temp != root)
            {
            temp.iAbort ();
            root.iStart ();
            }
        }

        public void Stop ()
        {
            if (on)
            {
                root.iAbort ();
                on = false;
            }
            else
            throw new InvalidOperationException("trying to stop a stopped script");
        }

        public void ChangeRoot ( neuro newRoot )
        {
            root = newRoot;
        }

    }

}