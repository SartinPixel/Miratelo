using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public static class Reaction
    {

        public static Spectre sp_blow;

        public static void LoadSpectres ()
        {
            sp_blow = Loader.LoadIntoScene <Spectre> ("sp/sp_blow");
        }

        public static void Clash (m_reactable to, Force force)
        {
            if (to.type == ObjType.skin)
            {
                sp_blow.Emit ( force.impactPoint );
                to.Clash?.Invoke (force);
            }
        }
    }

    public struct Force
    {
        public ForceType type;
        public float raw;
        public Vector3 impactPoint;

        public Force(ForceType type, float raw, Vector3 impactPoint)
        {
            this.type = type;
            this.raw = raw;
            this.impactPoint = impactPoint;
        }
    }

    public enum ForceType { perce, slash, hard, diffuse }
}
