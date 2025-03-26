using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgeOfHeroes.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Player", menuName = "Soccer2DCustomObjects/Player", order = 1)]
    public class Player : ScriptableObjBase
    {
        public Color m_SkinColor = Color.white;
        public Sprite m_HeadSprite;

        public float[] m_PowerBase;
        public int m_TechniqueNum = 0;
        public string m_TechniqueTitle = "";
        public string m_TeamName = "";
    }
}