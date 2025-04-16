using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pixify
{
    public class PixStyle
    {
        public static PixStyle o
        {
            get 
            {
                if (_o== null)
                _o = new PixStyle();
                return _o;
            }
        }
        static PixStyle _o;


        public GUIStyle Title1;
        public GUIStyle Title2;
        
        public GUIStyle Title3;
        public GUIStyle Subtitle1;

        public PixStyle ()
        {
            GUIStyle Base = new GUIStyle ("Label");
            
            Title1 = new GUIStyle (Base);
            Title1.fontStyle = FontStyle.Bold;
            Title1.fontSize = 18;
            Title2 = new GUIStyle (Title1);
            Title2.fontSize = 14;
            Title2.normal.textColor = new Color ( 1, 0.25f, 0);
            Title3 = new GUIStyle (Title2);
            Title3.normal.textColor = new Color ( 0, 0.8f, 1);
            Title3.alignment = TextAnchor.UpperLeft;
            Subtitle1 = new GUIStyle (Title1);
            Subtitle1.fontSize = 8;
        }

    }
}
