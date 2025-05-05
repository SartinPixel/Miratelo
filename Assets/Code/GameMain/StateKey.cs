using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixify;

namespace Triheroes.Code
{
    public static class StateKey
    {
        public static readonly SuperKey zero = new SuperKey("zero");

        public static readonly SuperKey brake = new SuperKey("brake");
        public static readonly SuperKey brake_rotation = new SuperKey("brake_rotation");
        public static readonly SuperKey idle = new SuperKey("idle");
        public static readonly SuperKey walk = new SuperKey("walk");
        public static readonly SuperKey walk_lateral = new SuperKey("walk_lateral");
        public static readonly SuperKey walk_tired = new SuperKey("walk_tired");
        public static readonly SuperKey run = new SuperKey("run");
        public static readonly SuperKey sprint = new SuperKey("sprint");

        public static readonly SuperKey jump = new SuperKey("jump");

        public static readonly SuperKey slash = new SuperKey("slash");
        public static readonly SuperKey parry_trajectile = new SuperKey("parry_trajectile");
        public static readonly SuperKey parry = new SuperKey("parry");

        public static readonly SuperKey aim = new SuperKey("aim");
    }

    public static class ControllerKey
    {
        public static readonly SuperKey hit_normal = new SuperKey("hit_normal");
    }
}
