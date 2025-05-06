using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixify;

namespace Triheroes.Code
{
    public class d_slash_attack : vDot <d_slash_attack>
    {
        SlashAttackCollider Col;
        SlashAttackColliderSecond Col2;
        SlashAttackSize targSize;

        float rawPower;
        float time;
        public  m_sword_user origin {get; private set;}

        // manual indexation
        static Dictionary<int, d_slash_attack> index;
        public static bool TryGet ( int id, out d_slash_attack value ) => index.TryGetValue ( id, out value );
        public static void InitIndex()
        {
            index = new Dictionary<int, d_slash_attack>();
        }

        public static void Fire ( m_sword_user sender, Vector3 pos, Quaternion rot, SlashAttackSize Size, float rawPower )
        {
            var a = BeginFire ();

            a.origin = sender;
            a.Col.transform.position = pos; a.Col.transform.rotation = rot;
            a.Col2.transform.position = pos; a.Col2.transform.rotation = rot;
            a.targSize = Size;
            a.rawPower = rawPower;
            sender.Weapon.mrr.reactable.Parry += a.Parried;

            a.Col.Collider.size = new Vector3 ( 0, a.targSize.Size.y, a.targSize.Size.z );
            a.Col.Collider.center = Vector3.left * a.targSize.Size.x * 0.5f;

            TriheroesMighty.sp_slash.Emit ( a.Col.transform.position, a.Col.transform.rotation, Size.Size.z * 1.5f );

            a.time = 0;

            EndFire ();
        }

        public override void Create()
        {
            // main slash collider
            Col = new GameObject("slash").AddComponent <SlashAttackCollider> ();
            Col.OnCollisionDetected += OnCollisionDetected;

            Col.gameObject.SetActive (false);
            Col2 = new GameObject("slash2").AddComponent <SlashAttackColliderSecond> ();
            Col2.gameObject.SetActive (false);
            index.Add(Col2.gameObject.GetInstanceID(), this);
        }

        protected override void OnAquire()
        {
            Col.gameObject.SetActive (true);
            Col2.gameObject.SetActive (true);

            Col2.Collider.size = new Vector3 ( targSize.Size.x ,targSize.Size.y,targSize.Size.z);
            Col2.Collider.center = new Vector3(0,0,targSize.zOffset);

            HittedCharacter.Clear ();
        }

        protected override void OnFree()
        {
            Col.gameObject.SetActive (false);
            Col2.gameObject.SetActive (false);
            origin.Weapon.mrr.reactable.Parry -= Parried;
        }

        List<m_attack_receiver> HittedCharacter = new List<m_attack_receiver>();
        void OnCollisionDetected (Collision col)
        {
            if ( m_attack_receiver.TryGet (col.id(), out m_attack_receiver A) && !HittedCharacter.Contains (A) && A.ma.Role.Side != origin.ma.Role.Side )
            { 
                Reaction.Clash ( origin.Weapon.mrr, A.mrr, new Force( ForceType.slash, rawPower , col.contacts[0].point ) );
                HittedCharacter.Add (A);
            }
        }

        void Parried (Force force)
        {
            DeFire (this);
        }

        void Main ()
        {
            if ( time > Col.Duration )
            {
                DeFire (this);
                return;
            }

            Col.Collider.size = new Vector3 ( targSize.Size.x*(time/Col.Duration) ,targSize.Size.y,targSize.Size.z);
            Col.Collider.center = new Vector3(( targSize.Size.x - Col.Collider.size.x) * -0.5f,0,targSize.zOffset);
            
            time += Time.deltaTime;
        }

        public class s_slash_attack : CoreSystem<d_slash_attack>
        {
            protected override void Main(d_slash_attack o)
            {
                o.Main ();
            }
        }

    }

}
