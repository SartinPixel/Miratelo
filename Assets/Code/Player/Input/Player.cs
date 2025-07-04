using System;
using System.Collections.Generic;
using UnityEngine;
using Pixify;

namespace Triheroes.Code
{
    public class Player : module
    {
        public override void Create()
        {
            VMove = New <InputAction> (character);
            HMove = New <InputAction> (character);
            MouseX = New <InputAction> (character);
            MouseY = New <InputAction> (character);
            Jump = New <InputAction> (character);
            Action1 = New <InputAction> (character);
            Action2 = New <InputAction> (character);
            Action3 = New <InputAction> (character);
            Focus = New <InputAction> (character);
            Dash = New <InputAction> (character);
            Alt = New <InputAction> (character);
            Aim = New <InputAction> (character);
            HatDown = New <InputAction> (character);

            VMove.Set("Vertical", true);
            HMove.Set("Horizontal", true);
            MouseX.Set("Mouse X", true);
            MouseY.Set("Mouse Y", true);
            Jump.Set("Action0");
            Action1.Set("Action1");
            Action2.Set("Action2");
            Action3.Set("Action3");
            Focus.Set("L2",true);
            Dash.Set("R2",true);
            Alt.Set("L1");
            Aim.Set("R1");
            HatDown.Set("DPadDown",true);
        }

        public static Vector3 MoveAxis3 => new Vector3(HMove.Raw,0, VMove.Raw);
        public static Vector2 DeltaMouse => new Vector2(MouseX.Raw, MouseY.Raw);

        public static InputAction VMove, HMove, MouseX, MouseY, Jump, Action1, Action2, Action3, Focus, Dash, Alt, Aim, HatDown;
    }

    public sealed class InputAction : bios
    {

        /// <summary>
        /// is the corresponding button held down
        /// </summary>
        public bool Active { get; private set; }
        /// <summary>
        /// is the corresponding button pressed down this frame
        /// </summary>
        public bool OnActive => _OnDown;
        /// <summary>
        /// is the corresponding button released this frame
        /// </summary>
        public bool OnRelease => _OnUp;
        /// <summary>
        /// raw value of the corresponding button
        /// </summary>
        public float Raw => _IsAxis ? Input.GetAxis ( _InputManagerAccessName ) : Input.GetButton ( _InputManagerAccessName )? 1f : 0f;

        bool _OnUp;
        bool _OnDown;
        bool _IsAxis;
        string _InputManagerAccessName;

        public void Set ( string InputManagerAccessName, bool IsInputManagerAccessNameAxis = false )
        {
            _IsAxis = IsInputManagerAccessNameAxis;
            _InputManagerAccessName = InputManagerAccessName;
            Aquire (this);
        }

        public override void Main()
        {
            _OnDown = false;
            _OnUp = false;

            if (Active == false)
            {
                if ( (_IsAxis && Mathf.Abs (Raw)>0.1f) || (!_IsAxis && Input.GetButton ( _InputManagerAccessName )) )
                {
                    _OnDown = true;
                    Active = true;
                }
            }
            else
            {
                if ( (_IsAxis && Mathf.Abs (Raw)<0.1f) || (!_IsAxis && !Input.GetButton ( _InputManagerAccessName )) )
                {
                    _OnUp = true;
                    Active = false;
                }
            }
        }

    }
}