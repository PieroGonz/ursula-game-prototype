using AgeOfHeroes.ScriptableObjects;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
namespace AgeOfHeroes.UI
{
    public class ProgressionRewardBtn : MonoBehaviour
    {
        [SerializeField, Space]
        private Contents m_Contents;
        [SerializeField, Space]
        private DataStorage m_DataStorage;
        //[SerializeField, Space]
        // private GameplayData m_GameplayData;
        [SerializeField, Space]
        private PlayerData m_PlayerData;

        [HideInInspector]
        public int m_ID;


        public Button m_GetReward;
        public Text m_Level;
        public Image m_Check;
        public Image m_Frame;
        public Image m_Pointer;
        public Image m_Reward;

        // Start is called before the first frame update
        void Start()
        {
            m_Reward.sprite = m_Contents.m_ProgressionRewards[m_ID].m_Icon;
        }

        // Update is called once per frame
        void Update()
        {
            if (m_Contents.m_ProgressionRewards[m_ID].m_Type == "stage")
            {
                m_Frame.gameObject.SetActive(true);
            }
            else
            {
                m_Frame.gameObject.SetActive(false);
            }

            if (m_PlayerData.m_PlayerLevel >= ((m_ID + 1) * 5))
            {
                if (m_Contents.m_ProgressionRewards[m_ID].m_Aquired)
                {
                    m_Check.gameObject.SetActive(true);
                    m_Pointer.gameObject.SetActive(false);
                    m_GetReward.enabled = false;
                }
                else
                {
                    m_Check.gameObject.SetActive(false);
                    m_Pointer.gameObject.SetActive(true);
                    m_GetReward.enabled = true;
                }
            }
            else
            {
                m_Check.gameObject.SetActive(false);
                m_GetReward.enabled = false;
            }

        }

        public void BtnClicked()
        {
            switch (m_Contents.m_ProgressionRewards[m_ID].m_Type)
            {
                case "coin":
                    m_DataStorage.Coin += m_Contents.m_ProgressionRewards[m_ID].m_Count;
                    UISystem.ShowCoinReward(m_Contents.m_ProgressionRewards[m_ID].m_Count);
                    break;
                case "gem":
                    m_DataStorage.Gem += m_Contents.m_ProgressionRewards[m_ID].m_Count;
                    UISystem.ShowGemReward(m_Contents.m_ProgressionRewards[m_ID].m_Count);
                    break;
                case "stage":
                    if (m_Contents.m_ProgressionRewards[m_ID].m_TitleEnglish == "Desert")
                    {
                        m_Contents.m_Stages[1].m_Unlocked = true;
                    }
                    else if (m_Contents.m_ProgressionRewards[m_ID].m_TitleEnglish == "Castle")
                    {
                        m_Contents.m_Stages[2].m_Unlocked = true;
                    }
                    UISystem.ShowStageReward(m_Contents.m_ProgressionRewards[m_ID].m_Icon, m_Contents.m_ProgressionRewards[m_ID].m_TitleFarsi);
                    break;
            }
            m_Contents.m_ProgressionRewards[m_ID].m_Aquired = true;
            //Debug.Log("Got it!");
            m_DataStorage.SaveData();
        }
    }
}
