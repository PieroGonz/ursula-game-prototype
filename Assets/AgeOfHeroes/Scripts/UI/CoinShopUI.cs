using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgeOfHeroes.ScriptableObjects;

using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace AgeOfHeroes.UI
{
    public class CoinShopUI : MonoBehaviour
    {
        // Start is called before the first frame update
        public Text m_BuyAll;
        public Button m_FreeCoinBtn;
        public Text m_FreeCoinTxt;

        public bool m_ClickedFreeCoin = false;

        public Text[] m_PriceTagTxt;
        public Button[] m_PriceTagBtn;

        public Image[] m_ScrollArrows;
        public RectTransform m_ScrollContent;

        public Image[] m_ScrollArrows2;
        public RectTransform m_ScrollContent2;

        [SerializeField, Space]
        private Contents m_Contents;
        [SerializeField, Space]
        private DataStorage m_DataStorage;
        [SerializeField, Space]
        private GameplayData m_GameplayData;
        [SerializeField, Space]
        private UIGraphicContents m_UIGraphicContents;
        [SerializeField, Space]
        private UITextContents m_UITextContentsContents;

        private (string, int)[] m_CoinProduct;
        private (string, int)[] m_GemProduct;
        private (string, int)[] m_PowerProduct;
        public static CoinShopUI m_Current;
        void Awake()
        {
            m_Current = this;
        }
        void Start()
        {
            CheckUnlockItems();



            m_CoinProduct = new (string, int)[6];
            m_CoinProduct[0] = ("", 50);
            m_CoinProduct[1] = ("Champions_CoinPack0", 1000);
            m_CoinProduct[2] = ("Champions_CoinPack1", 2000);
            m_CoinProduct[3] = ("Champions_CoinPack2", 5000);
            m_CoinProduct[4] = ("Champions_CoinPack3", 10000);
            m_CoinProduct[5] = ("Champions_CoinPack4", 15000);

            m_GemProduct = new (string, int)[6];
            m_GemProduct[0] = ("Champions_GemPack0", 10);
            m_GemProduct[1] = ("Champions_GemPack1", 20);
            m_GemProduct[2] = ("Champions_GemPack2", 50);
            m_GemProduct[3] = ("Champions_GemPack3", 100);
            m_GemProduct[4] = ("Champions_GemPack4", 200);
            m_GemProduct[5] = ("Champions_GemPack5", 500);

            m_PowerProduct = new (string, int)[5];
            m_PowerProduct[0] = ("Champions_PowerPack0", 5);
            m_PowerProduct[1] = ("Champions_PowerPack1", 5);
            m_PowerProduct[2] = ("Champions_PowerPack2", 5);
            m_PowerProduct[3] = ("Champions_PowerPack3", 5);
            m_PowerProduct[4] = ("Champions_PowerPack4", 1);


        }
        void OnDestroy()
        {

        }
        // Update is called once per frame
        void Update()
        {
            if (m_DataStorage.VideoCountSeen >= 4)
            {
                m_FreeCoinBtn.interactable = false;

                if (TimeControl.m_Main.m_TimeList["time-free-coin-video"].m_ReachedEnd)
                {
                    m_DataStorage.VideoCountSeen = 0;
                }
                else
                {
                    int hourleft = (int)TimeControl.m_Main.m_TimeList["time-free-coin-video"].m_HoursCount - (TimeControl.m_Main.m_TimeList["time-free-coin-video"].m_DeltaTime.Hours) - 1;
                    int minuteleft = 60 - TimeControl.m_Main.m_TimeList["time-free-coin-video"].m_DeltaTime.Minutes;

                    m_FreeCoinBtn.interactable = false;
                    m_FreeCoinTxt.text = "Mins" + minuteleft + " &" + "Hour" + hourleft;
                }
            }
            else
            {
                m_FreeCoinTxt.text = "Watch video";
                m_FreeCoinBtn.interactable = true;
            }

            if (m_ScrollContent.anchoredPosition.x < -50)
            {
                m_ScrollArrows[0].gameObject.SetActive(true);
            }
            else
            {
                m_ScrollArrows[0].gameObject.SetActive(false);
            }

            if (m_ScrollContent.anchoredPosition.x > -500)
            {
                m_ScrollArrows[1].gameObject.SetActive(true);
            }
            else
            {
                m_ScrollArrows[1].gameObject.SetActive(false);
            }

            /////////------------------------------

            if (m_ScrollContent2.anchoredPosition.x < -20)
            {
                m_ScrollArrows2[0].gameObject.SetActive(true);
            }
            else
            {
                m_ScrollArrows2[0].gameObject.SetActive(false);
            }
        }
        public void BtnCoinPack(int num)
        {
            if (num == 0)
            {
                if (Application.platform != RuntimePlatform.Android)
                {
                    HandleGetFreeCoin();
                }
                else
                {
                    if (!m_DataStorage.CheckInternet())
                    {
                        UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 1, m_UITextContentsContents.m_Messages[0], m_UIGraphicContents.m_Graphics[4]);
                        //msg.AutoCloseMessage(1);
                    }
                    else
                    {
                        if (!m_ClickedFreeCoin)
                        {
                            m_ClickedFreeCoin = true;

                            Invoke("ResetClickFreeCoin", 3);
                        }
                    }
                }
                SoundGallery.PlaySound("button32");

            }
            else
            {
                if (!m_DataStorage.CheckInternet())
                {
                    UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 1, m_UITextContentsContents.m_Messages[0], m_UIGraphicContents.m_Graphics[4]);

                }
                else
                {
                    if (Application.platform != RuntimePlatform.Android)
                    {
                        m_DataStorage.Coin += m_CoinProduct[num].Item2;
                        // UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 1, m_UITextContentsContents.m_Messages[5], m_UIGraphicContents.m_Graphics[7]);
                        UISystem.ShowCoinReward(m_CoinProduct[num].Item2);
                        m_DataStorage.SaveData();
                    }
                    else
                    {
#if UNITY_ANDROID

#endif
                    }
                }
            }

            SoundGallery.PlaySound("button32");
        }

        public void BtnPowerPack(int num)
        {

            if (!m_DataStorage.CheckInternet())
            {
                UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 1, m_UITextContentsContents.m_Messages[0], m_UIGraphicContents.m_Graphics[5]);
                msg.AutoCloseMessage(1);
            }
            else
            {
                if (Application.platform != RuntimePlatform.Android)
                {

                    if (num == 4)
                    {
                        for (int i = 0; i < m_DataStorage.PowerCounts.Length; i++)
                        {
                            m_DataStorage.PowerCounts[i] += m_PowerProduct[num].Item2;
                        }

                    }
                    else
                    {
                        m_DataStorage.PowerCounts[num] += m_PowerProduct[num].Item2;
                    }
                    m_DataStorage.SaveData();
                    UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 1, m_UITextContentsContents.m_Messages[50], m_UIGraphicContents.m_Graphics[5]);

                }
                else
                {
#if UNITY_ANDROID

#endif
                }
            }

            SoundGallery.PlaySound("button32");
        }

        public void BtnGemPack(int num)
        {

            if (!m_DataStorage.CheckInternet())
            {
                UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 1, m_UITextContentsContents.m_Messages[0], m_UIGraphicContents.m_Graphics[4]);
                msg.AutoCloseMessage(1);
            }
            else
            {
                if (Application.platform != RuntimePlatform.Android)
                {
                    m_DataStorage.Gem += m_GemProduct[num].Item2;
                    m_DataStorage.SaveData();
                    //UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 1, m_UITextContentsContents.m_Messages[10], m_UIGraphicContents.m_Graphics[6]);
                    UISystem.ShowGemReward(m_GemProduct[num].Item2);

                }
                else
                {
#if UNITY_ANDROID

#endif
                }
            }

            SoundGallery.PlaySound("button32");
        }

        public void BtnPurchaseItems(string productID)
        {

            if (!m_DataStorage.CheckInternet())
            {
                UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 1, m_UITextContentsContents.m_Messages[0], m_UIGraphicContents.m_Graphics[4]);
                msg.AutoCloseMessage(1);
            }
            else
            {
                switch (productID)
                {
                    case "Champions_RostamPack":
                        if (Application.platform != RuntimePlatform.Android)
                        {

                            for (int j = 0; j < m_Contents.m_Characters.Length; j++)
                            {
                                if (!m_Contents.m_Characters[j].m_Unlocked && m_Contents.m_Characters[j].m_ItemID == "Rostam")
                                {
                                    for (int i = 0; i < m_Contents.m_Characters[j].m_Abilities.Length; i++)
                                    {
                                        m_Contents.m_Characters[j].m_Abilities[i].m_Unlocked = true;
                                    }
                                    for (int k = 0; k < m_Contents.m_Characters[j].m_Weapons.Length; k++)
                                    {
                                        m_Contents.m_Characters[j].m_Weapons[k].m_Unlocked = true;
                                    }
                                    for (int l = 0; l < m_Contents.m_Characters[j].m_Skins.Length; l++)
                                    {
                                        m_Contents.m_Characters[j].m_Skins[l].m_Unlocked = true;
                                    }
                                    m_Contents.m_Characters[j].m_Unlocked = true;
                                    m_DataStorage.SaveData();
                                }
                            }
                            CheckUnlockItems();
                            UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 1, m_UITextContentsContents.m_Messages[36], m_UIGraphicContents.m_Graphics[6]);
                            msg.f_Clicked_Exit = ExitMessage;

                        }
#if UNITY_ANDROID

#endif

                        break;
                    case "Champions_CommandoPack":
                        if (Application.platform != RuntimePlatform.Android)
                        {
                            for (int j = 0; j < m_Contents.m_Characters.Length; j++)
                            {
                                if (!m_Contents.m_Characters[j].m_Unlocked && m_Contents.m_Characters[j].m_ItemID == "Valerie")
                                {
                                    for (int i = 0; i < m_Contents.m_Characters[j].m_Abilities.Length; i++)
                                    {
                                        m_Contents.m_Characters[j].m_Abilities[i].m_Unlocked = true;
                                    }
                                    for (int k = 0; k < m_Contents.m_Characters[j].m_Weapons.Length; k++)
                                    {
                                        m_Contents.m_Characters[j].m_Weapons[k].m_Unlocked = true;
                                    }
                                    for (int l = 0; l < m_Contents.m_Characters[j].m_Skins.Length; l++)
                                    {
                                        m_Contents.m_Characters[j].m_Skins[l].m_Unlocked = true;
                                    }
                                    m_Contents.m_Characters[j].m_Unlocked = true;
                                    m_DataStorage.SaveData();
                                }
                            }

                            CheckUnlockItems();
                            UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 1, m_UITextContentsContents.m_Messages[37], m_UIGraphicContents.m_Graphics[7]);
                            msg.f_Clicked_Exit = ExitMessage;
                        }
