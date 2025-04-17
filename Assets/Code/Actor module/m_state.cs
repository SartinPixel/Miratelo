using System;
using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class m_state_player : core
    {
        public action state {private set; get;}

        /// <summary>
        /// aquire state player, don't forget to set a state before
        /// </summary>
        protected sealed override void OnAquire()
        {
            if (!state.on)
            state.iStart ();
            else
            {
            SelfFree ();
            throw new InvalidOperationException("stateplayer cannot start a live node");
            }
        }

        protected sealed override void OnFree()
        {
            if (state.on)
            state.iAbort ();
        }

        public void Main ()
        {
            if (state.on)
            state.iExecute ();
            else
            SelfFree ();
        }

        public void SetState (action state)
        {
            if (this.state != null && this.state.on)
            {
            this.state.iAbort ();
            state.iStart ();
            }
            this.state = state;
        }
    }

    [RegisterAsBase]
    public class m_arm_state : m_state_player
    {}

    public class s_state_player : CoreSystem <m_state_player>
    {
        protected override void Main(m_state_player o)
        {
            o.Main ();
        }
    }

    public class state_switcher : neuro
    {

        action root;
        public action nextState;
        public state_switcher ( params action[] o )
        {
            this.o = o;
            root = o [0];
            nextState = root;
        }

        protected sealed override void BeginStep()
        {
            root.iStart ();
        }

        protected sealed override bool Step()
        {
            if (root.on)
            root.iExecute ();

            if (!root.on)
            {
                if ( root != nextState)
                {
                    root = nextState;
                    root.iStart ();
                }
                else
                root.iStart ();
            }

            return false;
        }

        protected sealed override void Abort()
        {
            if (root.on)
            root.iAbort ();
        }
    }

}