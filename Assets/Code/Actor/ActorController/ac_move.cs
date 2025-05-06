using System;
using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public abstract class ac_move_target_core : action
    {
        [Depend]
        protected m_actor ma;
        [Depend]
        protected m_standard_character_controller_host mscch;
        [Depend]
        protected m_state_stack mss;
        public float Speed;
        public float WalkFactor;

        protected m_actor target;

        protected override void BeginStep()
        {
            if (ma.target == null)
            return;
            
            target = ma.target;
            mss.SetMainState (mscch.ss);
            mss.AquireMainStatePlayer (this);
        }

        protected sealed override bool Step()
        {
            if (ma.target != target)
            return true;

            return Move ();
        }
        
        protected abstract bool Move ();

        protected override void Stop()
        {
            mss.FreeMainStatePlayer (this);
        }
    }

    public class ac_goto_target_agm : ac_move_target_core
    {
        public bool StopWhenDone;
        public float StopDistance = 0.23f;

        float distance;

        public ac_goto_target_agm ( float speed, float walkFactor, float stopDistance, bool stopWhenDone)
        {
            Speed = speed; StopDistance = stopDistance; StopWhenDone = stopWhenDone; WalkFactor = walkFactor;
        }

        protected override void BeginStep()
        {
            base.BeginStep ();

            if (ma.target != null)
            distance = ma.md.r + target.md.r + StopDistance;
        }

        protected override bool Move()
        {
            Vector3 targetPosition = target.md.position;
            Vector3 direction = (targetPosition - ma.md.position).Flat ();

            if ( Vector3.Distance (ma.md.position, targetPosition) > distance )
            mscch.cgm.Walk ( direction.normalized * Speed, WalkFactor);
            else if (StopWhenDone)
            return true;

            return false;
        }
    }

    public class ac_move_arround_target : ac_move_target_core
    {
        public float AngleAmount;

        float angle;

        public ac_move_arround_target ( float speed, float walkFactor, float angleAmount )
        {
            AngleAmount = angleAmount;
            Speed = speed;
            WalkFactor = walkFactor;
        }

        protected override void BeginStep()
        {
            base.BeginStep();
            angle = 0;
        }

        protected override bool Move()
        {
            Vector3 DesiredRotY = Vecteur.RotDirection ( ma.md.position, target.md.position ).OnlyY() + Mathf.Sign ( AngleAmount ) * Vector3.up * 90;
            mscch.cgm.rotDir = DesiredRotY;
            Vector3 DesiredDir = Vecteur.LDir ( DesiredRotY, Speed * Vector3.forward);

            angle += Mathf.DeltaAngle ( Vecteur.RotDirectionY ( target.md.position, ma.md.position ), Vecteur.RotDirectionY ( target.md.position, ma.md.position + DesiredDir * Time.deltaTime/*a*/ * WalkFactor ) );

            mscch.cgm.Walk ( DesiredDir, WalkFactor );

            if (Mathf.Abs (angle) > Mathf.Abs(AngleAmount) )
            return true;

            UIDebug.PushText ( Mathf.DeltaAngle ( angle, AngleAmount ).ToString () );

            return false;
        }
    }
}