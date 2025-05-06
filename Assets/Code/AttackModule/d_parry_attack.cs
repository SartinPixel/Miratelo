using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class d_parry_attack : vDot <d_parry_attack>
    {
        Sword sender;
        
        public static d_parry_attack Fire ( Sword sender )
        {
            var a = BeginFire ();
            a.sender = sender;
            EndFire ();
            return a;
        }

        void Main ()
        {
            if ( Physics.Linecast ( sender.transform.position + sender.transform.TransformDirection (Vector3.forward),sender.transform.position, out RaycastHit hit, Vecteur.Attack ) )
            {
                if ( d_slash_attack.TryGet ( hit.collider.id(), out var A ))
                {
                    Reaction.Clash ( sender.mrr, A.origin.Weapon.mrr, new Force (ForceType.parry,0,hit.point) );
                }
            }
        }

        public class s_parry_attack : CoreSystem<d_parry_attack>
        {
            protected override void Main(d_parry_attack o)
            {
                o.Main ();
            }
        }
    }
}
