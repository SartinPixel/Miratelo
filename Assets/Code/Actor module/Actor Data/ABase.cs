using System;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    /// <summary>
    /// Basic actor data
    /// </summary>
    public class ABase : CharacterData
    {
        [Header ("Skin Settings")]
        public ASkin Skin;
        public bool HumanFootIk = true;
        public Weapon [] AttachedWeapon;
    }
}
