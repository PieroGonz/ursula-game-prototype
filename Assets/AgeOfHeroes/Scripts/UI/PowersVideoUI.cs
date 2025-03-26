using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using AgeOfHeroes.ScriptableObjects;
using AgeOfHeroes.Gameplay;

namespace AgeOfHeroes.UI
{


    public class PowersVideoUI : MonoBehaviour
    {
        [SerializeField]
        private Image[] m_PassImages;
        [SerializeField]
        private Text[] m_PassTexts;
        [SerializeField]
        private Text[] m_SheepText;
        [SerializeField]
        private Button[] m_PowerButtons;

        [SerializeField]
        private Button m_BtnOk;

        public Image[] m_PowerCountBacks;
        public Text[] m_PowerCounts;
        public Button m_PowerVideoBtn;
        public Text m_PowerVideoTxt;

        [SerializeField]
        private DataStorage m_DataStorage;
        [SerializeField, Space]
        private GameplayData m_GameplayData;
        [SerializeField, Space]
        private Contents m_Contents;
        [SerializeField, Space]
        private UIGraphicContents m_UIGraphicContents;
        [SerializeField, Space]
        private UITextContents m_UITextContentsContents;
        private int m_PowerNum;

        void Start()
        {

            for (int i = 0; i < 3; i++)
            {
                m_PassImages[i].gameObject.SetActive(false);
            }
            m_PassImages[m_PowerNum].gameObject.SetActive(true);
            m_PassTexts[1].gameObject.SetActive(false);

        }

        void Update()
        {
            Image videoimage = UISystem.FindImage(gameObject, "img-video");
            Image timeimage = UISystem.FindImage(gameObject, "img-time");

            if (m_DataStorage.m_PowersVideoCountSeen >= 3)
            {

                if (TimeControl.m_Main.m_TimeList["time-powers-video"].m_ReachedEnd)
                {
                    m_DataStorage.m_PowersVideoCountSeen = 0;
                    videoimage.gameObject.SetActive(true);
                    timeimage.gameObject.SetActive(false);
                    TimeControl.m_Main.m_TimeList["time-powers-video"].Reset();
                }
                else
                {
                    int hourleft = 12 - (TimeControl.m_Main.m_TimeList["time-powers-video"].m_DeltaTime.Hours) - 1;
                    int minuteleft = 60 - TimeControl.m_Main.m_TimeList["time-powers-video"].m_DeltaTime.Minutes;

                    videoimage.gameObject.SetActive(false);
                    timeimage.gameObject.SetActive(true);
                    m_PowerVideoBtn.interactable = false;
                    m_PowerVideoTxt.text = "ﻪﻘﯿﻗﺩ " + minuteleft + " و " + "ﺖﻋﺎﺳ " + hourleft;
                }


            }
            else
            {
                m_PowerVideoTxt.text = "ﻦﯿﺒﺑ ﻩﺎﺗﻮﮐ ﻮﯾﺪﯾﻭ";
                m_PowerVideoBtn.interactable = true;
            }

            //
            if (m_GameplayData.m_CheckReward)
            {
                m_DataStorage.PowerCounts[m_PowerNum]++;
                DisableButtons();
                m_DataStorage.SaveData();
                m_GameplayData.m_CheckReward = false;
                UIMessage_C msg = UISystem.ShowMessage_C("UIMessage_C", 1, m_UITextContentsContents.m_Messages[14], m_Contents.m_Powers[m_PowerNum].m_Icon);

            }

            for (int i = 0; i < 3; i++)
            {
                m_PowerCounts[i].text = m_DataStorage.PowerCounts[i].ToString();
            }

        }
        public void BtnPowerPack(int num)
        {

            m_PowerNum = num;

            for (int i = 0; i < 3; i++)
            {
                m_PassImages[i].gameObject.SetActive(false);
                m_SheepText[i].gameObject.SetActive(false);
            }
            m_PassImages[m_PowerNum].gameObject.SetActive(true);
            m_SheepText[m_PowerNum].gameObject.SetActive(true);

            SoundGallery.PlaySound("button32");

        }


