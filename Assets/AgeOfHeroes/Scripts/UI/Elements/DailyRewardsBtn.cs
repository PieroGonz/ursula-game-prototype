using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AgeOfHeroes.ScriptableObjects;
namespace AgeOfHeroes
{
    public class DailyRewardsBtn : MonoBehaviour
    {
        [HideInInspector]
        public DailyRewardsData m_DRData;
        [HideInInspector]
        public int m_RewardBtnCounter;
        [SerializeField]
        private DataStorage m_DataStorage;

        [SerializeField, Space]
        private UIGraphicContents m_UIGraphicContents;

        [SerializeField, Space]
        private UITextContents m_UITextContentsContents;
        // Start is called before the first frame update
        void Start()
        {

            UISystem.FindText(gameObject, "text-title").text = m_DRData.m_TitleEnglish;


            //----------reward data

            UISystem.FindImage(gameObject, "img-rewardicon").sprite = m_DRData.m_Reward.m_Icon;
            switch (m_DRData.m_Reward.m_Type)
            {
                case "coin":
                    UISystem.FindText(gameObject, "text-rewardcount").text = "+" + m_DRData.m_Reward.m_Count.ToString();
                    break;
                case "gem":
                    UISystem.FindText(gameObject, "text-rewardcount").text = "+" + m_DRData.m_Reward.m_Count.ToString();
                    break;
            }
            //------------
            UpdateInfo();
        }
        void OnDestroy()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void UpdateInfo()
        {
            Image cover = UISystem.FindImage(gameObject, "img-icon-deactivate");
            Image btn = UISystem.FindImage(gameObject, "BtnGetReward");
            Image star = UISystem.FindImage(gameObject, "img-star");

            if (m_DRData.m_ReachedDay)
            {
                star.gameObject.SetActive(true);
                btn.gameObject.SetActive(true);
                cover.gameObject.SetActive(false);

                if (m_DRData.m_GotReward)
                {
                    btn.gameObject.SetActive(false);
                    cover.gameObject.SetActive(true);
                }
                else
                {
                    btn.gameObject.SetActive(true);
                }
            }
            else
            {
                star.gameObject.SetActive(false);
                btn.gameObject.SetActive(false);
                cover.gameObject.SetActive(true);
            }
        }
        public void BtnClicked()
        {
            if (Application.platform != RuntimePlatform.Android)
            {
                m_DataStorage.m_CurrentRewardVideoRequest = m_RewardBtnCounter;
                HandleGetDailyReward();
            }
            else
            {
                if (!m_DataStorage.CheckInternet())
                {
                    UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 1, m_UITextContentsContents.m_Messages[0], m_UIGraphicContents.m_Graphics[4]);
                    msg.AutoCloseMessage(1);
                }
                else
                {
                    m_DataStorage.m_CurrentRewardVideoRequest = m_RewardBtnCounter;
                }
            }


            SoundGallery.PlaySound("pop1");
        }
        private void HandleGetDailyReward()
        {
            if (m_DataStorage.m_CurrentRewardVideoRequest == m_RewardBtnCounter)
            {
                if (m_DRData.m_Reward.m_Type == "coin")
                {
                    m_DataStorage.Coin += m_DRData.m_Reward.m_Count;
                    m_DRData.m_Reward.m_Aquired = true;
                    m_DRData.m_GotReward = true;
                    UISystem.ShowCoinReward(m_DRData.m_Reward.m_Count);
                }
                else
                {
                    m_DataStorage.Gem += m_DRData.m_Reward.m_Count;
                    m_DRData.m_Reward.m_Aquired = true;
                    m_DRData.m_GotReward = true;
                    UISystem.ShowGemReward(m_DRData.m_Reward.m_Count);
                }
                TimeControl.m_Main.m_TimeInstances[6].Reset();
                m_DataStorage.m_GotDailyReward = true;
                m_DataStorage.SaveData();
                UpdateInfo();
                UISystem.RemoveUI("DailyRewardUI");
            }
        }
    }
}