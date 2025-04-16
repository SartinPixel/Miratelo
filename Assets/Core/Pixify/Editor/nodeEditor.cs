using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Pixify
{
    [nodeInspectorOf (typeof (node))]
    public class nodeInspector
    {
        public node target;

        public virtual void OnCreate() {}

        public virtual void OnBodyGUI() => NodeInspector (target);

        void NodeInspector (node node)
        {
            foreach (FieldInfo fi in node.GetType().GetFields( BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance ))
            {
                if (fi.GetCustomAttribute<HideInInspector>() == null)
                    NGUILayout.FieldGUI(fi, node);
            }
        }

        public static nodeInspector CreateInspector (node target)
        {
            var Y = System.AppDomain.CurrentDomain.GetAssemblies();
            List<Type> AllNodeInspector = new List<Type>();

            foreach (var y in Y)
            foreach (Type x in y.GetTypes())
            {
                if (x.IsSubclassOf(typeof(nodeInspector)))
                    AllNodeInspector.Add(x);
            }
            
            Type Current = typeof(nodeInspector);
            foreach (Type t in AllNodeInspector)
            {
                Type NodeTypeOfThis = t.GetCustomAttribute<nodeInspectorOfAttribute>().NodeType;

                if ((target.GetType().IsSubclassOf(NodeTypeOfThis) || target.GetType() == NodeTypeOfThis) && NodeTypeOfThis.IsSubclassOf(Current.GetCustomAttribute<nodeInspectorOfAttribute>().NodeType))
                    Current = t;
            }

            nodeInspector nI = (nodeInspector) Activator.CreateInstance(Current);
            nI.target = target;
            nI.OnCreate();

            return nI;
        }
    }


    [AttributeUsage(AttributeTargets.Class)]
    public class nodeInspectorOfAttribute : Attribute
    {
        public Type NodeType;
        public nodeInspectorOfAttribute(Type Class)
        { NodeType = Class; }
    }


    public static class NGUILayout
    {
        public static void FieldGUI(FieldInfo fi, object o)
        {
            if (fi.FieldType == typeof(bool))
            {
                fi.SetValue(o, GUILayout.Toggle((bool)fi.GetValue(o), fi.Name));
                return;
            }
            if (fi.FieldType == typeof(int))
            {
                fi.SetValue(o, EditorGUILayout.IntField(fi.Name, (int)fi.GetValue(o)));
                return;
            }
            if (fi.FieldType == typeof(string))
            {
                fi.SetValue(o, EditorGUILayout.TextField(fi.Name, (string)fi.GetValue(o)));
                return;
            }

            if (fi.FieldType == typeof(string[]))
            {
                List<string> vars = new List<string>();
                vars.AddRange((string[])fi.GetValue(o));
                if (GUILayout.Button("++"))
                    vars.Add("");
                for (int i = 0; i < vars.Count; i++)
                {
                    GUILayout.BeginHorizontal();
                    vars[i] = EditorGUILayout.TextField(vars[i]);
                    if (GUILayout.Button("X", GUILayout.Width(16), GUILayout.Height(16)))
                    {
                        vars.RemoveAt(i); break;
                    }
                    GUILayout.EndHorizontal();
                }
                fi.SetValue(o, vars.ToArray());
                return;
            }


            if (fi.FieldType == typeof(float))
            {
                fi.SetValue(o, EditorGUILayout.FloatField(fi.Name, (float)fi.GetValue(o)));
                return;
            }

            if (fi.FieldType == typeof(Vector3))
            {
                fi.SetValue(o, EditorGUILayout.Vector3Field(fi.Name, (Vector3)fi.GetValue(o)));
                return;
            }

            if (fi.FieldType == typeof(Color))
            {
                fi.SetValue(o, EditorGUILayout.ColorField(fi.Name, (Color)fi.GetValue(o)));
                return;
            }

            if (fi.FieldType.IsEnum)
            {
                fi.SetValue(o, EditorGUILayout.EnumPopup(fi.Name, (Enum)fi.GetValue(o)));
                return;
            }

            if (fi.FieldType == typeof (SuperKey))
            {
                fi.SetValue (o, new SuperKey(EditorGUILayout.TextField ( fi.Name,((SuperKey) fi.GetValue(o)).keyName )) );
            }
            
            if (fi.FieldType.IsSubclassOf(typeof(UnityEngine.Object)))
            {
                    fi.SetValue(o, EditorGUILayout.ObjectField(fi.Name, (UnityEngine.Object)fi.GetValue(o), fi.FieldType, true));
            }
        }
    }

}
