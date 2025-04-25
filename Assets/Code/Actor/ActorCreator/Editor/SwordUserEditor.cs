using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixify;
using UnityEditor;

namespace Triheroes.Code
{
    public class SwordUserEditor : ActorEditor.AniExtSampler
    {
        protected override List<SuperKey> GetAnimationKeyList => new List<SuperKey>() {
        AnimationKey.slash_0,
        AnimationKey.slash_1,
        AnimationKey.slash_2
        };

        Transform RefSword;
        Transform RefSlash;

        //TODO find another way of referencing

        public override void CreateGameObjectForReference(ASkin S)
        {
            Transform RH = S.Hand[1];
            RefSword = Loader.LoadIntoScene<Transform>("ref/ref_sword");
            RefSlash = Loader.LoadIntoScene<Transform>("ref/ref_slash");

            RefSword.SetParent(RH);
            RefSword.transform.localPosition = Vector3.zero;
            RefSword.transform.localRotation = Const.SwordDefaultRotation;

            RefSlash.transform.position = RH.transform.position;

            Selection.SetActiveObjectWithContext(RefSlash, RefSlash);
        }
        public override void RemoveGameObjectForReference()
        {
            Object.DestroyImmediate(RefSlash.gameObject);
            Object.DestroyImmediate(RefSword.gameObject);
        }

        public override AniExt.StateExt StateExtUsingAni(Animator Ani, SuperKey key, float t)
        {
            AniExt.StateExt ext = new AniExt.StateExt();
            ext.v3 = RefSlash.position - Ani.GetBoneTransform(HumanBodyBones.RightHand).position;
            ext.q = RefSlash.rotation;
            ext.f = t;
            ext.Key = key;
            return ext;
        }

        public override AniExt.StateExt StateExtOfAni(Animator Ani, SuperKey key)
        {
            AniExt.StateExt ext = new AniExt.StateExt();
            ext.v3 = Ani.GetBoneTransform(HumanBodyBones.RightHand).position;
            ext.Key = key;
            return ext;
        }
    }
}
