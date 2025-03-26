using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgeOfHeroes.ScriptableObjects;
using UnityEngine.UI;
namespace AgeOfHeroes
{
    public class ProgressLevelsUI : MonoBehaviour
    {

        public GameObject m_ProgressPartPrefab;
        public RectTransform m_ContentPart;
        //public ProgressPart[] m_Parts;
        [SerializeField, Space]
        private DataStorage m_DataStorage;
        [SerializeField, Space]
        private PlayerData m_PlayerData;
        [SerializeField, Space]
        private Contents m_Contents;

        public Sprite[] m_RewardImages;

        // Start is called before the first frame update
        void Start()
        {
            int maxCount = 50;
            //m_Parts = new ProgressPart[maxCount];
            //float xStart = 860;

            //for (int i = 0; i < maxCount; i++)
            //{
            //    float size = 500 + i * 20;
            //    GameObject obj = Instantiate(m_ProgressPartPrefab);
            //    obj.transform.SetParent(m_ContentPart);
            //    obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(xStart, -126);
            //    obj.GetComponent<RectTransform>().sizeDelta = new Vector2(size, 150);
            //    xStart += size;
            //    m_Parts[i] = obj.GetComponent<ProgressPart>();
            //    m_Parts[i].m_RewardImage.sprite = m_Contents.m_LevelupRewards[i].m_Icon;

            //    switch (m_Contents.m_LevelupRewards[i].m_Type)
            //    {
            //        case "coin":
            //            m_Parts[i].m_Text_RewardText.text = "+" + m_Contents.m_LevelupRewards[i].m_Count.ToString();
            //            break;
            //        case "gem":
            //            m_Parts[i].m_Text_RewardText.text = "+" + m_Contents.m_LevelupRewards[i].m_Count.ToString();
            //            break;
            //    }
            //}
            //print(xStart);

            //m_Parts[0].m_RewardPanel.gameObject.SetActive(false);

            //Vector2 pos = m_Parts[m_PlayerData.m_PlayerLevel].GetComponent<RectTransform>().anchoredPosition - new Vector2(500, 0);
            //pos.y = 0;
            //m_ContentPart.anchoredPosition = new Vector2(25500, 0) - pos;

            int minXp = 0;
            int maxXp = 100;
            for (int i = 0; i < m_PlayerData.m_PlayerLevel; i++)
            {
                minXp += 100 + (40 * i);
                maxXp += 100 + (40 * (i + 1));
            }

            //m_Parts[0].m_Text_Level.text = m_PlayerData.m_PlayerLevel.ToString();
            //m_Parts[0].m_Text_NextLevel.text = (m_PlayerData.m_PlayerLevel+1).ToString();
            //m_Parts[i].m_RewardBtnCounter = i;

            //m_Parts[0].m_XPFactor = ((float)m_DataStorage.m_PlayerXP / (float)XPBarAdd.Current.m_MaxXP);
            //m_Parts[0].m_Text_CurrentXP.text = (minXp + m_DataStorage.m_PlayerXP).ToString();
            //m_Parts[0].m_LevelBack.sprite = m_Parts[i].m_LevelBackSprites[1];
            //m_Parts[0].m_RewardPanel.transform.localScale = Vector3.one;
            //m_Parts[0].m_RewardPanel.GetComponent<CanvasGroup>().alpha = 1;
            //m_Parts[0].m_Text_MaxXP.text = maxXp.ToString();


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
                    //m_Parts[0].m_RewardImage.sprite = m_RewardImages[0];
                    //m_Parts[0].m_Text_RewardText.text = coin.ToString();
                    break;
                case 1:
                    //m_Parts[0].m_RewardImage.sprite = m_RewardImages[1];
                    //m_Parts[0].m_Text_RewardText.text = gem.ToString();
                    break;
            }
            //if (m_Parts[i].m_LevelUpReward[i].m_Aquired)
            //{
            //    m_Parts[i].m_RewardPanel.gameObject.SetActive(false);
            //}


        }

        // Update is called once per frame
        void Update()
        {
            //int maxCount = 50;
            //int minXp = 0;
            //int maxXp = 100;
            //for (int i = 0; i < maxCount; i++)
            //{
            //    m_Parts[i].m_Text_Level.text = i.ToString();
            //    m_Parts[i].m_RewardBtnCounter = i;
            //    if (i > m_PlayerData.m_PlayerLevel)
            //    {
            //        m_Parts[i].m_XPFactor = 0;
            //        m_Parts[i].m_Text_CurrentXP.gameObject.SetActive(false);
            //        m_Parts[i].m_LevelBack.sprite = m_Parts[i].m_LevelBackSprites[0];
            //        m_Parts[i].m_RewardPanel.transform.localScale = 0.6f * Vector3.one;
            //        m_Parts[i].m_RewardPanel.GetComponent<CanvasGroup>().alpha = .5f;
            //        m_Parts[i].m_RewardPanel.GetComponent<Button>().interactable = false;
            //    }
            //    else if (i == m_PlayerData.m_PlayerLevel)
            //    {
            //        m_Parts[i].m_XPFactor = ((float)m_DataStorage.m_PlayerXP / (float)XPBarAdd.Current.m_MaxXP);
            //        m_Parts[i].m_Text_CurrentXP.text = (minXp + m_DataStorage.m_PlayerXP).ToString();
            //        m_Parts[i].m_LevelBack.sprite = m_Parts[i].m_LevelBackSprites[1];
            //        m_Parts[i].m_RewardPanel.transform.localScale = Vector3.one;
            //        m_Parts[i].m_RewardPanel.GetComponent<CanvasGroup>().alpha = 1;
            //    }
            //    else
            //    {
            //        m_Parts[i].m_Text_CurrentXP.gameObject.SetActive(false);
            //        m_Parts[i].m_XPFactor = 1;
            //        m_Parts[i].m_LevelBack.sprite = m_Parts[i].m_LevelBackSprites[1];
            //        m_Parts[i].m_RewardPanel.transform.localScale = Vector3.one;
            //        m_Parts[i].m_RewardPanel.GetComponent<CanvasGroup>().alpha = 1;

            //    }
            //    if (m_Parts[i].m_LevelUpReward[i].m_Aquired)
            //    {
            //        m_Parts[i].m_RewardPanel.gameObject.SetActive(false);
            //    }

            //    m_Parts[i].m_Text_MaxXP.text = maxXp.ToString();
            //    maxXp += 100 + (40 * (i + 1));
            //    minXp += 100 + (40 * i);
            //}
        }

        public void BtnClose()
        {
            UISystem.RemoveUI(gameObject);
        }
    }
}