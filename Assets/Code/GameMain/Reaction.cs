using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public static class Reaction
    {

        public static Spectre sp_blow;
        public static Spectre sp_impact;

        public static void LoadSpectres ()
        {
            sp_blow = Loader.LoadIntoScene <Spectre> ("sp/sp_blow");
            sp_impact = Loader.LoadIntoScene <Spectre> ("sp/sp_impact");
        }

        public static void Clash (m_reaction_receiver from, m_reaction_receiver to, Force force)
        {
            to.Clash?.Invoke ( force );
            to.reactable.Clash ( force, from.reactable );
        }
    }

    public struct Force
    {
        public ForceType type;
        public float raw;
        public Vector3 impactPoint;
        public Vector3 Normal;

        public Force(ForceType type, float raw, Vector3 impactPoint, Vector3 Normal)
        {
            this.type = type;
            this.raw = raw;
            this.impactPoint = impactPoint;
            this.Normal = Normal;
        }

        public Force(ForceType type, float raw, Vector3 impactPoint)
        {
            this.type = type;
            this.raw = raw;
            this.impactPoint = impactPoint;
            this.Normal = Vector3.zero;
        }
    }

    public enum ForceType { diffuse, perce, slash, perce_parry, hard }
}
