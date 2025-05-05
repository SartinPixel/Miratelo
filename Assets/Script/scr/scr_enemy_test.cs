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
            new ac_equip_weapon ( 0 ),
            new ac_goto_target_agm ( 10, 0.23f, true ),
            new ac_slash (0),
            new ac_slash (1),
            new ac_slash (2)
        );

        var r = new sequence (
            new ac_get_a_target(30),
            new change_root ( r1 )
            );

        scriptHolder.Add ( new SuperKey("test"), NewScriptFromRoots (r, r1) );
        
        scriptHolder.Add(ControllerKey.hit_normal,HitLibrary.hit_normal ());
    }
}