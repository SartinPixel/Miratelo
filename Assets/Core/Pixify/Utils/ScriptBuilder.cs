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
    }
}