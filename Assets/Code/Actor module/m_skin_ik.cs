using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class m_skin_foot_ik : module
    {
        [Depend]
        m_ground_data mgd;
        [Depend]
        m_skin ms;

        public SkinIk S;

        public Transform[] Foot;
        public float GetLeftFootIkCurves() => S.Ani.GetFloat(Hash.lik);
        public float GetRightFootIkCurves() => S.Ani.GetFloat(Hash.rik);

        public enum FootId { left, right }
        public FootId DominantFoot;

        public override void Create ()
        {
            S = character.GetComponent<ABase>().Skin.gameObject.AddComponent<SkinIk>();
            S.OnIk += FootIk;
            S.LateIk += CheckIkRot;
            Foot = new Transform[] { S.Ani.GetBoneTransform(HumanBodyBones.LeftFoot), S.Ani.GetBoneTransform(HumanBodyBones.RightFoot) };
        }

        Quaternion Rotlik;
        Quaternion Rotrik;

        void FootIk()
        {
            if (!S.ikOn && mgd.onGround)
                S.ikOn = true;

            if (S.ikOn)
            {
                CalculateIkPos();
                CheckCanContinueIk();
            }

            UpdateDominantFoot();
        }

        void CheckIkRot()
        {
            // foot rotation
            Foot[0].rotation = Quaternion.Lerp(Foot[0].rotation, Rotlik, S.iklx);
            Foot[1].rotation = Quaternion.Lerp(Foot[1].rotation, Rotrik, S.ikrx);
        }

        void UpdateDominantFoot()
        {
            if (DominantFoot == FootId.right && GetLeftFootIkCurves() > GetRightFootIkCurves())
                DominantFoot = FootId.left;
            if (DominantFoot == FootId.left && GetLeftFootIkCurves() < GetRightFootIkCurves())
                DominantFoot = FootId.right;
        }

        void CalculateIkPos()
        {
            float PosYl = 0;
            float PosYr = 0;

            S.ikl = Foot[0].position;
            S.ikr = Foot[1].position;
            Rotlik = Foot[0].rotation;
            Rotrik = Foot[1].rotation;

            S.iklx = GetLeftFootIkCurves();
            S.ikrx = GetRightFootIkCurves();

            if (Physics.Raycast(Foot[0].position + Vector3.up * 0.5f, Vector3.down, out RaycastHit hit, 1, Vecteur.Solid))
            {
                PosYl = hit.point.y - ms.Coord.position.y + 0.1f;
                S.ikl = hit.point + new Vector3(0, 0.1f, 0);
                Rotlik = Quaternion.FromToRotation(Foot[0].forward, hit.normal) * Foot[0].rotation;
            }
            if (Physics.Raycast(Foot[1].position + Vector3.up * 0.5f, Vector3.down, out hit, 1, Vecteur.Solid))
            {
                PosYr = hit.point.y - ms.Coord.position.y + 0.1f;
                S.ikr = hit.point + new Vector3(0, 0.1f, 0);
                Rotrik = Quaternion.FromToRotation(Foot[1].forward, hit.normal) * Foot[1].rotation;
            }

            ms.PosY = new Vector3(0, Mathf.Min(PosYl * S.iklx, PosYr * S.ikrx, 0), 0);
        }

        void CheckCanContinueIk()
        {
            // if not on ground, disable Ik
            if (!mgd.onGround)
            {
                ms.PosY = Vector3.zero;
                S.ikOn = false;
            }
        }

    }
}
