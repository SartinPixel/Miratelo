using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class d_t_explosive : vDot <d_t_explosive>
    {
        d_trajectile host;
        float rawPower;
        public static d_t_explosive Fire ( d_trajectile host, float rawPower )
        {
            var a = BeginFire ();

            host.mrr.reactable.Blocked += a.Blocked;
            a.host = host;
            a.rawPower = rawPower;
            host.Defired += a.Blocked;

            EndFire ();
            return a;
        }

        void Blocked (Force force) => Blocked ();

        void Blocked ()
        {
            // TODO don't hardcode radius
            d_explosion.Fire ( host.position, rawPower, 2 );
            DeFire (this);
        }

        protected override void OnFree()
        {
            host.mrr.reactable.Blocked -= Blocked;
            host.Defired -= Blocked;
        }
    }

}