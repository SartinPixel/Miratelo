using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    // ground data used by movement modules like m_capsule_character_controller
    public class d_ground_data : pix
    {
        /// <summary>
        /// character is on the ground physically
        /// </summary>
        public bool onGroundAbs = true;
        /// <summary>
        /// character is on the ground visually
        /// </summary>
        public bool onGround = true;
        public Vector3 groundNormal = Vector3.up;
    }
}