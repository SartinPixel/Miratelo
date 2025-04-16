using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class m_standard_character_controller_host : module
    {
        public ac_ground_movement agm;
        public ac_fall af;
        public state_switcher ss {private set; get;}

        public override void Create()
        {
            agm = new ac_ground_movement();
            af = new ac_fall ();
            ss = new state_switcher ( agm, af );
            character.RegisterRoot (ss);
        }
    }
}