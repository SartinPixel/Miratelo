using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pixify
{
    public class m_character_controller : core
    {
        public Dictionary <SuperKey, script> Scripts;
        script focus;

        public m_character_controller ( Dictionary <SuperKey, script> Scripts )
        {
            this.Scripts = Scripts;
        }

        public override void Create()
        {
            focus = Scripts.First().Value;
            foreach (var s in Scripts.Values)
            s.RegisterScript (this);
        }

        protected override void OnAquire()
        {
            if ( focus != null )
            focus = Scripts.First().Value;
            else if ( Scripts.Count > 0 )
                focus = Scripts.First().Value;
            focus.Start ();
        }

        public void ChangeFocus(SuperKey Key)
        {
            focus.Stop();
            if (!Scripts.TryGetValue(Key, out focus))
                Debug.LogError("Error, no script of key :" + Key.ToString());
            focus.Start();
        }

        public void Main ()
        {
            focus.Execute ();
        }

        protected override void OnFree()
        {
            focus.Stop ();
        }
    }

    public class s_character_controller : CoreSystem<m_character_controller>
    {
        protected override void Main(m_character_controller o)
        {
            o.Main ();
        }
    }
}