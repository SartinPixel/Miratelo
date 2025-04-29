using System;
using System.Collections.Generic;
using Pixify;
using UnityEngine;
using UnityEngine.UI;

namespace Triheroes.Code
{
    public class m_black_bar : module
    {
        public static m_black_bar o;
        RawImage tr0;
        RawImage tr1;

        float x;
        float targetX = 9;
        mt_push mp;

        public override void Create()
        {
            o = this;

            tr0 = character.GetComponent <UIBase> ().tr0;
            tr1 = character.GetComponent <UIBase> ().tr1;

            mp = character.ConnectNode ( new mt_push() );
            mp.Aquire (this);
        }

        public void Push ( float X )
        {
            if (targetX != X)
            {
            targetX = X;
            mp.Set ( X, Getx, Setx );
            }

            float Getx () => x;
            void Setx ( float X )
            {
                x = X;
                tr0.rectTransform.sizeDelta = new Vector2 ( tr0.rectTransform.sizeDelta.x , x );
                tr1.rectTransform.sizeDelta = new Vector2 ( tr1.rectTransform.sizeDelta.x , x );
            }
        }

    }
}
