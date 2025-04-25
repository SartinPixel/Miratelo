using System;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class m_reactable : indexedmodule <m_reactable>
    {
        public ObjType type;
        public Action <Force> Clash;

        public override void Create1()
        {
            type = character.GetComponent <ABase>().CharacterObjType;
        }
    }
    
    public enum ObjType { skin, metal, wood }
}