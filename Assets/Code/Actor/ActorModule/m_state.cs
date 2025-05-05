using System;
using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    /// <summary>
    /// stack state
    /// main state have priority, cannot be aborted by substate
    /// self aquired, no need to aquire, the aquire method is overriden to aquire the state players instead
    /// aquire the state players instead
    /// </summary>
    public sealed class m_state_stack : core
    {
        state_player [] StateStack;

        public override void Create()
        {
            StateStack = new state_player [2];

            for (int i = 0; i < StateStack.Length; i++)
                StateStack [i] = character.ConnectNode ( new state_player() );

            // self aquire
            Aquire ( new Null () );
        }

        public bool stateIsOn (int layer) => StateStack [layer].on;
        /// <summary>
        /// Aquire the first state player
        /// </summary>
        /// <param name="host"></param>
        public void AquireMainStatePlayer ( node host ) => AquireStatePlayer ( 0, host );

        public void AquireStatePlayer ( int layer, node host ) => StateStack [layer].Aquire (host);
        /// <summary>
        /// free the first state player
        /// </summary>
        /// <param name="host"></param>
        public void FreeMainStatePlayer (node host) => FreeStatePlayer ( 0, host );
        public void FreeStatePlayer ( int layer, node host ) => StateStack [layer].Free (host);
        public core GetStatePlayer ( int layer ) => StateStack [layer];

        public void Main ()
        {
            for (int i = 0; i < StateStack.Length; i++)
            {
                if (StateStack [i].on)
                StateStack[i].Update ();
            }
        }

        public action GetState (int layer) => StateStack [layer].state;

        public void SetMainState ( action state) => SetState ( 0, state );

        public void SetState (int layer, action state) => StateStack [layer].SetState ( state );

        private class state_player : core
        {
            public action state;
            protected sealed override void OnAquire()
            {
                if (!state.on)
                    state.iStart();
                else
                {
                    SelfFree();
                    throw new InvalidOperationException("FATAL ERROR: stateplayer cannot start a live node");
                }
            }

            protected sealed override void OnFree()
            {
                if (state.on)
                {
                state.iAbort();
                state = null;
                }
            }

            public void Update()
            {
                if (state.on)
                    state.iExecute();
                else
                    SelfFree();
            }

            public void SetState ( action state )
            {
                if (on)
                SelfAbort ();
                this.state = state;
            }
        }
    }

    public class s_state_stack : CoreSystem<m_state_stack>
    {
        protected override void Main(m_state_stack o)
        {
            o.Main ();
        }
    }

    public class state_switcher : neuro
    {

        action root;
        public action nextState;
        public state_switcher(params action[] o)
        {
            this.o = o;
            root = o[0];
            nextState = root;
        }

        protected sealed override void BeginStep()
        {
            root.iStart();
        }

        protected sealed override bool Step()
        {
            if (root.on)
                root.iExecute();

            if (!root.on)
            {
                if (root != nextState)
                {
                    root = nextState;
                    root.iStart();
                }
                else
                    root.iStart();
            }

            return false;
        }

        public void SwitchTo ( action nextState )
        {
            if (root.on)
            root.iAbort ();
            this.nextState = nextState;
        }

        protected sealed override void Abort()
        {
            if (root.on)
                root.iAbort();
        }
    }

}