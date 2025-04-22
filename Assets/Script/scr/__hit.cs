using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixify;
using Triheroes.Code;
using static Pixify.ScriptWriter;

public static partial class HitLibrary
{
    public static script hit_normal ()
    {
        var r = new sequence ( new ac_hit_cgm () ).set ( false );
        return NewScriptFromRoot(r);
    }
}
