using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixify;

namespace Triheroes.Code
{
    public class d_sfx : vDot<d_sfx>
    {
        AudioSource Au;

        public override void Create()
        {
            Au = new GameObject("sfx tube").AddComponent<AudioSource>();
            Au.spatialBlend = 1;
        }

        public void Set(AudioClip sfx)
        {
            Au.clip = sfx;
        }

        protected sealed override void OnAquire()
        {
            Au.gameObject.SetActive(true);
            Au.Play();
        }

        public void CheckIfAudioDone()
        {
            if (!Au.isPlaying)
                DeFire(this);
        }

        public class s_sfx : CoreSystem<d_sfx>
        {
            protected override void Main(d_sfx o)
            {
                o.CheckIfAudioDone();
            }
        }

        protected sealed override void OnFree()
        {
            Au.gameObject.SetActive(false);
        }
    }
}
