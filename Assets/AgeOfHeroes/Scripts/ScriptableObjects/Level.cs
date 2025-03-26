using AgeOfHeroes.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Level", menuName = "Scriptable/Level", order = 1)]
    public class Level : ScriptableObjBase
    {
        public int m_LevelNum = 0;
        public int m_WorldNum = 0;
        public int m_ReqiredLevel;
        public bool m_Passed = false;
        public Character[] m_EnemyCharacters;

        public int m_LevelTheme = 0;
    }
}
