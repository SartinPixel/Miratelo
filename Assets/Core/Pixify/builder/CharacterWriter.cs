using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pixify
{
    public class CharacterWriter : MonoBehaviour , IBlockAuthor
    {
        void Awake ()
        {
            var writers = GetComponents<Writer>();
            List <Type> PixTypes = new List<Type> ();

            foreach (var a in writers)
            PixTypes.AddRange ( a.RequiredPix() );

            var Constructor = new PreBlock ( PixTypes.ToArray(), this );
            var b = Constructor.CreateBlock ();

            foreach (var a in writers)
            a.AfterWrite (b);

            Destroy (this);
        }
        
        public void OnWriteBlock()
        {
            var writers = GetComponents<Writer>();
            foreach (var a in writers)
            a.OnWriteBlock ();
        }
    }
}