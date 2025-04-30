using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public sealed class ac_slash : action
    {
        [Depend]
        m_sword_user msu;
        [Depend]
        m_skin ms;
        [Depend]
        m_actor ma;

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
            d_slash_attack.Fire ( msu, ms.Coord.position + Vecteur.LDir ( ms.rotY, msu.SlashPos [currentSlashId] ), Quaternion.Euler (ms.rotY) * msu.SlashRot [currentSlashId], msu.Weapon.slashSize );
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

    public sealed class ac_slash_parry_trajectile : action
    {
        [Depend]
        m_sword_user msu;
        [Depend]
        m_skin ms;

        d_trajectile trajectile;
        public void SetTrajectile ( d_trajectile trajectileToParry )
        {
            trajectile = trajectileToParry;
        }

        protected override void BeginStep()
        {
            if ( !msu.on )
            {
            AppendStop();
            return;
            }

            if ( msu.state == StateKey.zero )
            Begin ();
            else
            Debug.LogError ("this character is trying to parry but the sword_user is not ready");
        }

        void Begin ()
        {
            ms.PlayState ( 0, AnimationKey.parry_0 , 0.1f, AppendStop );
            msu.state = StateKey.parry_trajectile;
        }

        protected override bool Step()
        {
            if ( trajectile!= null && trajectile.on )
            {
                if ( Vector3.Distance ( trajectile.position, msu.Weapon.transform.position ) < ( msu.Weapon.Lenght + ( trajectile.speed * Time.deltaTime ) ) && Mathf.Abs ( Mathf.DeltaAngle ( Vecteur.RotDirectionY ( ms.Coord.position, trajectile.position ), ms.rotY.y ) ) < 90 )
                {
                Reaction.Clash ( msu.Weapon.mrr , trajectile.mrr, new Force (ForceType.perce_parry,0,trajectile.position,Vecteur.LDir ( ms.rotY, Vector3.forward )) );
                trajectile = null;
                }
            }
            return false;
        }

        protected override void Stop()
        {
            if ( msu.state == StateKey.parry_trajectile )
            msu.state = StateKey.zero;
        }
    }
}
