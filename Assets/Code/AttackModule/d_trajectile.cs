using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixify;

namespace Triheroes.Code
{
    public class d_trajectile : vDot <d_trajectile>
    {
        public m_reaction_receiver mrr;
        public float timeLeft;
        public float speed;
        public Vector3 position;
        public Quaternion rotation;
        public DotSkin dotSkin;
        float rawPower;

        public override void Create()
        {
            mrr = new m_reaction_receiver();
            mrr.reactable = new r_metal ();
            mrr.Clash += Clash;
        }

        public static void Fire ( DotSkin dotSkin, Vector3 pos, Quaternion rot, float speed, float rawPower )
        {
            var a = BeginFire ();

            a.dotSkin = dotSkin;
            a.position = pos;
            a.rotation = rot;
            a.speed = speed;
            a.rawPower = rawPower;
            a.timeLeft = 15;

            EndFire ();
        }

        void Clash ( Force force )
        {
            if (on && force.type == ForceType.perce_parry )
            rotation = Vecteur.RotDirectionQuaternion (Vector3.zero,force.Normal);
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
                    Reaction.Clash ( mrr, A.mrr, new Force ( ForceType.perce, rawPower, position) );
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
                if (m_trajectile_alert.TryGet (hit.collider.id() , out m_trajectile_alert A))
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
