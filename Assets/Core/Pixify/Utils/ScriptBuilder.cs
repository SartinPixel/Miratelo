using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pixify
{
    /// <summary>
    /// set of utility to speed create script
    /// </summary>
    public static class ScriptWriter
    {
        public static script NewScriptFromRoot ( neuro root )
        {
            return new script ( RootToTree (root), root );
        }

        public static script NewScriptFromRoots ( params neuro[] roots )
        {
            List <action> actions = new List<action>();
            foreach ( var r in roots )
            actions.AddRange (RootToTree (r));
            return new script ( actions , roots [0] );
        }

        public static List<action> RootToTree ( neuro root )
        {
            List <action> ns = new List<action>();
            RecursiveAdd (root);

            void RecursiveAdd (action a)
            {
                ns.Add (a);
                if (a is neuro r)
                {
                    foreach ( var a1 in r.o)
                    RecursiveAdd (a1);
                }
            }
            return ns;
        }

        public static action[] DO ( params action[] o )
        {
            return o;
        }

        public static condition[] IF ( params condition[] c )
        {
            return c;
        }

        public static condition NOT ( condition c )
        {
            c.inverse = true;
            return c;
        }
    }
}