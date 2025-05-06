using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pixify
{
    [Serializable]
    public struct DotSkin
    {
        public Vector3 Rotator;
        public Mesh Mesh;
        public Material Material;
    }

    /// <summary>
    /// self hosted pool system, this core is orphelin, selffree will make it escape from the pool system
    /// </summary>
    public class vDot <T> : core where  T: vDot <T>, new ()
    {
        // index in pool
        int index;

        // generic pool
        static Null [] poolA;
        static Queue <int> poolB;
        static T[] poolC;

        /// <summary>
        /// to be hardcode initialized by the sceneMaster
        /// </summary>
        public static void InitPool (int length)
        {
            poolB = NewPool (length);
        }

        static Queue <int> NewPool (int length)
        {
            poolA = new Null[length];
            var B = new Queue<int> ();
            poolC = new T [length];

            for (int i = 0; i < length; i++)
            {
                B.Enqueue (i);
                poolC [i] = new T() {index = i};
                poolC [i].Create ();
                poolA [i] = new Null ();
            }

            return B;
        }

        static void IncreasePool ()
        {
            int length = poolC.Length;
            int newlength = length * 2;

            List<Null> l0 = new List<Null> ( poolA );
            l0.AddRange ( new Null [length] );
            poolA = l0.ToArray ();

            List <T> l1 = new List<T> ( poolC );
            l1.AddRange ( new T[length] );
            poolC = l1.ToArray ();

            for (int i = length; i < newlength; i++)
            {
                poolB.Enqueue (i);
                poolC [i] = new T() {index = i};
                poolC [i].Create ();
                poolA [i] = new Null ();
            }
        }

        protected static T BeginFire ()
        {
            if (Firing)
            throw new InvalidOperationException ("FATAL ERROR: BeginPiow when a piow was in progress");

            if (poolB.Count == 0)
            IncreasePool ();
            
            hotIndex = poolB.Dequeue();
            T a = poolC [hotIndex];
            Firing = true;

            return a;
        }

        protected static void EndFire ()
        {
            Firing = false;
            poolC [hotIndex].Aquire ( poolA [hotIndex] );
        }

        static int hotIndex;
        static bool Firing;

        public static void DeFire (vDot<T> DotV)
        {
            int index = DotV.index;
            poolB.Enqueue(index);
            DotV.Free ( poolA [index] );
        }

        protected new void SelfFree ()
        {
            Debug.LogError ("Self free on a vDot is a bad idea, as it will escape from the pool");
        }
    }
}
