using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class m_standard_character_controller_host : module
    {
        public c_ground_movement cgm;
        public c_fall cf;
        public state_switcher ss {private set; get;}

        public override void Create()
        {
            cgm = new c_ground_movement();
            cf = new c_fall ();
            ss = new state_switcher ( cgm, cf );
            character.ConnectRoot (ss);
        }
    }
}