using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Pixify
{

    // Gameobject with an unique index module dictionary
    public class Character : MonoBehaviour
    {

        protected Dictionary<Type, module> modules;
        List <node> nodes;

        public module NeedModule(Type moduleType)
        {
            if (!moduleType.IsSubclassOf(typeof (module)))
                throw new InvalidOperationException("cannot depend on a non module type");

            if (modules.TryGetValue(moduleType, out module m))
                return m;
            else
            {
                m = Activator.CreateInstance(moduleType) as module;
                modules.Add(moduleType, m);

                m.character = this;
                RegisterNode (m);
                m.Create ();

                return m;
            }
        }

        uint ptr;
        void RegisterNode ( node m )
        {
            ptr ++;
            m.id = ptr;
            nodes.Add (m);

            Type current = m.GetType ();
            while ( current != typeof ( node ) )
            {
                var fis = current.GetFields( BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                foreach (var fi in fis)
                {
                if (fi.GetCustomAttribute<DependAttribute>() != null)
                    fi.SetValue ( m, NeedModule(fi.FieldType) );
                }
                current = current.BaseType;
            }
        }

        /// <summary>
        /// connect and initialize this node with the character if it is not connected
        /// </summary>
        public action ConnectAction (action a)
        {
            if (!nodes.Contains (a))
            {
                RegisterNode (a);
                a.Create ();
            }
            return a;
        }

        public node ConnectNode ( node n )
        {
            if (!nodes.Contains (n))
            {
                RegisterNode (n);
                n.Create ();
            }
            return n;
        }

        public T ConnectAction <T> (T a) where T:action
        {
            if (!nodes.Contains (a))
            {
                RegisterNode (a);
                a.Create ();
            }
            return a;
        }

        /// <summary>
        /// connect and initialize this root and its tree with the character
        /// </summary>
        public action [] ConnectRoot (neuro r)
        {
            List<action> actions = ScriptWriter.RootToTree ( r );
            foreach ( var n in actions )
            ConnectAction (n);
            return actions.ToArray();
        }

        void Awake()
        {
            modules = new Dictionary<Type, module> ();
            nodes = new List<node>();
            GetCharacterControllerData ();
        }


        void GetCharacterControllerData ()
        {
            ScriptInit[] CCS = GetComponents <ScriptInit> ();
            if ( CCS.Length > 0 )
            {
                Dictionary <SuperKey,script> scripts = new Dictionary<SuperKey, script> ();
                foreach (var ccs in CCS)
                    ccs.OnAddScript ( scripts );

                m_character_controller mcc = new m_character_controller ( scripts );
                mcc.character = this;
                modules.Add (mcc.GetType(),mcc);
                nodes.Add (mcc);
                mcc.Create ();

                mcc.Aquire ( new Null() );
            }
        }

    }

}
