using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgeOfHeroes.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CharacterSkins", menuName = "Scriptable/CharacterSkins", order = 1)]

    public class CharacterSkins : ScriptableObjBase
    {
        public GameObject m_Prefab;
        public GameObject m_BaseBodyPrefab;
        public float m_AddedHealth;
    }
}

