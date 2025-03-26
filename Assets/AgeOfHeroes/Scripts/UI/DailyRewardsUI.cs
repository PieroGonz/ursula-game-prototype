using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgeOfHeroes.ScriptableObjects;
namespace AgeOfHeroes
{
    public class DailyRewardsUI : MonoBehaviour
    {
        public GameObject m_DRBtnPrefab;
        public Transform m_ContentPart;
        public DailyRewardsBtn[] m_Btns;

        [SerializeField, Space]
        private Contents m_Contents;
        [SerializeField, Space]
        private DataStorage m_DataStorage;
        // Start is called before the first frame update
        void Start()
        {
            m_Btns = new DailyRewardsBtn[m_Contents.m_DailyRewards.Length];
            for (int i = 0; i < m_Contents.m_DailyRewards.Length; i++)
            {
                GameObject obj = Instantiate(m_DRBtnPrefab);
                obj.transform.SetParent(m_ContentPart);
                m_Btns[i] = obj.GetComponent<DailyRewardsBtn>();
                m_Btns[i].m_DRData = m_Contents.m_DailyRewards[i];
                m_Btns[i].m_RewardBtnCounter = i;
                if (m_Btns[i].m_DRData.m_DayNum == m_DataStorage.m_DayNumInGame)
                {
                    m_Btns[i].m_DRData.m_ReachedDay = true;
                }
                else
                {
                    m_Btns[i].m_DRData.m_ReachedDay = false;
                }
            }

        }
        // Update is called once per frame
        void Update()
        {

        }
        public void CalculateDay(int daynum)
        {
            for (int i = 0; i < m_Contents.m_DailyRewards.Length; i++)
            {
                if (m_Btns[daynum].m_DRData.m_DayNum == 1)
                {

                }

            }

            m_Btns[daynum].m_DRData.m_ReachedDay = true;
        }
        public void BtnClose()
        {
            UISystem.RemoveUI(gameObject);
        }
    }
}