#if UNITY_ANDROID

#endif
                        break;

                    case "Champions_SamuraiPack":
                        if (Application.platform != RuntimePlatform.Android)
                        {
                            for (int j = 0; j < m_Contents.m_Characters.Length; j++)
                            {
                                if (!m_Contents.m_Characters[j].m_Unlocked && m_Contents.m_Characters[j].m_ItemID == "Moojung")
                                {
                                    for (int i = 0; i < m_Contents.m_Characters[j].m_Abilities.Length; i++)
                                    {
                                        m_Contents.m_Characters[j].m_Abilities[i].m_Unlocked = true;
                                    }
                                    for (int k = 0; k < m_Contents.m_Characters[j].m_Weapons.Length; k++)
                                    {
                                        m_Contents.m_Characters[j].m_Weapons[k].m_Unlocked = true;
                                    }
                                    for (int l = 0; l < m_Contents.m_Characters[j].m_Skins.Length; l++)
                                    {
                                        m_Contents.m_Characters[j].m_Skins[l].m_Unlocked = true;
                                    }
                                    m_Contents.m_Characters[j].m_Unlocked = true;
                                    m_DataStorage.SaveData();
                                }
                            }

                            CheckUnlockItems();
                            UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 1, m_UITextContentsContents.m_Messages[38], m_UIGraphicContents.m_Graphics[8]);
                            msg.f_Clicked_Exit = ExitMessage;
                        }
