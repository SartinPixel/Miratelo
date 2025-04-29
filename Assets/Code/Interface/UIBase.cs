using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixify;
using UnityEngine.UI;

namespace Triheroes.Code
{
    public class UIBase : CharacterData
    {
        void Awake ()
        {
            gameObject.AddComponent <Character> ().NeedModule (typeof (m_ui));
        }
        
        public RawImage tr0;
        public RawImage tr1;
    }
}