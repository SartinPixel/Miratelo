using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pixify
{
    public class change_focus : action
    {
        SuperKey key;

        public change_focus (SuperKey key)
        {
            this.key = key;
        }

        protected override bool Step()
        {
            script.host.ChangeFocus (key);
            return false;
        }
    }

    public class Log : action
    {
        string log;

        public Log (string message)
        {
            log = message;
        }

        protected override bool Step()
        {
            Debug.Log (log);
            return true;
        }
    }

    public class Skip : action
    {
        protected override bool Step()
        {
            return true;
        }
    }

    public class Null : action
    {
        protected override bool Step()
        {
            return false;
        }
    }
}

