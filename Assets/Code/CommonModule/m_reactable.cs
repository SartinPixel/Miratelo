using System;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class m_reaction_receiver : indexedmodule <m_reaction_receiver>
    {
        public reactable reactable;

        public override void Create1()
        {
            var type = character.GetComponent <ABase>().CharacterObjType;

            switch (type)
            {
                case ObjType.skin:
                reactable = new r_skin ();
                break;
            }

            character.ConnectNode ( reactable );
        }
    }

    public abstract class reactable : node
    {
        public abstract void Clash (Force force, reactable from);
        
        // generic reaction
        public Action <Force> Parry;
        
        public Action <Force> Hit;
    }

    public enum ObjType { skin, metal, wood }
}