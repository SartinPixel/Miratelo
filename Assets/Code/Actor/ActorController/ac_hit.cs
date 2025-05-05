using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class ac_hit_cgm : action
    {
        [Depend]
        m_capsule_character_controller mccc;
        [Depend]
        m_skin ms;
        [Depend]
        m_hit mh;

        protected override void BeginStep()
        {
            mccc.Aquire (this);
            ms.PlayState (0, AnimationKey.hit, 0.1f, AppendStop);
            mccc.velocityDir += ( ms.Coord.position - mh.LastAttack.impactPoint).normalized * 0.1f;
        }

        protected override void Stop()
        {
            mccc.Free (this);
        }
    }

    public class ac_hit : action
    {
        [Depend]
        m_skin ms;

        protected override void BeginStep()
        {
            ms.PlayState (ms.upper, AnimationKey.hitu, 0.1f, AppendStop);
        }
    }

    public class ac_hit_knocked_out_cgm : action
    {
        [Depend]
        m_capsule_character_controller mccc;
        [Depend]
        m_skin ms;
        [Depend]
        m_hit mh;

        bool AnimationDone;
        bool PartADone;

        protected override void BeginStep()
        {
            mccc.Aquire (this);

            AnimationDone = false;
            PartADone = false;
            ms.PlayState (0, AnimationKey.hit_knocked_a, 0.1f);

            mccc.velocityDir += ( ms.Coord.position - mh.LastAttack.impactPoint).normalized * mh.LastAttack.raw;
            mccc.velocityDir += mh.LastAttack.raw * Vector3.up;

            mccc.Coord.transform.position += Vector3.up * 0.2f;
        }

        void EvAnimationDone ()
        {
            AnimationDone = true;
        }

        protected override bool Step()
        {
            if ( !PartADone && mccc.mgd.onGroundAbs )
            {
            PartADone = true;
            ms.PlayState (0, AnimationKey.hit_knocked_b, 0.1f,EvAnimationDone);
            }

            if (AnimationDone && mccc.mgd.onGroundAbs && mccc.velocityDir.sqrMagnitude == 0)
            return true;
            return false;
        }

        protected override void Stop()
        {
            mccc.Free (this);
        }
    }
}