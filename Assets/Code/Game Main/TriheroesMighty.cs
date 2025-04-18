
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
            // scene hardcoded system
            gameObject.AddComponent <GlobalAI> ();

            // scene hardcoded pool
            d_trajectile.InitPool (32);

            Systems = new List<CoreSystemBase>()
            {
                // character physic data
                new s_capsule_character_controller_ground_data (),

                // procedural animation
                new s_skin_produceral_bow (),

                // character controller and AI
                new s_character_controller (),
                new s_state_player (),

                // active attacks
                new d_trajectile.s_trajectile (),

                // character physic
                new s_ccc_gravity (),
                new s_move (),

                // character graphic and animation event
                new s_skin ()
            };
        }
    }
}