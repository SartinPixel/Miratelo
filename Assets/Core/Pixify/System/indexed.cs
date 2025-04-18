using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pixify
{
    public abstract class indexedmodule<T> : module where T : indexedmodule<T>, new()
    {
        public static Dictionary<int, T> index;

        /// <summary>
        /// to be initialized by sceneMaster
        /// </summary>
        public static void InitIndex()
        {
            index = new Dictionary<int, T>();
        }

        public sealed override void Create()
        {
            index.Add(character.gameObject.GetInstanceID(), this as T);
            Create1 ();
        }

        public virtual void Create1 () {}
    }

    public abstract class indexedcore<T> : core where T : indexedcore<T>, new()
    {
        public static Dictionary<int, T> index;

        /// <summary>
        /// to be initialized by sceneMaster
        /// </summary>
        public static void InitIndex()
        {
            index = new Dictionary<int, T>();
        }

        protected sealed override void OnAquire()
        {
            index.Add(character.gameObject.GetInstanceID(), this as T);
            OnAquire1();
        }
        protected virtual void OnAquire1() { }
        protected virtual void OnFree1() { }

        protected sealed override void OnFree()
        {
            index.Remove(character.gameObject.GetInstanceID());
            OnFree1();
        }
    }
}
