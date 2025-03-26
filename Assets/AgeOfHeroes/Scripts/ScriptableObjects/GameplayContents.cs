using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgeOfHeroes.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameplayContents", menuName = "Scriptable/GameplayContents", order = 1)]
    public class GameplayContents : ScriptableObject
    {
        public GameObject[] m_StatParticles;
        public GameObject[] m_HitParticles;
        public GameObject m_SelectPawnParticle;
        public GameObject m_HealParticle;
    }
}