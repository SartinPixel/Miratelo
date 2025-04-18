using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixify;
using Triheroes.Code;
using static Pixify.ScriptWriter;

public class PlayerController : ScriptInit
{
    public override void OnAddScript(Dictionary<SuperKey, script> scriptHolder)
    {
        var pc_dash = new pc_dash ();
        var pc_draw = new pc_draw ();
        var pc_jump = new pc_jump ();
        var pc_normal_move = new pc_normal_move ();


        var r = new parallel(
                    new pc_active_master_controller(),
                    pc_normal_move,
                    pc_jump,
                    pc_draw,
                    pc_dash,
                    new guard(  IF (new ac_has_active_weapon()),
                                DO (new change_focus(new SuperKey("equip_selector"))) )
        );
        scriptHolder.Add(new SuperKey("idle"), NewScriptFromRoot(r));

        r = new parallel(
                new guard(  IF (NOT( new ac_has_active_weapon())),
                            DO (new change_focus ( new SuperKey("idle"))) ),
                new guard(  IF (new ac_check_active_weapon_type(WeaponType.Sword)),
                            DO (new change_focus( new SuperKey("sword"))) ),
                new guard(  IF (new ac_check_active_weapon_type(WeaponType.Bow)),
                            DO (new change_focus(new SuperKey("bow")) )
                    )
        );
        scriptHolder.Add(new SuperKey("equip_selector"), NewScriptFromRoot(r));

        r = new parallel(
                new pc_active_master_controller(),
                pc_normal_move,
                pc_jump,
                pc_dash,
                pc_draw,
                new pc_sword(),
                new guard ( IF (NOT(new ac_check_active_weapon_type (WeaponType.Sword))),
                            DO (new change_focus ( new SuperKey ( "equip_selector"))) )
        );
        scriptHolder.Add(new SuperKey("sword"), NewScriptFromRoot(r));

        r = new parallel(
                new pc_active_master_controller(),
                pc_normal_move,
                pc_jump,
                pc_draw,
                new guard ( IF (NOT(new ac_check_active_weapon_type (WeaponType.Bow))),
                            DO (new change_focus ( new SuperKey ( "equip_selector"))) )
        );
        scriptHolder.Add(new SuperKey("bow"), NewScriptFromRoot(r));
    }
}