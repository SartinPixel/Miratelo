using System.Collections;
using System.Collections.Generic;
using Pixify;
using UnityEngine;

namespace Triheroes.Code
{
    public class m_actor : module
    {
        [Depend]
        public m_skin ms;
        [Depend]
        public m_dimension md;
        // TO DO remove this, some actor does not need to be attackable, this is just for test
        [Depend]
        public m_attack_receiver mar;

        public m_actor target {get; private set;}
        public Role Role;
        
        // Character Locking this Character
        public List <m_actor> lockers = new List<m_actor>();
        public m_actor primaryLocker => (lockers.Count>0)?lockers[0]:null;

        public override void Create()
        {
            Role = character.GetComponent <ABase> ().Role;
            StartActor ();
        }

        public void LockATarget ( m_actor actor )
        {
            if (actor == null)
            return;
            
            UnlockTarget ();
            target = actor;
            actor.lockers.Add ( this );
        }

        public void UnlockTarget()
        {
            if (target != null && target.lockers.Contains (this))
            target.lockers.Remove (this);
            target = null;
        }

        public m_actor GetNearestFacedFoe ( float distance )
        {
            List<m_actor> foe = GlobalAI.o.GetFoes(Role);
            foe.Sort( new SortDistanceA<m_actor>(ms.rotY.y, ms.Coord.position, distance) );

            if (foe.Count > 0 && Vector3.Distance(ms.Coord.position, foe[0].ms.Coord.position) < distance)
                return foe[0];

            return null;
        }

        void StartActor()
        {
            if (Role.Side == Role._side.ally)
            GlobalAI.o.Allies.Add (this);
            else
            GlobalAI.o.Enemies.Add (this);
        }
    }
}
