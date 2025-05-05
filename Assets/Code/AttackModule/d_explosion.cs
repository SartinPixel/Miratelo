using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixify;

namespace Triheroes.Code
{
    public class d_explosion : vDot <d_explosion>
    {
        float radius;
        float rawPower;
        Vector3 position;
        m_reaction_receiver mrr;

        public override void Create()
        {
            mrr = new m_reaction_receiver ();
        }

        public static void Fire ( Vector3 pos, float rawPower ,float radius )
        {
            var a = BeginFire ();

            a.position = pos;
            a.radius = radius;
            a.rawPower = rawPower;

            EndFire ();
        }

        protected override void OnAquire()
        {
            Reaction.sp_explosion.Emit (position);

            Collider[] HittedCharacter = Physics.OverlapSphere ( position, radius, Vecteur.Character );
            for (int i = 0; i < HittedCharacter.Length; i++)
            {
                if ( m_attack_receiver.TryGet ( HittedCharacter[i].id(), out m_attack_receiver A ) )
                Reaction.Clash ( mrr, A.mrr, new Force ( ForceType.explosion, rawPower, position) );
            }

            DeFire (this);
        }
    }
}