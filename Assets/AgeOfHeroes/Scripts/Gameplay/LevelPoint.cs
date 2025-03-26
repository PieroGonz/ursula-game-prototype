using AgeOfHeroes.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes
{
    public class LevelPoint : MonoBehaviour
    {
        public Level m_Level;
        public Sprite[] m_Sprites;
        public TMPro.TextMeshPro m_TextMeshPro;

        public GameObject m_CloseMark;
        public GameObject m_PassFlag;

        public SpriteRenderer m_SpriteR;

        [SerializeField]
        private DataStorage m_DataStorage;
        // Start is called before the first frame update
        void Start()
        {
            bool levelPassed = m_DataStorage.m_LastLevelNum > m_Level.m_LevelNum;
            bool levelUnlocked = (m_DataStorage.m_LastLevelNum >= m_Level.m_LevelNum);
            if (levelUnlocked)
            {
                if (levelPassed)
                {
                    m_SpriteR.sprite = m_Sprites[1];
                    m_PassFlag.SetActive(true);
                }
                else
                {
                    m_SpriteR.sprite = m_Sprites[0];
                    m_PassFlag.SetActive(false);
                }
            }
            else
            {
                m_SpriteR.sprite = m_Sprites[2];
                m_PassFlag.SetActive(false);
            }

            m_CloseMark.gameObject.SetActive(false);
            m_TextMeshPro.text = (m_Level.m_LevelNum + 1).ToString();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}