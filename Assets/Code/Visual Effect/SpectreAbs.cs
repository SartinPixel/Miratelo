using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Triheroes.Code
{
    public class SpectreAbs : MonoBehaviour
    {
        ParticleSystem System;
        ParticleSystem.MainModule Main;
        short StandardCount;
        Quaternion InitialRotation;

        void Awake ()
        {
            System = GetComponent <ParticleSystem> ();
            Main = System.main;
            StandardCount = System.emission.GetBurst (0).maxCount;
            InitialRotation = Quaternion.Euler (new Vector3 ( Main.startRotationX.constant , Main.startRotationY.constant, Main.startRotationZ.constant ) * Mathf.Rad2Deg);
        }

        public void Emit(Vector3 position, Quaternion rotation, float MaxSize)
        {
            Vector3 rot = (rotation * InitialRotation).eulerAngles;
            ParticleSystem.EmitParams e = new ParticleSystem.EmitParams { position = position, rotation3D = rot};
            Main.startSize = new ParticleSystem.MinMaxCurve(0,MaxSize);
            System.Emit ( e, StandardCount );
        }
    }
}
