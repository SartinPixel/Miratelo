using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    [RegisterAsBase]
    public class m_state_energy_auto : m_stat_auto
    {

        public float energy {
            set {
                if ( value < _energy )
                {
                canRestore = false;
                enabled = true;
                }

                _energy = Mathf.Clamp ( value, 0, MaxEnergy );
            } 
            get => _energy;
            }

        float _energy;
        bool canRestore = false;
        float delay;

        // param
        public float MaxEnergy;
        public float DelayTime;
        public float RestorationPerSecond;

        public override void Create()
        {
            energy = MaxEnergy;
            Aquire ( new Null() );
        }

        protected override void OnAquire()
        {
            enabled = false;
        }

        public override void Main()
        {
            if ( !canRestore )
            {
                if ( delay >= DelayTime )
                {
                canRestore = true;
                delay = 0;
                }
                else
                delay += Time.deltaTime;
            }

            if ( canRestore )
            energy += RestorationPerSecond * Time.deltaTime;

            if (energy == MaxEnergy)
            {
                enabled = false;
                canRestore = false;
            }
        }

    }
}