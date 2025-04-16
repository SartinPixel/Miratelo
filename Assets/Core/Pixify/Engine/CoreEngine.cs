using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pixify
{
    // entend your scenemaster script with this to use the core engine
    public abstract class CoreEngine : MonoBehaviour
    {
        public static CoreEngine o;

        List < CoreSystemBase > Systems = new List<CoreSystemBase> ();
        Dictionary < Type, List <core> > IndexedCores = new Dictionary<Type, List<core>> ();

        public CoreEngine ()
        {
            o = this;
        }

        public void Awake()
        {
            o = this;
            CreateSystems ( out Systems );
            foreach (var sys in Systems )
            sys.cores = RequestListModulesOfType ( sys.SystemType() );
        }

        void LateUpdate ()
        {
            foreach ( var s in Systems )
            s.Execute ();
        }

        List<core> RequestListModulesOfType (Type t)
        {
            if ( IndexedCores.TryGetValue (t, out List<core> L) )
            return L;
            else
            {
                L = new List<core>();
                IndexedCores.Add (t, L);
                return L;
            }
        }

        /// <summary>
        /// Return the systems
        /// </summary>
        protected abstract void CreateSystems ( out List<CoreSystemBase> Systems);

        public void Register ( core core ) => RequestListModulesOfType ( core.GetType() ).Add (core);

    }

    public abstract class CoreSystemBase
    {
        public List < core > cores;
        public abstract void Execute ();
        public abstract Type SystemType ();
    }

    public abstract class CoreSystem <T> : CoreSystemBase where T : core
    {
        public sealed override Type SystemType () => typeof (T);

        public sealed override void Execute ()
        {
            foreach (T o in cores)
            {
                if (o.on)
                Main (o);
            }
        }

        protected abstract void Main ( T o );
    }
}
