using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgeOfHeroes.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Kit", menuName = "Soccer2DCustomObjects/Kit", order = 1)]
    public class Kit : ScriptableObjBase
    {
        public GameObject m_BodyBasePrafab;
        public string m_TeamName = "";
        public Player[] m_DefaultPlayers;
        public Sprite[] m_KitPartSprites;
    }
}