using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixify;

namespace Triheroes.Code
{
    public class m_skin_produceral_bow : core
    {
        [Depend]
        m_bow_user mbu;
        [Depend]
        m_skin ms;

        public Transform bow => mbu.Weapon.transform;

        public enum isRotatingWithBowState { disabled, running, hold }
        public isRotatingWithBowState isRotatingWithBow;
        public Vector3 TargetRotY;

        // variable needed for interpolation
        public Quaternion weakDiff;

        // cached transform needed for the body transformation
        public Transform Spine;

        public override void Create()
        {
            Spine = ms.Ani.GetBoneTransform(HumanBodyBones.Spine);
        }
    }
}
