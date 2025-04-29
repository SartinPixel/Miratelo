using System;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class m_skin : core
    {

        public Transform Coord;
        public Animator Ani;
        AniExt AniExt;

        public Vector3 rotY;
        public Vector3 posY;

        // animation player per state
        Player[] Players;
        /// <summary>
        /// layer index
        /// </summary>
        public int sword, bow, knee, r_arm, upper;

        /// <summary>
        /// allow movement from skin animation
        /// </summary>
        public bool SkinMove;
        public Vector3 SkinDir;
        public float GetSpdCurves() => Ani.GetFloat(Hash.spd);

        public sealed override void Create()
        {
            FetchSkinData ();
            CacheCommonLayerIndex();
            CreatePlayerForEachLayer();
            // self aquire
            Aquire (new Null());
        }

        public void Update()
        {
            foreach (var p in Players)
                p.update();
        }

        void FetchSkinData ()
        {
            // fetch character data
            Coord = character.transform;
            ABase A = character.GetComponent<ABase>();
            Ani = A.Skin.GetComponent<Animator>();
            AniExt = A.Skin.AniExt;

            // disable fucking firing event
            Ani.fireEvents = false;

            // create skin additional module on request
            if (A.HumanFootIk)
                character.NeedModule (typeof (m_skin_foot_ik) );
        }

        void CacheCommonLayerIndex()
        {
            bow = Ani.GetLayerIndex("bow");
            sword = Ani.GetLayerIndex("sword");
            r_arm = Ani.GetLayerIndex("r_arm");
            knee = Ani.GetLayerIndex("knee");
            upper = Ani.GetLayerIndex("upper");
        }

        void CreatePlayerForEachLayer()
        {
            List<Player> sPlayersInitializer = new List<Player>();

            for (int i = 0; i < Ani.layerCount; i++)
            {
                if (AniExt.RealLayer[i] == true)
                    sPlayersInitializer.Add(new StatePlayer(i, Ani));
                else
                    sPlayersInitializer.Add(new SyncPlayer(i, Ani));
            }
            Players = sPlayersInitializer.ToArray();
        }

        #region Animation command

        public float[] EventPointsOfState (SuperKey Key)
        {
            if (AniExt.States.TryGetValue(Key, out AniExt.State State))
                return State.EvPoint;
            else
                Debug.LogError("no state corresponding key in Animator controller");
            return null;
        }

        public void PlayState(int layerIndex, SuperKey Key, float FadeDuration = 0.1f, Action EndAction = null, Action AbortAction = null, Action Ev0 = null, Action Ev1 = null, Action Ev2 = null, Action Ev3 = null)
        {
            if (AniExt.States.TryGetValue(Key, out AniExt.State State))
                ((StatePlayer)Players[layerIndex]).play(State.Key, State.Duration, FadeDuration, State.EvPoint, false, EndAction, AbortAction, Ev0, Ev1, Ev2, Ev3);
            else
                Debug.LogError("no state corresponding key in Animator controller");
        }

        public void HoldState(int layerIndex, SuperKey Key, float FadeDuration = 0.1f, Action EndAction = null, Action AbortAction = null, Action Ev0 = null, Action Ev1 = null, Action Ev2 = null, Action Ev3 = null)
        {
            if (AniExt.States.TryGetValue(Key, out AniExt.State State))
                ((StatePlayer)Players[layerIndex]).play(State.Key, State.Duration, FadeDuration, State.EvPoint, true, EndAction, AbortAction, Ev0, Ev1, Ev2, Ev3);
            else
                Debug.LogError("no state corresponding key in Animator controller");
        }

        public void ActivateSyncLayer(int layerIndex)
        {
            ((SyncPlayer)Players[layerIndex]).ActivateSyncLayer();
        }

        public void DisableSyncLayer(int layerIndex)
        {
            ((SyncPlayer)Players[layerIndex]).DisableSyncLayer();
        }

        public void ControlledStop(int layerIndex)
        {
            ((StatePlayer)Players[layerIndex]).stop();
        }

        public bool CurrentStateEqualTo(int layerIndex, SuperKey key)
        {
            if (AniExt.States.TryGetValue(key, out AniExt.State AnimationState))
                return AnimationState.Key == ((StatePlayer)Players[layerIndex]).animationId;
            else
                Debug.LogError("no state corresponding key in Animator controller");
            return false;
        }

        public bool IsTransitioningFrom(int layerIndex, SuperKey key)
        {
            if (AniExt.States.TryGetValue(key, out AniExt.State AnimationState))
                return ((StatePlayer)Players[layerIndex]).IsTransitioningFrom(AnimationState.Key);
            else
                Debug.LogError("no state corresponding key in Animator controller");
            return false;
        }

        #endregion

        #region Animation player
        abstract class Player
        {
            protected Animator Ani;
            protected int LayerIndex;
            public abstract void update();
            protected float playTime;
        }

        sealed class StatePlayer : Player
        {
            public int animationId = -1;
            int previousAnimationId = -1;
            bool playing;
            float duration;
            float[] evPoint;
            int evId = 0;
            float fadeDuration;
            bool hold;
            Action E, A, E0, E1, E2, E3;

            public StatePlayer(int layerIndex, Animator Ani)
            {
                LayerIndex = layerIndex;
                this.Ani = Ani;
            }

            // NOTE: don't ever call play state inside the Abort event, because it will be called infinitely
            public void play(int Animation, float Duration, float TransitionDuration, float[] EventPoint, bool Hold, Action EndAction, Action AbortAction, Action Ev0, Action Ev1, Action Ev2, Action Ev3)
            {
                if (animationId != Animation || playing == false)
                {
                    if (playing == false && LayerIndex != 0)
                        Ani.Play(Animation, LayerIndex, 0);
                    else
                        Ani.CrossFadeInFixedTime(Animation, TransitionDuration, LayerIndex, 0f, 0);

                    if (playing) A?.Invoke();

                    E = null; A = null; E0 = null; E1 = null; E2 = null; E3 = null;

                    previousAnimationId = animationId; animationId = Animation;
                    playing = true; playTime = 0; evId = 0;
                    evPoint = EventPoint;
                    duration = Duration;
                    fadeDuration = TransitionDuration;
                    hold = Hold;

                    E += EndAction; A += AbortAction; E0 += Ev0; E1 += Ev1; E2 += Ev2; E3 += Ev3;
                }
            }

            public void stop()
            {
                if (hold)
                {
                    playTime = duration - 0.1f; hold = false;
                }
                else
                    playing = false;
            }

            public bool IsTransitioningFrom(int Animation)
            {
                return playing && playTime <= fadeDuration && Animation == previousAnimationId;
            }

            public override void update()
            {
                if (playing)
                {
                    playTime += Time.deltaTime;

                    // make sure the correct animation fire the events
                    int currentFiringAnimation = animationId;
                    if (evId < evPoint.Length)
                        while (playTime > evPoint[evId])
                        {
                            // fire in the hole
                            switch (evId)
                            {
                                case 0: E0?.Invoke(); break;
                                case 1: E1?.Invoke(); break;
                                case 2: E2?.Invoke(); break;
                                case 3: E3?.Invoke(); break;
                                case 4: Debug.LogError("For performance, max allowed fire event is 3"); break;
                            }
                            if (currentFiringAnimation != animationId)
                                return;
                            evId++;
                            if (evId >= evPoint.Length)
                                break;
                        }
                    // stop if done
                    if (playTime >= duration)
                    {
                        if (!hold)
                            playing = false;
                        E?.Invoke();
                    }
                }

                if (LayerIndex != 0)
                    LayerFading();
            }

            void LayerFading()
            {
                if (playing && playTime < duration - 0.1f)
                    Ani.SetLayerWeight(LayerIndex, Mathf.MoveTowards(Ani.GetLayerWeight(LayerIndex), 1, Time.deltaTime * 10));
                else if (!playing || (!hold && playTime >= duration - 0.1f))
                    Ani.SetLayerWeight(LayerIndex, Mathf.MoveTowards(Ani.GetLayerWeight(LayerIndex), 0, Time.deltaTime * 10));
            }
        }

        sealed class SyncPlayer : Player
        {

            private enum _state { off, turningON, turningOFF }
            _state State;

            public SyncPlayer(int layerIndex, Animator Ani)
            {
                LayerIndex = layerIndex;
                this.Ani = Ani;
            }

            public void ActivateSyncLayer()
            {
                State = _state.turningON;
                playTime = 0;
            }

            public void DisableSyncLayer()
            {
                State = _state.turningOFF;
                playTime = 0;
            }

            public override void update()
            {
                switch (State)
                {
                    case _state.turningON:
                        if (playTime / 0.1f >= 1)
                        {
                            State = _state.off;
                            Ani.SetLayerWeight(LayerIndex, 1);
                            break;
                        }
                        Ani.SetLayerWeight(LayerIndex, playTime / 0.1f);
                        playTime += Time.deltaTime;
                        return;
                    case _state.turningOFF:
                        if (playTime / 0.1f >= 1)
                        {
                            State = _state.off;
                            Ani.SetLayerWeight(LayerIndex, 0);
                            break;
                        }
                        Ani.SetLayerWeight(LayerIndex, 1 - (playTime / 0.1f));
                        playTime += Time.deltaTime;
                        return;
                }
            }
        }
        #endregion
    }

}