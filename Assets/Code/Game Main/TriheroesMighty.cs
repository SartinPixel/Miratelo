using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class TriheroesMighty : CoreEngine
    {
        // TODO find another way to hardcode slash
        public static SpectreAbs sp_slash;

        protected override void CreateSystems(out List<CoreSystemBase> Systems)
        {
            // scene hardcoded system
            gameObject.AddComponent <GlobalAI> ();

            // scene hardcoded pool
            d_trajectile.InitPool (32);
            d_slash_attack.InitPool (32);

            // scene hardcoded object
             sp_slash = Loader.LoadIntoScene <SpectreAbs> ( "sp/sp_slash" );

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
                new d_slash_attack.s_slash_attack (),
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