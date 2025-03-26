using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CharAbility", menuName = "Scriptable/CharAbility", order = 1)]
    public class CharAbility : ScriptableObjBase
    {
        [Space]
        public bool m_MainWeapon = true;
        public float m_BaseDamageRangeMin = 5;
        public float m_BaseDamageRangeMax = 5;
        public bool m_Heal = false;
        public bool m_Stun = false;
        public bool m_Buff = false;
        public bool m_Bleed = false;
        public bool m_Blade = false;
        public int m_CoolDown;
        public float m_AttackDistance = 50;
    }
}
