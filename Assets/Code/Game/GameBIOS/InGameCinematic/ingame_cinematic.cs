using System.Collections;
using System.Collections.Generic;
using Pixify;
using static Pixify.treeBuilder;
using UnityEngine;

namespace Triheroes.Code
{
    public class in_game_cinematic : bios
    {
        protected override void OnAquire()
        {
            for (int i = 0; i < play.ActorCount; i++)
            {
                TreeStart ( play.GetMainCharacter(i) );
                play.GetMainCharacter(i).RequireModule <m_character_controller> ().StartRoot ( PlayerCinematicActorLibrary.Dummy () );
            }
        }

        public override void Main()
        {}
    }
}
