using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class ac_slash : action
    {

        [Depend]
        m_sword_user msu;
        [Depend]
        m_skin ms;

        bool nextComboReady;

        protected override void BeginStep()
        {
            if (!msu.on)
            EndSlash ();

            if (msu.state == StateKey.zero)
            BeginSlash (0);
            else
            EndSlash ();

            nextComboReady = false;
        }

        protected override void Stop()
        {
            if (msu.state == StateKey.slash)
            msu.state = StateKey.zero;
        }

        int currentSlashId;
        void BeginSlash ( int id )
        {
            if (!msu.on)
            Debug.LogError ("this character is trying to play slash animation on a character that is not using sword");

            ms.PlayState ( 0, m_sword_user.SlashKeys [id] , 0.1f, ComboEnd, null, Slash );
            msu.state = StateKey.slash;
            currentSlashId = id;
        }

        public void ComboAppend ()
        {
            nextComboReady = true;
        }

        void Slash ()
        {
            d_slash_attack.Fire ( ms.Coord.position + Vecteur.LDir ( ms.RotY, msu.SlashPos [currentSlashId] ), Quaternion.Euler (ms.RotY) * msu.SlashRot [currentSlashId], msu.Weapon.slashSize );
        }

        void ComboEnd ()
        {
            if ( nextComboReady )
            {
                if (ms.CurrentStateEqualTo (0, AnimationKey.slash_0))
                    BeginSlash( 1 );
                else if (ms.CurrentStateEqualTo (0, AnimationKey.slash_1))
                    BeginSlash( 2 );
                else if (ms.CurrentStateEqualTo (0, AnimationKey.slash_2))
                    BeginSlash( 0 ); 
                nextComboReady = false;
            }
            else
                EndSlash();
        }

        void EndSlash ()
        {
            AppendStop ();
        }

    }
}
