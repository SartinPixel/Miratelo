using System.Collections;
using System.Collections.Generic;
using Pixify;
using Triheroes.Code;
using UnityEngine;

public class test_stamina_injector : CharacterData
{
    public float MaxEnergy = 7;
    public float DelayTime = 2;
    public float RestorationPerSecond = 1;

    void Start ()
    {
        var msea = GetComponent <Character> ().NeedModule ( typeof (m_state_energy_auto) ) as m_state_energy_auto;
        msea.MaxEnergy = MaxEnergy;
        msea.DelayTime = DelayTime;
        msea.RestorationPerSecond = RestorationPerSecond;
        msea.energy = MaxEnergy;
    }
}