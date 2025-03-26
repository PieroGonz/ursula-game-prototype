using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgeOfHeroes.ScriptableObjects;
using AgeOfHeroes.UI;
namespace AgeOfHeroes
{
    public class XPBarAdd : MonoBehaviour
    {
        public static XPBarAdd Current;
        [SerializeField, Space]
        private DataStorage m_DataStorage;
        public int m_MaxXP;
        [SerializeField, Space]
        private PlayerData m_PlayerData;


        public int m_LastXP;
        public int m_LastXPAdd;
        void Awake()
        {
            Current = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            m_MaxXP = 100 + m_PlayerData.m_PlayerLevel * 20;

            if (m_DataStorage.m_PlayerXP >= m_MaxXP)
            {
                m_PlayerData.m_PlayerLevel++;
                m_DataStorage.m_PlayerXP = m_DataStorage.m_PlayerXP - m_MaxXP;
                //m_DataStorage.UploadDataonDatabase("PlayerLevel");

            }
            m_PlayerData.SaveData();
            m_DataStorage.SaveData();

        }

        // Update is called once per frame
        void Update()
        {
            m_MaxXP = 100 + m_PlayerData.m_PlayerLevel * 50;
        }

        public void EarnXP(int xpAmount)
        {

            m_LastXP = m_DataStorage.m_PlayerXP;
            m_LastXPAdd = xpAmount;

            m_DataStorage.m_PlayerXP += xpAmount;
            if (m_DataStorage.m_PlayerXP >= m_MaxXP)
            {
                m_PlayerData.m_PlayerLevel++;
                m_DataStorage.m_PlayerXP = m_DataStorage.m_PlayerXP - m_MaxXP;
                HandleLevelUpReward();
                UISystem.ShowUI("LevelUpUI");
            }
            else
            {
                UISystem.ShowUI("XPUpUi");
            }

            m_PlayerData.SaveData();
            m_DataStorage.SaveData();
            // m_DataStorage.UploadDataonDatabase("PlayerXP");

        }

        public void HandleLevelUpReward()
        {
            int coin = 10;
            coin += Mathf.FloorToInt((float)m_PlayerData.m_PlayerLevel / 20f) * 10;
            int gem = 1;
            gem += Mathf.FloorToInt((float)m_PlayerData.m_PlayerLevel / 20f);

            int rewardType = 0;
            if (m_PlayerData.m_PlayerLevel >= 3 && m_PlayerData.m_PlayerLevel % 3 == 0)
            {
                rewardType = 1;
            }

            switch (rewardType)
            {
                case 0:
                    m_DataStorage.Coin += coin;
                    //m_DataStorage.UploadDataonDatabase("Coin");
                    break;
                case 1:
                    m_DataStorage.Gem += gem;
                    //m_DataStorage.UploadDataonDatabase("Gem");
                    break;
            }

            //if (MainMenuTabsUI.m_Main != null)
            //MainMenuTabsUI.m_Main.UpdateTabAlerts();
        }
    }
}