#if UNITY_ANDROID

#endif
                        break;
                    case "Champions_CustomizeAvatar":
                        if (Application.platform != RuntimePlatform.Android)
                        {
                            if (!m_DataStorage.m_CustomizeProfilePic)
                            {
                                m_DataStorage.m_CustomizeProfilePic = true;
                                m_DataStorage.SaveData();
                                CheckUnlockItems();
                            }
                        }
#if UNITY_ANDROID

#endif

                        break;
                }


            }

            SoundGallery.PlaySound("button32");
        }
        public void BtnClose()
        {
            m_ClickedFreeCoin = false;

            //if (SceneManager.GetActiveScene().name == "MainMenu" && UISystem.FindUIByName("MainMenuUI") == null)
            //    UISystem.ShowUI("MainMenuUI");
            UISystem.RemoveUI(gameObject);
            SoundGallery.PlaySound("button32");
        }

        public void ResetClickFreeCoin()
        {
            m_ClickedFreeCoin = false;
        }

        public void HandleGetFreeCoin()
        {
            m_DataStorage.VideoCountSeen++;
            if (m_DataStorage.VideoCountSeen >= 4)
            {
                TimeControl.m_Main.m_TimeInstances[2].Reset();
            }

            m_DataStorage.Coin += 50;
            m_DataStorage.SaveData();
            //m_DataStorage.UploadDataonDatabase("Coin");
            UISystem.ShowCoinReward(50);
            //UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 1, m_UITextContentsContents.m_Messages[5], m_UIGraphicContents.m_Graphics[7]);

        }
        public void CheckUnlockItems()
        {
            bool rostamUnlocked = false;
            bool valerieUnlocked = false;
            bool moojungUnlocked = false;
            for (int j = 0; j < m_Contents.m_Characters.Length; j++)
            {
                if (m_Contents.m_Characters[j].m_Unlocked)
                {
                    switch (m_Contents.m_Characters[j].m_ItemID)
                    {
                        case "Rostam":
                            m_PriceTagBtn[0].gameObject.SetActive(false);
                            rostamUnlocked = true;
                            break;
                        case "Valerie":
                            m_PriceTagBtn[1].gameObject.SetActive(false);
                            valerieUnlocked = true;
                            break;
                        case "Moojung":
                            m_PriceTagBtn[2].gameObject.SetActive(false);
                            moojungUnlocked = true;
                            break;

                    }
                }
                if (rostamUnlocked && valerieUnlocked && moojungUnlocked)
                {
                    m_BuyAll.gameObject.SetActive(true);
                }
                else
                {
                    m_BuyAll.gameObject.SetActive(false);
                }

            }

            if (m_DataStorage.m_RemoveAds)
            {
                m_PriceTagBtn[3].gameObject.SetActive(false);
            }
            if (m_DataStorage.m_CustomizeProfilePic)
            {
                m_PriceTagBtn[4].gameObject.SetActive(false);
            }
        }


        public bool ExitMessage()
        {
            GameObject obj = UISystem.FindOpenUIByName("SquadUI");
            if (obj != null)
            {
                obj.GetComponent<SquadUI>().UpdatePlayerInfo();
            }
            return true;
        }

    }


}
