using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Triheroes.Code
{
    public class Spectre : MonoBehaviour
    {
        ParticleSystem System;

        void Awake ()
        {
            System = GetComponent <ParticleSystem> ();
        }

        public void Emit(Vector3 position)
        {
            ParticleSystem.EmitParams e = new ParticleSystem.EmitParams { position = position };
            System.Emit ( e, 1 );
        }
    }
}
