using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Triheroes.Code
{
    public class GlobalAI : MonoBehaviour
    {
        public static GlobalAI o;
        // all active ally
        public List<m_actor> Allies;
        // all active enemy
        public List<m_actor> Enemies;

        void Awake()
        {
            o = this;
            Allies = new List<m_actor>();
            Enemies = new List<m_actor>();
        }

        public  List<m_actor> GetFoes (Role role)
        {
            if (role.Side == Role._side.enemy)
            return Allies;
            else
            return Enemies;
        }
    }
    
    // role assignement to a character
    [System.Serializable]
    public struct Role
    {
        public enum _side { ally, enemy }
        public _side Side;

        public string GetTag()
        {
            if (Side == _side.ally)
                return "ally";
            else
                return "enemy";
        }
        public string GetAgainstTag()
        {
            if (Side == _side.ally)
                return "enemy";
            else
                return "ally";
        }
        public void Reverse()
        {
            if (Side == _side.ally)
            Side = _side.enemy; else Side = _side.ally;
        }
    }
}
