using System;
using System.Reflection;
using UnityEngine;

namespace Pixify
{
    /// <summary>
    /// this attribute will let field to be serialized in paper // must be public
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ExportAttribute : Attribute { }

    public static class Loader
    {
        public static T CopyWithExport <T> ( T a )
        {
            T b = (T) Activator.CreateInstance (a.GetType());
            foreach ( FieldInfo fi in a.GetType().GetFields() )
            {
                if (fi.IsPublic && fi.GetCustomAttribute<ExportAttribute>() != null)
                fi.SetValue ( b ,fi.GetValue ( a ) );
            }
            return b;
        }

        public static T LoadIntoScene <T> ( string path ) where T: UnityEngine.Object
        {
            T o = Resources.Load <T> (path);

            if (!o)
            {
            Debug.LogError ( string.Concat ("hardcoded Resources not existing in ",path) );
            return null;
            }
            else
            return ScriptableObject.Instantiate ( o );
        }

        public static bool Compare (  float A, float B, Comparator comparator)
        {
            switch (comparator)
            {
                case Comparator.less:
                return A<B;
                case Comparator.more:
                return A>B;
                case Comparator.equal:
                return A==B;
                case Comparator.lessEqual:
                return A<=B;
                case Comparator.moreEqual:
                return A>=B;
            }
            return false;
        }
    }
    
    public enum Comparator { less, more, equal, lessEqual, moreEqual }
}
