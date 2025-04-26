using System;
using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;


namespace Triheroes.Code
{
    public class CameraBase : CharacterData
    {
        public Transform CameraPivot;
        public Camera Cam;

        void Awake ()
        {
            gameObject.AddComponent <Character> ().NeedModule ( typeof (m_camera) );
        }
    }
}