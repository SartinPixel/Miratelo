using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{

    [CreateAssetMenu (menuName = "RPG/trajectile paper") ]
    public class TrajectilePaper : ScriptableObject
    {
        public DotSkin Skin;

        // TODO make this modular
        public enum extension {none, explosive}
        public extension Extension;

        public float ExpPower;

        public void Shoot ( Vector3 pos, Quaternion rot, float speed, float rawPower )
        {
            var trajectile = d_trajectile.Fire ( Skin, pos, rot, speed, rawPower );
            switch ( Extension )
            {
                case extension.explosive:
                d_t_explosive.Fire ( trajectile, ExpPower );
                break;
            }
        }
    }

}