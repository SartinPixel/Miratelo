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

        int ComboId = 0;
        bool nextComboReady;

        public ac_slash () {}
        public ac_slash ( int ComboId )
        {
            this.ComboId = ComboId;
        }

        protected override void BeginStep()
        {
            if (!msu.on)
            EndSlash ();

            if (msu.state == StateKey.zero)
            BeginSlash (ComboId);
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

            // slash attack
            ms.PlayState ( 0, m_sword_user.SlashKeys [id] , 0.1f, ComboEnd, null, Slash );
            msu.state = StateKey.slash;
            currentSlashId = id;

            // send alert
            Collider[] NearbyChar = Physics.OverlapSphere ( msu.Weapon.transform.position, msu.Weapon.Lenght * 1.5f, Vecteur.Character );
            for (int i = 0; i < NearbyChar.Length; i++)
            {
                if ( m_slash_alert.TryGet ( NearbyChar[i].id(), out m_slash_alert A ) )
                A.AlertSlash ( ms.EventPointsOfState ( m_sword_user.SlashKeys [id] ) [0], m_sword_user.ParryKeys [id] );
            }
        }

        public void ComboAppend ()
        {
            nextComboReady = true;
        }

        void Slash ()
        {
            d_slash_attack.Fire ( msu, ms.Coord.position + Vecteur.LDir ( ms.rotY, msu.SlashPos [currentSlashId] ), Quaternion.Euler (ms.rotY) * msu.SlashRot [currentSlashId], msu.Weapon.slashSize, msu.Weapon.XPower );
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
        public void Set ( d_trajectile trajectileToParry )
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
                Reaction.Clash ( msu.Weapon.mrr , trajectile.mrr, new Force (ForceType.parry,0,trajectile.position,Vecteur.LDir ( ms.rotY, Vector3.forward )) );
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

    public sealed class ac_slash_parry : action
    {
        [Depend]
        m_sword_user msu;
        [Depend]
        m_skin ms;

        bool ParryActived;
        SuperKey parryKey;
        d_parry_attack ParryAttack;

        public void Set ( SuperKey parryKey )
        {
            this.parryKey = parryKey;
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
            ms.PlayState ( 0, parryKey, 0.1f, AppendStop, null, ActiveParry, StopParry );
            msu.state = StateKey.parry;
            ParryActived = false;
        }

        void ActiveParry ()
        {
            ParryActived = true;
            ParryAttack = d_parry_attack.Fire ( msu.Weapon );
        }

        void StopParry()
        {
            if (ParryActived)
            {
            d_parry_attack.DeFire ( ParryAttack );
            ParryActived = false;
            }
        }

        protected override void Stop()
        {
            if ( msu.state == StateKey.parry )
            msu.state = StateKey.zero;

            if (ParryActived)
            {
            d_parry_attack.DeFire ( ParryAttack );
            ParryActived = false;
            }
        }
    }
}
