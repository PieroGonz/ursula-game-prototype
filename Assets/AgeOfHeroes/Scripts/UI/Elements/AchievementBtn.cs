using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AgeOfHeroes.ScriptableObjects;
namespace AgeOfHeroes
{
    public class AchievementBtn : MonoBehaviour
    {
        [HideInInspector]
        public AchievementData m_AchData;
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
            UISystem.FindImage(gameObject, "img-icon").sprite = m_AchData.m_Icon;
            UISystem.FindText(gameObject, "text-title").text = m_AchData.m_TitleFarsi;
            UISystem.FindText(gameObject, "text-desc").text = m_AchData.m_Description;

            string count = m_AchData.Counter + " / " + m_AchData.CounterMax;
            UISystem.FindText(gameObject, "text-count").text = count;


            //----------reward data

            UISystem.FindImage(gameObject, "img-rewardicon").sprite = m_AchData.m_Reward.m_Icon;
            switch (m_AchData.m_Reward.m_Type)
            {
                case "coin":
                    UISystem.FindText(gameObject, "text-rewardcount").text = "+" + m_AchData.m_Reward.m_Count.ToString();
                    break;
                case "gem":
                    UISystem.FindText(gameObject, "text-rewardcount").text = "+" + m_AchData.m_Reward.m_Count.ToString();
                    break;
            }
            //------------
            UpdateInfo();
        }

        // Update is called once per frame
        void Update()
        {
            Image bar = UISystem.FindImage(gameObject, "img-bar");
            Image barBack = UISystem.FindImage(gameObject, "img-barback");

            float factor = (float)m_AchData.Counter / (float)m_AchData.CounterMax;
            //bar.rectTransform.rect = new Vector2(factor * barBack.rectTransform.rect.width, 0);
            bar.rectTransform.offsetMin = Vector2.zero;
            bar.rectTransform.offsetMax = new Vector2(-(1 - factor) * barBack.rectTransform.rect.size.x, 0);
        }
        public void UpdateInfo()
        {
            Image btn = UISystem.FindImage(gameObject, "BtnGetReward");
            Image star = UISystem.FindImage(gameObject, "img-star");

            if (m_AchData.m_Achieved || m_AchData.Counter == m_AchData.CounterMax)
            {
                star.gameObject.SetActive(true);
                if (m_AchData.m_GotReward)
                {
                    btn.gameObject.SetActive(false);
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
            }
        }
        public void BtnClicked()
        {
            //MainMenuTabsUI.m_Main.UpdateTabAlerts();

            if (m_AchData.m_Reward.m_Type == "coin")
            {
                m_DataStorage.Coin += m_AchData.m_Reward.m_Count;
                m_AchData.m_Reward.m_Aquired = true;
                m_AchData.m_GotReward = true;

                // UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 1, m_UITextContentsContents.m_Messages[5], m_UIGraphicContents.m_Graphics[7]);
                UISystem.ShowCoinReward(m_AchData.m_Reward.m_Count);
            }
            else
            {
                m_DataStorage.Gem += m_AchData.m_Reward.m_Count;
                m_AchData.m_Reward.m_Aquired = true;
                m_AchData.m_GotReward = true;
                //UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 1, m_UITextContentsContents.m_Messages[10], m_UIGraphicContents.m_Graphics[6]);
                UISystem.ShowGemReward(m_AchData.m_Reward.m_Count);
            }

            //MainMenuTabsUI.m_Main.UpdateTabAlerts();
            m_DataStorage.SaveData();
            UpdateInfo();
            SoundGallery.PlaySound("pop1");
        }
    }
}