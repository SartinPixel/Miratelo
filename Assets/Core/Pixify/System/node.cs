using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pixify
{
    
    public abstract class node
    {
        /// <summary>
        /// called by the host controller ( script engine or character )
        /// </summary>
        public virtual void Create () {}

        /// <summary>
        /// registered id for the character
        /// </summary>
        [HideInInspector]
        public uint id;
    }

    [AttributeUsage (AttributeTargets.Field)]
    public sealed class DependAttribute : Attribute
    {
    }

}