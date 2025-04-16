using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pixify
{
    public abstract class ScriptInit : CharacterData
    {
        public abstract void OnAddScript ( Dictionary <SuperKey, script> scriptHolder );
    }
}