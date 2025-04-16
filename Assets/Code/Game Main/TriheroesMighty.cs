using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class TriheroesMighty : CoreEngine
    {
        protected override void CreateSystems(out List<CoreSystemBase> Systems)
        {
            Systems = new List<CoreSystemBase>()
            {
                new s_capsule_character_controller_ground_data (),

                new s_character_controller (),
                new s_state_player (),

                new s_ccc_gravity (),

                new s_move (),
                new s_skin ()
            };
        }
    }
}