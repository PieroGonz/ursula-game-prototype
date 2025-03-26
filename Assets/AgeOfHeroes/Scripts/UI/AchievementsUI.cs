using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgeOfHeroes.ScriptableObjects;
namespace AgeOfHeroes
{
    public class AchievementsUI : MonoBehaviour
    {
        public GameObject m_AchBtnPrefab;
        public Transform m_ContentPart;
        public AchievementBtn[] m_Btns;

        [SerializeField, Space]
        private Contents m_Contents;
        // Start is called before the first frame update
        void Start()
        {
            m_Btns = new AchievementBtn[m_Contents.m_Achievements.Length];
            for (int i = 0; i < m_Contents.m_Achievements.Length; i++)
            {
                GameObject obj = Instantiate(m_AchBtnPrefab);
                obj.transform.SetParent(m_ContentPart);
                m_Btns[i] = obj.GetComponent<AchievementBtn>();
                m_Btns[i].m_AchData = m_Contents.m_Achievements[i];
                m_Btns[i].m_RewardBtnCounter = i;


            }
        }
        // Update is called once per frame
        void Update()
        {

        }

        public void BtnClose()
        {
            UISystem.RemoveUI(gameObject);
        }
    }
}