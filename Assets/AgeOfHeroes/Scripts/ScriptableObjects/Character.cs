using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgeOfHeroes.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Character", menuName = "Scriptable/Character", order = 1)]
    public class Character : ScriptableObjBase
    {
        public GameObject m_EmptyCharacter;
        public GameObject m_BodyBasePrefab;

        public CharAbility[] m_Abilities;

        public float m_Health;
        public float m_Damage;
        public float m_Defence;


        public CharacterSkins[] m_Skins;
        public CharacterEquipment[] m_Weapons;

        public int m_SkinNum;
        public int m_WeaponNum;

    }
}