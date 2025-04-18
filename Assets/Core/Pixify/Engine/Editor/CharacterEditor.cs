using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pixify
{

    [CustomEditor(typeof (Character))]
    public class CharacterEditor : Editor
    {
        Character Target;
        nodeInspector[] nI;

        void OnEnable()
        {
            Target = target as Character;
        }

        public override void OnInspectorGUI()
        {
            if (EditorApplication.isPlaying)
            ModuleUI ();
        }

        void ModuleUI ()
        {
            if (nI == null)
            CreateInspectors ();

            GUILayout.Label("Pixify Character Engine", PixStyle.o.Title1);
            Rect SectionDefiner;
            for (int i = 0; i < nI.Length; i++)
            {
                SectionDefiner = EditorGUILayout.BeginVertical();
                
                Color color = new Color(.2f, .2f, .2f);
                if ( nI[i].target is module )
                color = new Color (0,0,.2f);
                if ( nI[i].target is core c)
                color = c.on? new Color(.0f, .2f, .0f) : new Color(.2f, .0f, .0f);
                EditorGUI.DrawRect(SectionDefiner, color);

                GUILayout.Label(nI[i].target.GetType().Name, PixStyle.o.Title3);
                nI[i].OnBodyGUI();
                
                EditorGUILayout.EndVertical();
            }

            Repaint ();
        }

        void CreateInspectors ()
        {
            List<module> modules = new List<module> ((typeof (Character).GetField ("modules", BindingFlags.NonPublic | BindingFlags.Instance).GetValue (Target) as Dictionary<Type, module>).Values);

            nI = new nodeInspector [modules.Count];
            for (int i = 0; i < modules.Count; i++)
            {
                nI[i] = nodeInspector.CreateInspector (modules [i]);
            }
        }

    }

}