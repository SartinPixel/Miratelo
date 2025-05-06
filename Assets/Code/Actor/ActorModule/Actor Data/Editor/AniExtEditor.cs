using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using Pixify;

namespace Triheroes.Code
{

    [CustomEditor(typeof(AniExt))]
    public class AniExtEditor : Editor
    {
        AniExt Target;


        void OnEnable()
        {
            Target = (AniExt)target;
            if (Target.States == null)
                Target.States = new Dictionary<SuperKey, AniExt.State>();
        }

        public override void OnInspectorGUI()
        {
            Target.Model = (RuntimeAnimatorController)EditorGUILayout.ObjectField(Target.Model, typeof(RuntimeAnimatorController), false);

            if (Target.Model && GUILayout.Button("Generate / Update Data"))
                GenerateData();

            base.OnInspectorGUI();
        }

        void GenerateData()
        {
            AnimatorControllerLayer[] layers = ((AnimatorController)Target.Model).layers;
            List<bool> realLayer = new List<bool>();
            Target.States.Clear();

            for (int i = 0; i < layers.Length; i++)
            {
                if (layers[i].syncedLayerIndex == -1)
                {
                    GenerateData(layers[i].stateMachine, ref Target.States);
                    realLayer.Add(true);
                }
                else
                    realLayer.Add(false);
            }

            Target.RealLayer = realLayer.ToArray();
            EditorUtility.SetDirty(Target);
        }

        void GenerateData(AnimatorStateMachine StateMachine, ref Dictionary<SuperKey, AniExt.State> States)
        {
            var states = StateMachine.states;
            foreach (var s in states)
            {
                if (States.ContainsKey(new SuperKey(s.state.name)))
                    continue;

                if (s.state.motion is AnimationClip c)
                {
                    float[] ef = new float[c.events.Length];

                    for (int i = 0; i < c.events.Length; i++)
                    {
                        ef[i] = c.events[i].time / s.state.speed;
                    }

                    States.Add(new SuperKey(s.state.name),
                    new AniExt.State()
                    {
                        Key = new SuperKey(s.state.name),
                        Duration = c.isLooping ? Mathf.Infinity : c.length / s.state.speed,
                        EvPoint = ef
                    }
                    );
                }
                else
                    States.Add(new SuperKey(s.state.name),
                       new AniExt.State()
                       {
                           Key = new SuperKey(s.state.name),
                           Duration = Mathf.Infinity
                       }
                       );
            }
        }

    }

}
