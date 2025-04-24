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

        public void block ( Vector3 Normal )
        {
            if (on)
            rotation = Vecteur.RotDirectionQuaternion (Vector3.zero,Normal);
        }

        void Main ()
        {
            ShootCast ();
            DrawGraphic ();
            CheckLife ();
            SendAlert ();
        }

        void ShootCast ()
        { 
            float spd = speed * Time.deltaTime;
            if (Physics.Raycast (position, Vecteur.Forward (rotation), out RaycastHit Hit, spd, Vecteur.SolidCharacterAttack ))
            {
                position += Vecteur.Forward (rotation) * Hit.distance;
                if ( m_attack_receiver.index.TryGetValue ( Hit.collider.id(), out m_attack_receiver A ) )
                {
                    Reaction.Clash ( A.mr, new Force ( ForceType.perce, 0, position ) );
                    DeFire (this);
                    return;
                }
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
                DeFire(this);
                return;
            }
        }

        void SendAlert ()
        {
            if (Physics.Raycast(position, Vecteur.Forward(rotation), out RaycastHit hit, 2 * speed, Vecteur.SolidCharacterAttack))
            {
                if (m_trajectile_alert.index.TryGetValue(hit.collider.id() , out m_trajectile_alert A))
                    A.AlertIncomingTrajectile ( this , hit.distance / speed );
            }
            return;
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
