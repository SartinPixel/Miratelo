using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixify;

namespace Triheroes.Code
{
    public class d_slash_attack : vDot <d_slash_attack>
    {
        SlashAttackCollider Col;
        SlashAttackSize targSize;

        float time;

        public static void Fire ( Vector3 pos, Quaternion rot, SlashAttackSize Size )
        {
            var a = BeginFire ();

            a.Col.transform.position = pos; a.Col.transform.rotation = rot;
            a.targSize = Size;

            a.Col.Collider.size = new Vector3 ( 0, a.targSize.Size.y, a.targSize.Size.z );
            a.Col.Collider.center = Vector3.left * a.targSize.Size.x * 0.5f;

            TriheroesMighty.sp_slash.Emit ( a.Col.transform.position, a.Col.transform.rotation, Size.Size.z * 1.5f );

            a.time = 0;

            EndFire ();
        }

        public override void Create()
        {
            Col = new GameObject("slash").AddComponent <SlashAttackCollider> ();
            Col.OnCollisionDetected += OnCollisionDetected;

            Col.gameObject.SetActive (false);
        }

        protected override void OnAquire()
        {
            Col.gameObject.SetActive (true);
        }

        protected override void OnFree()
        {
            Col.gameObject.SetActive (false);
        }

            
        void OnCollisionDetected (Collision col)
        {

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
