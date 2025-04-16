using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class ac_fall : action
    {
        [Depend]
        m_ground_data mgd;
        [Depend]
        m_capsule_character_controller mccc;
        [Depend]
        m_skin ms;
        [Depend]
        m_standard_character_controller_host mscch;

        public SuperKey AnimationFallEnd = AnimationKey.fall_end;

        public void MoveAir( Vector3 DirPerS)
        {
            mccc.dir += DirPerS * Time.deltaTime/*a*/ * mscch.agm.walkFactor;
        }
        
        protected override void BeginStep()
        {
            ms.PlayState ( 0, AnimationKey.fall, 0.1f );
            mccc.Aquire (this);
        }

        protected override bool Step()
        {
            if (mgd.onGround && mccc.verticalVelocity < 0 && Vector3.Angle (Vector3.up, mgd.groundNormal) <= 45)
            {
                ms.PlayState(ms.knee, AnimationFallEnd, 0.05f);
                mscch.ss.nextState = mscch.agm;
                return true;
            }
            return false;
        }

        protected override void Stop()
        {
            mccc.Free (this);
        }
    }
}