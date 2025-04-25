using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using UnityEditor.Animations;
using Pixify;

namespace Triheroes.Code
{

    public class ActorEditor : EditorWindow
    {
        static ActorEditor o;
        ASkin S;

        Animator Ani;
        AniExt AniExt;

        AniExtSampler[] Samplers = new AniExtSampler[1] { new SwordUserEditor() };

        bool IsSkinValidToBeSampled => S && AniExt && Ani;
        bool IsSampling => AnimationMode.InAnimationMode();

        public static void ShowWindow(ASkin skin)
        {
            ActorEditor window = GetWindow<ActorEditor>();
            window.Show();
            o = window;
            window.S = skin;
        }

        void OnGUI()
        {
            if (!S)
            return;

            if (GUILayout.Button("Start Sampling"))
                StartSampling();

            if (IsSampling)
                if (IsSkinValidToBeSampled)
                {
                    SampleClipGUI();
                    DrawLine ();
                    ClipSelectionGUI();
                    DrawLine ();
                    SaveToAniExtGUI();
                    DrawLine ();
                    SaveOverrideToSkinGUI();
                }
                else
                    StopSampling();

            if (IsSampling && GUILayout.Button("Stop Sampling"))
                StopSampling();
        }

        void StartSampling()
        {
            if (!S) return;
            if (!S.AniExt) return;

            AnimationMode.StartAnimationMode ();

            AniExt = S.AniExt;
            Ani = S.GetComponent<Animator>();
        }

        void StopSampling()
        {
            currentClip = null;
            SetCurrentSampler(null);

            AnimationMode.EndSampling();
            AnimationMode.StopAnimationMode();
            SceneView.RepaintAll();
        }

        
        void DrawLine ()
        {
            EditorGUI.DrawRect ( new Rect (GUILayoutUtility.GetLastRect().position.x,GUILayoutUtility.GetLastRect().position.y+GUILayoutUtility.GetLastRect().size.y+2,GUILayoutUtility.GetLastRect().size.x,2) , Color.black );
            GUILayout.Space (6);
        }

        SuperKey currentKey;
        AnimationClip currentClip;
        AniExtSampler currentSampler;

        float sampleTime;

        void SampleClipGUI()
        {
            if (!currentClip)
                return;

            GUILayout.Label( string.Concat ( "Time:" , sampleTime.ToString() ), PixStyle.o.Subtitle1 );
            sampleTime = EditorGUILayout.Slider(sampleTime, 0, currentClip.length);
            AnimationMode.SampleAnimationClip(S.gameObject, currentClip, sampleTime);
        }

        void ClipSelectionGUI()
        {
            GUILayout.Label ("Choose clip", PixStyle.o.Title3);
            foreach (var s in Samplers)
                {
                GUILayout.Label ( s.GetType().Name ,PixStyle.o.Subtitle1);
                foreach (var k in s.AnimationKeyList)
                    if (GUILayout.Button(k.keyName))
                    {
                        SelectClip(k);
                        SetCurrentSampler(s);
                        currentKey = k;
                    }
                }
        }
        
        void SetCurrentSampler(AniExtSampler sampler)
        {
            if (currentSampler != null)
                currentSampler.RemoveGameObjectForReference();

            currentSampler = sampler;

            if (currentSampler != null && Ani != null)
                currentSampler.CreateGameObjectForReference(S);
        }

        void SelectClip(SuperKey k)
        {
            currentClip = FindClipInAniExt(k);
            sampleTime = 0;
        }

        void SaveToAniExtGUI()
        {
            if (currentSampler == null || !currentClip) return;

            if (currentClip && GUILayout.Button("Save to AniExt"))
            {
                AniExt.StatesExt.AddOrChange(currentKey, currentSampler.StateExtUsingAni(Ani, currentKey, sampleTime));
                EditorUtility.SetDirty(AniExt);
            }
        }

        // TODO block it from saving in prephabs
        void SaveOverrideToSkinGUI()
        {
            if (GUILayout.Button("Create StateExt for Skin"))
            {
                S.StateExt = new List<AniExt.StateExt>();

                AnimationClip tempclip;
                AniExt.StateExt temp;

                foreach (var s in Samplers)
                    foreach (var k in s.AnimationKeyList)
                    {
                        if (AniExt.StatesExt.TryGetValue(k, out temp))
                        {
                            tempclip = FindClipInAniExt(k);

                            if (!tempclip)
                                continue;

                            AnimationMode.SampleAnimationClip(S.gameObject, tempclip, temp.f);
                            S.StateExt.Add(s.StateExtOfAni(Ani, k));
                        }
                    }

                EditorUtility.SetDirty(S);
            }
        }

        AnimationClip FindClipInAniExt(SuperKey k)
        {
            AnimatorControllerLayer[] layers = ((AnimatorController)AniExt.Model).layers;
            List<ChildAnimatorState> States = new List<ChildAnimatorState>();

            foreach (var l in layers)
                States.AddRange(l.stateMachine.states);

            return States.Find(x => x.state.motion is AnimationClip && string.Equals(x.state.name, k.keyName)).state.motion as AnimationClip;
        }

        public abstract class AniExtSampler
        {
            public abstract AniExt.StateExt StateExtUsingAni(Animator Ani, SuperKey key, float t);
            public abstract AniExt.StateExt StateExtOfAni(Animator Ani, SuperKey key);
            public abstract void CreateGameObjectForReference(ASkin S);
            public abstract void RemoveGameObjectForReference();
            protected abstract List<SuperKey> GetAnimationKeyList { get; }
            public List<SuperKey> AnimationKeyList;

            public AniExtSampler()
            {
                AnimationKeyList = GetAnimationKeyList;
            }
        }

        class ScriptPostprocessor : AssetPostprocessor
        {
            // stop sampling on recompilation
            protected void OnPreprocessAsset()
            {
                if (o != null && assetPath.EndsWith(".cs", StringComparison.OrdinalIgnoreCase) ||
                        assetPath.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                    if (o.IsSampling)
                        o.StopSampling();
            }
        }
    }

    [InitializeOnLoad]
    class CharEditorIcon
    {
        static CharEditorIcon()
        {
            EditorApplication.hierarchyWindowItemOnGUI += CharacterEditorIconGUI;
        }

        private static void CharacterEditorIconGUI(int instanceID, Rect selectionRect)
        {
            var obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (obj == null)
                return;

            if (obj.GetComponent<ASkin>())
            {
                Rect r = new Rect(new Vector2(selectionRect.x + selectionRect.width - 32, selectionRect.y), new Vector2(38, selectionRect.height));
                if (GUI.Button(r, "Skin"))
                {
                    ActorEditor.ShowWindow(obj.GetComponent<ASkin>());
                }
            }
        }
    }

}
