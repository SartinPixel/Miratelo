using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class m_dimension : module
    {
        public float r;
        public float h;
        public Vector3 position => character.transform.position;
    }
}