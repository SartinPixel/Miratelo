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
        /*
        var r1 = new sequence(
        new ac_goto_target_agm(5, WalkFactor.sprint, 4, true),
        new parallel(
        new ac_equip_weapon(1), new ac_idle()).Set(true),
        new parallel(
            new spawner(new ac_bow_shoot()).Set(true, 3, 2),
            new ac_idle(),
            new ac_aim_target()
            ).Set (true),
        new ac_return_weapon (),
        new ac_goto_target_agm(5, WalkFactor.sprint, 4, true),
        new ac_move_arround_target(5, WalkFactor.sprint, -90),
        new ac_look_at_target (1024),
        
        new parallel ( new ac_equip_weapon (0),
        new xcondition ( IF ( new ac_target_distance (1.5f, Comparator.more ) ), new ac_dash (direction.forward) ).Set ( true, xcondition.mode.CheckAlways ) ),

        new ac_slash ( 2 ),
        new ac_slash ( 1 ),
        new ac_slash ( 0 ),
        new ac_look_at_target (1024),
        new ac_dash (direction.back),
        new ac_return_weapon()
        ).set ( true );

        var r = new sequence(
            new ac_get_a_target(30),
            new change_root(r1)
            );

        scriptHolder.Add(new SuperKey("test"), NewScriptFromRoots(r, r1));
        */

        scriptHolder.Add(ControllerKey.hit_normal, HitLibrary.hit_normal());
        scriptHolder.Add(ControllerKey.hit_knocked_out, HitLibrary.hit_knocked_out());
    }
}