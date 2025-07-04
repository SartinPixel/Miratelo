using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixify;

namespace Triheroes.Code
{
    // for players character with humanoid characteristics
    // don't need character controller here, automatically added by game master
    public class StandardPlayerAuthor : Scripter
    {
        [Header("Skin (appearance)")]
        public skin_writer skin;
        [Header("Actor definition")]
        public actor_writer actor;
        [Header("Stats")]
        public stat_writer stat;
        [Header("Skills")]
        public skill_writer skill;

        override public ModuleWriter[] GetModules ()
        {
            return new ModuleWriter[] { skin, actor, stat, skill };
        }

        override public void OnSpawn ( Vector3 position, Quaternion rotation, Character c )
        {
            c.GetModule <m_skin> ().rotY = rotation.eulerAngles;
        }
    }
}