        public void Btn_Buy()
        {
            int price = 1500;
            if (m_DataStorage.Coin >= price)
            {
                m_DataStorage.Coin -= price;
                m_DataStorage.PowerCounts[m_PowerNum]++;
                UIMessage_C msg = UISystem.ShowMessage_C("UIMessage_C", 1, m_UITextContentsContents.m_Messages[14], m_Contents.m_Powers[m_PowerNum].m_Icon);
                m_DataStorage.SaveData();
            }
            else
            {
                if (m_GameplayData.m_PowerIngameUIButton)
                {
                    UIMessage_C msgs = UISystem.ShowMessage_C("UIMessage_C", 0, m_UITextContentsContents.m_Messages[20], m_UIGraphicContents.m_Graphics[7]);
                    m_GameplayData.m_PowerIngameUIButton = false;
                    Time.timeScale = 1;
                    msgs.f_Clicked_Yes = ShowStore;
                }
                else
                {
                    UIMessage_C msg = UISystem.ShowMessage_C("UIMessage_C", 0, m_UITextContentsContents.m_Messages[2], m_UIGraphicContents.m_Graphics[7]);
                    msg.f_Clicked_Yes = ShowStore;
                }


            }

            SoundGallery.PlaySound("button32");
        }
        public bool ShowStore()
        {
            if (m_GameplayData.m_PowerIngameUIButton)
            {
                BtnAddCoinOnExitGame();
            }
            else
            {
                m_GameplayData.m_PowerIngameUIButton = true;
                SceneManager.LoadScene("MainMenu");
                //m_GameplayData.m_PowerIngameUIButton = false;
                // m_AlertMessage = false;

                // if (MainMenuTabsUI.m_Main != null)
                // {
                //     MainMenuTabsUI.m_Main.SelectTab(3);
                //     MainMenuTabsUI.m_Main.m_CurrentTabObject.GetComponentInChildren<UITabControl>().SelectTab(2);
                //     UISystem.RemoveUI(gameObject);
                // }
                // else
                // {
                //     m_GameplayData.m_PowerIngameUIButton = true;
                //     SceneManager.LoadScene("MainMenu");

                // }
            }
            return true;
        }
        public void Btn_Video()
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
                    HandleGetFreePower();
                }
                else
                {

                }
            }
        }

        public void DisableButtons()
        {
            // for (int i = 0; i < 3; i++)
            // {
            //     m_PowerButtons[i].interactable = false;
            // }
            //  m_BtnOk.gameObject.SetActive(true);
        }

        public void BtnClose()
        {
            if (m_GameplayData.m_PowerIngameUIButton)
            {
                //GameControl.Current.ResumeGame();
                UISystem.RemoveUI(gameObject);
            }
            else
            {
                if (m_GameplayData.m_PowerInStoreUI)
                {
                    m_GameplayData.m_PowerInStoreUI = false;
                    MainMenuTabsUI.m_Main.SelectTab(3);
                    MainMenuTabsUI.m_Main.m_CurrentTabObject.GetComponentInChildren<UITabControl>().SelectTab(0);
                    //UISystem.ShowUI("StoreUI");
                }
                UISystem.RemoveUI(gameObject);
            }

            SoundGallery.PlaySound("button32");
        }

        public void HandleGetFreePower()
        {
            m_DataStorage.m_PowersVideoCountSeen++;
            if (m_DataStorage.m_PowersVideoCountSeen >= 3)
            {
                TimeControl.m_Main.m_TimeInstances[6].Reset();
            }
            m_GameplayData.m_CheckReward = true;
        }
        public void BtnAddCoinOnExitGame()
        {
            if (m_GameplayData.m_PowerIngameUIButton)
            {
                UIMessage_C msg = UISystem.ShowMessage_C("UIMessage_C", 1, m_UITextContentsContents.m_Messages[19], m_UIGraphicContents.m_Graphics[14]);

                msg.f_Clicked_Yes = HaltShowStore;
            }
            else
            {
                HaltShowStore();
            }
            SoundGallery.PlaySound("button32");
        }
        public bool HaltShowStore()
        {
            m_GameplayData.m_PowerIngameUIButton = false;
            ShowStore();
            return true;
        }


    }
}
