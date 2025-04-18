using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class ac_goto_target_agm : action
    {
        [Depend]
        m_actor ma;
        [Depend]
        m_standard_character_controller_host mscch;
        [Depend]
        m_state_player msp;

        public float Speed;
        public bool StopWhenDone;
        public float StopDistance = 0.23f;

        m_actor target;
        float distance;

        public ac_goto_target_agm ( float speed, float stopDistance, bool stopWhenDone)
        {
            Speed = speed; StopDistance = stopDistance; StopWhenDone = stopWhenDone;
        }

        protected override void BeginStep()
        {
            if (ma.target == null)
            return;

            target = ma.target;
            distance = ma.md.r + target.md.r + StopDistance;
            msp.SetState (mscch.ss);
            msp.Aquire (this);
        }

        protected override bool Step()
        {
            if (ma.target != target)
            return true;

            Vector3 targetPosition = target.ms.Coord.position;
            Vector3 direction = (targetPosition - ma.ms.Coord.position).Flat ();

            if ( Vector3.Distance (ma.ms.Coord.position, targetPosition) > distance )
            mscch.agm.Walk ( direction.normalized * Speed );
            else if (StopWhenDone)
            return true;

            return false;
        }

        protected override void Stop()
        {
            msp.Free (this);
        }

    }
}