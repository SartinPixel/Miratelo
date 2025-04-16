using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pixify
{
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

