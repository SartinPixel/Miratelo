using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixify;
using Triheroes.Code;

public class TestScript : ScriptInit
{
    public override void OnAddScript( Dictionary <SuperKey, script> scriptHolder )
    {
        var r = new parallel (
            new pc_active_master_controller (),
            new pc_normal_move (),
            new pc_jump (),
            new pc_draw (),
            new pc_sword (),
            new pc_dash ()
        );
        
        scriptHolder.Add (new SuperKey ("test"), ScriptWriter.NewScriptFromRoot (r));
    }
}