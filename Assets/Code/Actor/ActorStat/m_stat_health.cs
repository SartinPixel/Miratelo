using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class m_stat_health : module
    {
        public float HP
        { 
            set {
            _HP = Mathf.Clamp ( value, 0, MaxHP );
            }
            get => _HP;
        }
        float _HP;

        public float MaxHP;

        public override void Create()
        {
            HP = MaxHP;
        }
    }
}