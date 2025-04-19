using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Triheroes.Code
{
    public class ASkin : MonoBehaviour
    {
        // metadata
        public AniExt AniExt;

        // extra metadata
        [SerializeField]
        public List <AniExt.StateExt> StateExt;

        [Header("common transform")]
        public Transform[] Hand;

        public Transform SwordPlace;
        public Transform BowPlace;

        #region editortool
        #if UNITY_EDITOR
            [ContextMenu("Add Hand transform")]
            void AddHandHandler()
            {
                Animator Ani = GetComponent<Animator>();
                GameObject A = new GameObject("LH"); A.transform.SetParent(Ani.GetBoneTransform(HumanBodyBones.LeftHand));
                A.transform.localPosition = Vector3.zero;
                GameObject B = new GameObject("RH"); B.transform.SetParent(Ani.GetBoneTransform(HumanBodyBones.RightHand));
                B.transform.localPosition = Vector3.zero;
                Hand = new Transform[2] { A.transform, B.transform };
            }
        #endif
        #endregion
    }
}
