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

        void RegisterNode ( node m )
        {
            foreach (var fi in m.GetType().GetFields( BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy))
            {
                if (fi.GetCustomAttribute<DependAttribute>() != null)
                {
                    fi.SetValue ( m, NeedModule(fi.FieldType) );
                }
            }
        }

        /// <summary>
        /// connect and initialize this node with the character
        /// </summary>
        public action ConnectAction (action a)
        {
            RegisterNode (a);
            a.Create ();
            return a;
        }

        public T ConnectAction <T> (T a) where T:action
        {
            RegisterNode (a);
            a.Create ();
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
                mcc.Create ();
                modules.Add (mcc.GetType(),mcc);

                mcc.Aquire ( new Null() );
            }
        }

    }

}
