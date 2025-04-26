using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Hold collection of data to be used by modules, this will be discarded when modules are ready
/// </summary>
namespace Pixify
{
    public class CharacterData : MonoBehaviour
    {   
        public void Update()
        {
        Destroy (this);
        }
    }
}
