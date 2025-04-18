using System.Collections;
using System.Collections.Generic;
using Pixify;
using Triheroes.Code;
using UnityEngine;
using static Pixify.ScriptWriter;

public class scr_enemy_test : ScriptInit
{
    public override void OnAddScript(Dictionary<SuperKey, script> scriptHolder)
    {
        var r1 = new sequence (
            new ac_goto_target_agm ( 10, 4, true ),
            new Null ()
        );

        var r = new sequence (
            new ac_get_a_target(30),
            new change_root ( r1 )
            );

        scriptHolder.Add ( new SuperKey("test"), NewScriptFromRoots (r, r1) );
    }
}