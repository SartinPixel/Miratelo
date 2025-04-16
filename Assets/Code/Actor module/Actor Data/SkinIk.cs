using System;
using UnityEngine;

namespace Triheroes.Code
{
    public class SkinIk : MonoBehaviour
    {
        public Animator Ani;
        public bool ikOn, _ikOn;

        public Vector3 ikr;
        public Vector3 ikl;

        public float ikrx;
        public float iklx;

        public Action OnIk;

        public Action LateIk;


        void Awake()
        {
            Ani = GetComponent<Animator>();
        }

        void LateUpdate()
        {
            LateIk?.Invoke();
        }

        void OnAnimatorIK()
        {
            OnIk?.Invoke();

            if (_ikOn)
            {
                Ani.SetIKPosition(AvatarIKGoal.LeftFoot, ikl);
                Ani.SetIKPosition(AvatarIKGoal.RightFoot, ikr);
                Ani.SetIKPositionWeight(AvatarIKGoal.LeftFoot, iklx);
                Ani.SetIKPositionWeight(AvatarIKGoal.RightFoot, ikrx);

                if (ikOn == false)
                {
                    _ikOn = false;
                    iklx = 0;
                    ikrx = 0;
                    Ani.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0);
                    Ani.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0);
                }
            }
            else
            {
                if (ikOn)
                    _ikOn = true;
            }
        }
    }
}
