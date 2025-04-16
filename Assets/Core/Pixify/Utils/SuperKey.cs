using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pixify
{
    [System.Serializable]
    public struct SuperKey
    {
        [SerializeField]
        private int value;

        [SerializeField]
        public string keyName;

        public SuperKey(string keyName)
        {
            this.keyName = keyName;
            this.value = Animator.StringToHash(keyName);
        }

        public static implicit operator int(SuperKey key)
        {
            return key.value;
        }

        public override string ToString()
        {
            return value.ToString();
        }

        //  Add equality operators
        public override bool Equals(object obj)
        {
            if (obj is SuperKey)
            {
                return this.value == ((SuperKey)obj).value;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }

}
