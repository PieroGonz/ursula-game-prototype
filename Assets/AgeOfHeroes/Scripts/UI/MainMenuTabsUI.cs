using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AgeOfHeroes.ScriptableObjects;
namespace AgeOfHeroes
{
    public class MainMenuTabsUI : MonoBehaviour
    {
        //public Image[] m_Tabs;
        public Image[] m_TabButtons;
        [HideInInspector]
        public int m_CurrentTab = 0;
        [HideInInspector]
        public GameObject m_CurrentTabObject;

        public string[] m_TabUIs;

        public Sprite[] m_TebBtnSprites;



        public static MainMenuTabsUI m_Main;

        [SerializeField, Space]
        private Contents m_Contents;
        [SerializeField, Space]
        private DataStorage m_DataStorage;
        [SerializeField, Space]
        private PlayerData m_PlayerData;
        [TextAreaAttribute]
        public string[] str_MessageUI_Alert;
        public Sprite[] spr_MessageUI_Alert;

        void Awake()
        {
            m_Main = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            if (UISystem.m_Main.m_UIData.m_UseOverrideTab)
            {
                m_CurrentTab = UISystem.m_Main.m_UIData.m_OverrideTabNum;
                UISystem.m_Main.m_UIData.m_UseOverrideTab = false;
            }
            SelectTab(m_CurrentTab);
            //UpdateTabAlerts();
            //if (m_CurrentTab == 0)
            //{
            //UpdateMessageAlerts();
            //}


            // Invoke("test", 2);
        }

        // public void test()
        // {
        //      DbControl.m_Current.GetOpponentData("Guest39474@guest.com");
        // }
        // Update is called once per frame
        void Update()
        {
        }

        public void SelectTab(int num)
        {
            if (m_CurrentTabObject != null)
            {
                UISystem.RemoveUI(m_CurrentTabObject);
                m_CurrentTabObject = null;
            }

            m_CurrentTabObject = UISystem.ShowUI(m_TabUIs[num]);
            Canvas canvas = m_CurrentTabObject.GetComponentInChildren<Canvas>();
            if (canvas != null)
            {
                canvas.sortingOrder = -1;
            }

            Vector2[] btnSizes = new Vector2[6];
            btnSizes[0] = new Vector2(120, 140);
            btnSizes[1] = new Vector2(120, 100);
            btnSizes[2] = new Vector2(120, 100);
            btnSizes[3] = new Vector2(120, 100);
            btnSizes[4] = new Vector2(120, 100);
            btnSizes[5] = new Vector2(100, 90);

            for (int i = 0; i < m_TabButtons.Length; i++)
            {
                m_TabButtons[i].rectTransform.sizeDelta = btnSizes[i];
                m_TabButtons[i].sprite = m_TebBtnSprites[0];
                //m_TabButtons[i].color = new Color(0.7f, 0.7f, 0.7f, 1);
                Image img = UISystem.FindImage(m_TabButtons[i].gameObject, "img-tabicon");
                img.transform.localScale = 0.9f * Vector3.one;
            }

            m_TabButtons[num].rectTransform.sizeDelta = btnSizes[num] + new Vector2(0, 20);
            m_TabButtons[num].sprite = m_TebBtnSprites[1];
            m_TabButtons[num].color = Color.white;
            Image img1 = UISystem.FindImage(m_TabButtons[num].gameObject, "img-tabicon");
            img1.transform.localScale = Vector3.one;
            m_CurrentTab = num;


            if (m_CurrentTabObject != null)
            {
                switch (num)
                {
                    case 3:
                        UITabControl tab = m_CurrentTabObject.GetComponentInChildren<UITabControl>();
                        if (tab != null)
                        {
                            tab.SelectTab(1);
                        }
                        break;
                }

            }
            if (m_CurrentTab == 0)
            {
                //UpdateMessageAlerts();
            }
            else
            {
                //DeactivateMessageAlerts();
            }

            SoundGallery.PlaySound("Button8");
        }

        //public void UpdateMessageAlerts()
        //{
        //    Image[] messagePanels = new Image[6];
        //    Image[] messageImages = new Image[6];
        //    Text[] messageTexts = new Text[6];
        //    for (int i = 1; i < 6; i++)
        //    {
        //        messagePanels[i] = UISystem.FindImage(m_TabButtons[i].gameObject, "MessagePanel");
        //        messageImages[i] = UISystem.FindImage(m_TabButtons[i].gameObject, "img-message");
        //        messageTexts[i] = UISystem.FindText(m_TabButtons[i].gameObject, "txt-message");

        //        messagePanels[i].gameObject.SetActive(false);
        //    }
        //    int messagenum = Random.Range(1, 6);
        //    switch (messagenum)
        //    {
        //        case 1:
        //            messageImages[1].sprite = spr_MessageUI_Alert[Random.Range(0, 6)];
        //            messageTexts[1].text = str_MessageUI_Alert[Random.Range(0, 3)];
        //            messagePanels[1].gameObject.SetActive(true);
        //            break;

