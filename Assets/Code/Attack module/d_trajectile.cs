using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixify;

namespace Triheroes.Code
{
    public class d_trajectile : vDot <d_trajectile>
    {

        public float timeLeft;
        public float speed;
        public Vector3 position;
        public Quaternion rotation;
        public DotSkin dotSkin;

        public static void Fire ( DotSkin dotSkin, Vector3 pos, Quaternion rot, float speed )
        {
            var a = BeginFire ();

            a.dotSkin = dotSkin;
            a.position = pos;
            a.rotation = rot;
            a.speed = speed;
            a.timeLeft = 15;

            EndFire ();
        }

        void Main ()
        {
            ShootCast ();
            DrawGraphic ();
            CheckLife ();
        }

        void ShootCast ()
        { 
            float spd = speed * Time.deltaTime;
            if (Physics.Raycast (position, Vecteur.Forward (rotation), out RaycastHit Hit, spd, Vecteur.SolidCharacterAttack ))
            {
                position += Vecteur.Forward (rotation) * Hit.distance;

                // DOTO attack receiving
                /*if ( m_attack_receiver.index.TryGetValue ( Hit.collider.id(), out m_attack_receiver A ) )
                {
                    DePiow (this);
                    return;
                }*/
            }
            else
            position += Vecteur.Forward (rotation) * spd;
        }

        void DrawGraphic ()
        {
            Graphics.DrawMesh(dotSkin.Mesh, position, rotation.AppliedAfter(dotSkin.Rotator), dotSkin.Material, 0);
        }

        void CheckLife()
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                DePiow(this);
                return;
            }
        }

        public class s_trajectile : CoreSystem<d_trajectile>
        {
            protected override void Main(d_trajectile o)
            {
                o.Main ();
            }
        }
    }
}
