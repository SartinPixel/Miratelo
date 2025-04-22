using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pixify
{
    public class m_character_controller : core
    {
        public Dictionary <SuperKey, script> Scripts;
        script focus;
        script overrideFocus;

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
            if (overrideFocus == null)
            focus.Stop();

            if (!Scripts.TryGetValue(Key, out focus))
                Debug.LogError("Error, no script of key :" + Key.ToString());

            if (overrideFocus == null)
            focus.Start();
        }

        public void Main ()
        {
            if (overrideFocus != null)
                {
                    overrideFocus.Execute();

                    if (!overrideFocus.on)
                    {
                        overrideFocus = null;
                        focus.Start();
                        return;
                    }
                }
            else
            focus.Execute ();
        }

        protected override void OnFree()
        {
            if (overrideFocus == null)
            focus.Stop ();
            else
            overrideFocus.Stop ();
        }

        public void OverrideFocus ( SuperKey key )
        {
            if ( overrideFocus == null )
            focus.Stop ();
            else
            overrideFocus.Stop ();

            if (!Scripts.TryGetValue(key, out script temp))
            throw new InvalidOperationException("FATAL ERROR: no controller with matching key in dictionary" + key.ToString());
            else
            {
                overrideFocus = temp;
                overrideFocus.Start();
            }
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