        //        case 2:
        //            messageImages[2].sprite = spr_MessageUI_Alert[Random.Range(7, 9)];
        //            messageTexts[2].text = str_MessageUI_Alert[Random.Range(7, 9)];
        //            messagePanels[2].gameObject.SetActive(true);
        //            break;
        //        case 3:
        //            messageImages[3].sprite = spr_MessageUI_Alert[6];
        //            messageTexts[3].text = str_MessageUI_Alert[6];
        //            messagePanels[3].gameObject.SetActive(true);
        //            break;
        //        case 4:
        //            messageImages[4].sprite = m_Contents.m_AllEquipment[Random.Range(0, 25)].m_Icon;
        //            messageTexts[4].text = str_MessageUI_Alert[Random.Range(3, 6)];
        //            messagePanels[4].gameObject.SetActive(true);
        //            break;
        //        case 5:
        //            messageImages[5].sprite = spr_MessageUI_Alert[Random.Range(9, 11)];
        //            messageTexts[5].text = str_MessageUI_Alert[Random.Range(9, 11)];
        //            messagePanels[5].gameObject.SetActive(true);
        //            break;

        //    }

        //}
        //public void DeactivateMessageAlerts()
        //{
        //    Image[] messagePanels = new Image[6];

        //    for (int i = 1; i < 6; i++)
        //    {
        //        messagePanels[i] = UISystem.FindImage(m_TabButtons[i].gameObject, "MessagePanel");
        //        messagePanels[i].gameObject.SetActive(false);
        //    }
        //}

        //public void UpdateTabAlerts()
        //{
        //    //----------------progress and achievment rewards
        //    int count = 0;
        //    //for (int i = 0; i < m_Contents.m_Achievements.Length; i++)
        //    //{
        //    //    if ((m_Contents.m_Achievements[i].m_Achieved || m_Contents.m_Achievements[i].Counter >= m_Contents.m_Achievements[i].CounterMax)
        //    //        && !m_Contents.m_Achievements[i].m_GotReward)
        //    //    {
        //    //        count++;
        //    //    }
        //    //}

        //    //for (int i = 1; i < m_PlayerData.m_PlayerLevel; i++)
        //    //{
        //    //    if (!m_Contents.m_LevelupRewards[i].m_Aquired)
        //    //    {
        //    //        count++;
        //    //    }
        //    //}

        //    Image img = UISystem.FindImage(m_TabButtons[5].gameObject, "img-alert");
        //    if (count > 0)
        //    {
        //        img.gameObject.SetActive(true);
        //        Text txt = UISystem.FindText(m_TabButtons[5].gameObject, "text-alert-count");
        //        txt.text = count.ToString();
        //    }
        //    else
        //    {
        //        img.gameObject.SetActive(false);
        //    }

        //    bool found = false;
        //    //----------------unit purchase and upgrades
        //    for (int i = 0; i < m_Contents.m_AllEquipment.Length; i++)
        //    {
        //        if (!m_Contents.m_AllEquipment[i].m_Unlocked
        //            && !m_Contents.m_AllEquipment[i].m_UnlockedAtStart)
        //        {
        //            if (m_DataStorage.Coin >= m_Contents.m_AllEquipment[i].m_PriceInCoin)
        //            {
        //                found = true;

        //                break;
        //            }
        //        }

        //        for (int j = 0; j < m_Contents.m_AllEquipment[i].m_PowerLevels.Length; j++)
        //        {
        //            if (m_Contents.m_AllEquipment[i].m_HavePowers[j] && m_Contents.m_AllEquipment[i].m_PowerLevels[j] < 8)
        //            {
        //                int price = m_Contents.m_UpgradePriceStart[j]
        //                    + (m_Contents.m_UpgradePriceAdd[j] * m_Contents.m_AllEquipment[i].m_PowerLevels[j]);

        //                if (m_DataStorage.Coin >= price)
        //                {

        //                    found = true;
        //                    break;
        //                }
        //            }
        //        }

        //        if (found)
        //            break;
        //    }

        //    if (!found)
        //    {
        //        for (int i = 0; i < m_Contents.m_BarnSkin.Length; i++)
        //        {
        //            if (!m_Contents.m_BarnSkin[i].m_Unlocked
        //                && !m_Contents.m_BarnSkin[i].m_UnlockedAtStart
        //                && m_Contents.m_BarnSkin[i].m_PriceInGem <= m_DataStorage.Gem)
        //            {
        //                found = true;

        //                break;
        //            }
        //        }
        //    }

        //    if (!found)
        //    {
        //        for (int i = 0; i < m_Contents.m_Decoratives.Length; i++)
        //        {
        //            if (!m_Contents.m_Decoratives[i].m_Unlocked
        //                && !m_Contents.m_Decoratives[i].m_UnlockedAtStart
        //                && m_Contents.m_Decoratives[i].m_PriceInGem <= m_DataStorage.Gem)
        //            {
        //                found = true;

        //                break;
        //            }
        //        }
        //    }

        //    if (!found)
        //    {
        //        for (int i = 0; i < m_Contents.m_Flags.Length; i++)
        //        {
        //            if (!m_Contents.m_Flags[i].m_Unlocked
        //                && !m_Contents.m_Flags[i].m_UnlockedAtStart
        //                && m_Contents.m_Flags[i].m_PriceInGem <= m_DataStorage.Gem)
        //            {
        //                found = true;
        //                Debug.Log("Found--" + m_Contents.m_Flags[i].m_TitleFarsi);
        //                break;
        //            }
        //        }
        //    }


        //    img = UISystem.FindImage(m_TabButtons[4].gameObject, "img-alert");
        //    if (found)
        //    {
        //        img.gameObject.SetActive(true);
        //        //Text txt = UISystem.FindText(m_TabButtons[4].gameObject, "text-alert-count");
        //        //txt.text = count.ToString();
        //    }
        //    else
        //    {
        //        img.gameObject.SetActive(false);
        //    }

    }

}