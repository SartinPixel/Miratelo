using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pixify
{
    public abstract class condition : node
    {
        public bool inverse;

        public bool Check ()
        {
            return inverse? !OnCheck() : OnCheck();
        }
        protected abstract bool OnCheck ();
    }

    public abstract class condition_neuro : neuro
    {
        [Depend]
        m_character_controller mcc;
        condition[] Condition;

        public enum ConditionCheckGateType { and, or }
        ConditionCheckGateType ConditionCheckGate;

        protected bool ConditionGate()
        {
            bool MeetCondition = true;
            if (ConditionCheckGate == ConditionCheckGateType.and)
                foreach (condition x in Condition)
                {
                    if (!x.Check())
                        MeetCondition = false;
                }
            else
            {
                MeetCondition = false;
                foreach (condition x in Condition)
                {
                    if (x.Check())
                    {
                        MeetCondition = true;
                        break;
                    }
                }
            }
            return MeetCondition;
        }

        public void Set ( ConditionCheckGateType gate )
        {
            ConditionCheckGate = gate;
        }

        public condition_neuro ( condition[] c, action[] o )
        {
            Condition = c;
            this.o = o;
        }

        public sealed override void Create()
        {
            foreach (condition x in Condition)
            {
                mcc.character.ConnectNode (x);
            }
        }
    }

    
    public class guard : condition_neuro
    {
        bool HasStarted;

        public guard ( condition[] c, action[] o ) : base ( c,o ) {}

        protected override bool Step()
        {
            bool MeetCondition = ConditionGate();

            if (HasStarted)
                foreach (var n in o)
                {
                    if (n.on)
                    n.iExecute();
                    else
                    n.iStart();
                }

            if (!HasStarted && MeetCondition)
            HasStarted = true;

            if (MeetCondition == false && HasStarted)
            {
                HasStarted = false;
                foreach (var n in o)
                {
                    if (n.on)
                        n.iAbort();
                }
            }
            return false;
        }

        protected override void Stop()
        {
            foreach (var n in o)
                if (n.on)
                    n.iAbort();
        }
    }
}
