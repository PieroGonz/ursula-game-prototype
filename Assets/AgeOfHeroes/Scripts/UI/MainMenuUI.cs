using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgeOfHeroes.ScriptableObjects;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

using System.Text.RegularExpressions;


namespace AgeOfHeroes.UI
{

    public class MainMenuUI : MonoBehaviour
    {
        public Image m_WaitingPanel;
        public Image m_CommentBtn;
        [SerializeField] private Sprite[] m_Music;
        [SerializeField] private Image m_MusicMuteImage;

        private bool m_AllowCreateRandomUser;
        [SerializeField] private Button m_RandomRewardButton;
        [SerializeField] private Button m_RewardWheelButton;
        [SerializeField] private Button m_DailyOfferButton;
        [SerializeField] private Button m_FreeCoinButton;

        [SerializeField] private GameObject m_MenusPanel;

        [HideInInspector]
        public bool m_IsCheckingMulti = false;
        public Image m_MultiWait;

        [SerializeField, Space]
        private Contents m_Contents;
        [SerializeField, Space]
        private DataStorage m_DataStorage;
        [SerializeField, Space]
        private GameplayData m_GameplayData;
        [SerializeField, Space]
        private OpponentData m_OpponentData;
        [SerializeField, Space]
        private PlayerData m_PlayerData;

        [SerializeField, Space]
        private UIGraphicContents m_UIGraphicContents;
        [SerializeField, Space]
        private UITextContents m_UITextContentsContents;
        void Start()
        {
            m_GameplayData.m_GameMode = GameModes.None;
            m_MenusPanel.SetActive(false);


            if (m_DataStorage.MuteMusic)
            {
                m_MusicMuteImage.sprite = m_Music[1];
            }
            else
            {
                m_MusicMuteImage.sprite = m_Music[0];
            }
            if (m_DataStorage.m_PlayerCommented)
            {
                m_CommentBtn.gameObject.SetActive(false);
            }


            //if for time

            if (!m_DataStorage.m_NotFirstTimeInGame)
            {
                //m_DataStorage.m_NotFirstTimeInGame = true;
                //m_DataStorage.SaveData();
                BtnTutorial();
            }
            else
            {
                if (!m_DataStorage.m_GotDailyReward)
                {
                    TimeControl.m_Main.m_TimeInstances[6].Reset();
                    m_DataStorage.m_GotDailyReward = true;
                    m_DataStorage.SaveData();
                    UISystem.ShowUI("DailyRewardUI");
                }

                if (TimeControl.m_Main.m_TimeInstances[6].m_ReachedEnd)
                {
                    if (m_DataStorage.m_DayNumInGame < 9)
                    {
                        m_DataStorage.m_DayNumInGame++;
                    }
                    else
                    {
                        m_DataStorage.m_DayNumInGame = 0;
                    }


                    if (m_DataStorage.m_GotDailyReward)
                    {
                        m_DataStorage.m_GotDailyReward = false;
                    }
                    else
                    {
                        UISystem.ShowUI("DailyRewardUI");
                    }

                    m_DataStorage.SaveData();
                }
                //if (TimeControl.m_Main.m_TimeInstances[3].m_ReachedEnd && !m_GameplayData.m_UnlockAdsMessage && m_DataStorage.m_GotDailyReward)
                //{
                //    Invoke("ShowTuPuzUI", 2);
                //    TimeControl.m_Main.m_TimeInstances[3].Reset();
                //}

                if (m_GameplayData.m_UnlockAdsMessage && !m_DataStorage.m_RemoveAds)
                {
                    m_GameplayData.m_UnlockAdsMessage = false;
                    UIMessage_B msg = UISystem.ShowMessage_B("UIMessage_B", 5, m_UITextContentsContents.m_Messages[45], null);
                    Image img = UISystem.FindImage(msg.gameObject, "MessageImageFade_2");
                    img.sprite = m_UIGraphicContents.m_Graphics[17];
                    img.gameObject.SetActive(true);
                    msg.f_Clicked_OK = UnlockADs;
                }
            }

            m_WaitingPanel.gameObject.SetActive(false);

            m_IsCheckingMulti = false;
            m_MultiWait.gameObject.SetActive(false);

            UISystem.m_Main.m_SquadSettings[0].ResetToSavedCharacters();
        }
        public bool UnlockADs()
        {

            return true;
        }

        void OnDestroy()
        {

        }
        // Update is called once per frame
        void Update()
        {
            Image imgVideo = UISystem.FindImage(m_RandomRewardButton.gameObject, "img-video");
            Image imgTime = UISystem.FindImage(m_RandomRewardButton.gameObject, "img-time");
            Image imgCountBack = UISystem.FindImage(m_RandomRewardButton.gameObject, "img-countback");
            Text textCount = UISystem.FindText(m_RandomRewardButton.gameObject, "text-count");

            if (m_DataStorage.m_RandomReward > 0)
            {
                imgCountBack.gameObject.SetActive(true);
                imgVideo.gameObject.SetActive(false);
                imgTime.gameObject.SetActive(false);
                m_RandomRewardButton.interactable = true;
                textCount.text = m_DataStorage.m_RandomReward.ToString();
            }
            else
            {
                if (TimeControl.m_Main.m_TimeList["time-random-reward-video"].m_ReachedEnd)
                {
                    imgCountBack.gameObject.SetActive(false);
                    imgVideo.gameObject.SetActive(true);
                    imgTime.gameObject.SetActive(false);
                    m_RandomRewardButton.interactable = true;
                }
                else
                {
                    imgCountBack.gameObject.SetActive(false);
                    imgVideo.gameObject.SetActive(false);
                    imgTime.gameObject.SetActive(true);
                    m_RandomRewardButton.interactable = false;
                }
            }

            imgVideo = UISystem.FindImage(m_RewardWheelButton.gameObject, "img-video");
            imgTime = UISystem.FindImage(m_RewardWheelButton.gameObject, "img-time");
            imgCountBack = UISystem.FindImage(m_RewardWheelButton.gameObject, "img-countback");
            textCount = UISystem.FindText(m_RewardWheelButton.gameObject, "text-count");

            if (m_DataStorage.m_RewardWheelCount > 0)
            {
                imgCountBack.gameObject.SetActive(true);
                imgVideo.gameObject.SetActive(false);
                imgTime.gameObject.SetActive(false);
                m_RewardWheelButton.interactable = true;
                textCount.text = m_DataStorage.m_RewardWheelCount.ToString();
            }
            else
            {
                if (TimeControl.m_Main.m_TimeList["time-reward-wheel-video"].m_ReachedEnd)
                {
                    imgCountBack.gameObject.SetActive(false);
                    imgVideo.gameObject.SetActive(true);
                    imgTime.gameObject.SetActive(false);
                    m_RewardWheelButton.interactable = true;
                }
                else
                {
                    imgCountBack.gameObject.SetActive(false);
                    imgVideo.gameObject.SetActive(false);
                    imgTime.gameObject.SetActive(true);
                    m_RewardWheelButton.interactable = false;
                }
            }

            Image imgTimebarDailyOffer = UISystem.FindImage(m_DailyOfferButton.gameObject, "img-time-bar-dailyoffer");
            Image imgGiftDailyOffer = UISystem.FindImage(m_DailyOfferButton.gameObject, "img-gift-dailyoffer");
            Image imgFlashDailyOffer = UISystem.FindImage(m_DailyOfferButton.gameObject, "img-flash-dailyoffer");
            Text textCountDailyOffer = UISystem.FindText(m_DailyOfferButton.gameObject, "txt-time-dailyoffer");

            if (m_DataStorage.m_DailyOfferRecieved)
            {
                if (TimeControl.m_Main.m_TimeList["time-daily-offer"].m_ReachedEnd)
                {
                    //Debug.Log("ReachedEnd");
                    m_DailyOfferButton.interactable = true;
                    imgGiftDailyOffer.color = Color.white;
                    m_DailyOfferButton.GetComponent<UiAnim_Loop_ScaleA>().enabled = true;
                    imgTimebarDailyOffer.gameObject.SetActive(false);
                    textCountDailyOffer.gameObject.SetActive(false);
                    imgFlashDailyOffer.gameObject.SetActive(true);
                    m_DataStorage.m_DailyOfferRecieved = false;
                    TimeControl.m_Main.m_TimeList["time-daily-offer"].Reset();
                }
                else
                {
                    int hourleft = 12 - (TimeControl.m_Main.m_TimeList["time-daily-offer"].m_DeltaTime.Hours) - 1;
                    int minuteleft = 60 - TimeControl.m_Main.m_TimeList["time-daily-offer"].m_DeltaTime.Minutes;

                    imgTimebarDailyOffer.gameObject.SetActive(true);
                    textCountDailyOffer.gameObject.SetActive(true);
                    m_DailyOfferButton.interactable = false;
                    imgFlashDailyOffer.gameObject.SetActive(false);
                    m_DailyOfferButton.GetComponent<UiAnim_Loop_ScaleA>().enabled = false;
                    imgGiftDailyOffer.color = Color.gray;
                    textCountDailyOffer.text = "ﻪﻘﯿﻗﺩ " + minuteleft + " و " + "ﺖﻋﺎﺳ " + hourleft;
                }
            }
            else
            {
                // m_PowerVideoTxt.text = "ﻦﯿﺒﺑ ﻩﺎﺗﻮﮐ ﻮﯾﺪﯾﻭ";
                m_DailyOfferButton.interactable = true;
                imgGiftDailyOffer.color = Color.white;
                //m_DailyOfferButton.GetComponent<UiAnim_Loop_ScaleA>().enabled = true;
                imgTimebarDailyOffer.gameObject.SetActive(false);
                textCountDailyOffer.gameObject.SetActive(false);
                imgFlashDailyOffer.gameObject.SetActive(true);
            }

            Image imgTimebarFreeCoin = UISystem.FindImage(m_FreeCoinButton.gameObject, "img-time-bar-freecoin");
            Image imgFreeCoinVideo = UISystem.FindImage(m_FreeCoinButton.gameObject, "img-freecoin-video");
            Image imgFlashFreeCoin = UISystem.FindImage(m_FreeCoinButton.gameObject, "img-flash-freecoin");
            Text textTimeFreeCoin = UISystem.FindText(m_FreeCoinButton.gameObject, "txt-time-freecoin");

            if (m_DataStorage.VideoCountSeen >= 4)
            {


                if (TimeControl.m_Main.m_TimeList["time-free-coin-video"].m_ReachedEnd)
                {
                    //Debug.Log("ReachedEnd");
                    m_FreeCoinButton.interactable = true;
                    imgFreeCoinVideo.color = Color.white;
                    m_FreeCoinButton.GetComponent<UiAnim_Loop_ScaleA>().enabled = true;
                    imgTimebarFreeCoin.gameObject.SetActive(false);
                    textTimeFreeCoin.gameObject.SetActive(false);
                    imgFlashFreeCoin.gameObject.SetActive(true);
                    m_DataStorage.VideoCountSeen = 0;
                    TimeControl.m_Main.m_TimeList["time-free-coin-video"].Reset();

                }
                else
                {
                    int hourleft = (int)TimeControl.m_Main.m_TimeList["time-free-coin-video"].m_HoursCount - (TimeControl.m_Main.m_TimeList["time-free-coin-video"].m_DeltaTime.Hours) - 1;
                    int minuteleft = 60 - TimeControl.m_Main.m_TimeList["time-free-coin-video"].m_DeltaTime.Minutes;

                    imgTimebarFreeCoin.gameObject.SetActive(true);
                    textTimeFreeCoin.gameObject.SetActive(true);
                    m_FreeCoinButton.interactable = false;
                    imgFlashFreeCoin.gameObject.SetActive(false);
                    //m_FreeCoinButton.GetComponent<UiAnim_Loop_ScaleA>().enabled = false;
                    imgFreeCoinVideo.color = Color.gray;
                    textTimeFreeCoin.text = "ﻪﻘﯿﻗﺩ " + minuteleft + " و " + "ﺖﻋﺎﺳ " + hourleft;

                }
            }
            else
            {
                m_FreeCoinButton.interactable = true;
                imgFreeCoinVideo.color = Color.white;
                //m_FreeCoinButton.GetComponent<UiAnim_Loop_ScaleA>().enabled = true;
                imgTimebarFreeCoin.gameObject.SetActive(false);
                textTimeFreeCoin.gameObject.SetActive(false);
                imgFlashFreeCoin.gameObject.SetActive(true);


            }
        }






        public bool WatchUnlockRewardWheelVideo()
        {
            if (Application.platform != RuntimePlatform.Android)
            {
                HandleGetFreeRewardWheel();
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

                }
            }

            return true;
        }
        public bool WatchCoinReward()
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
                    msg.AutoCloseMessage(1);
                }
                else
                {

                }
            }
            return true;
        }
        public bool WatchUnlockRewardBoxVideo()
        {
            if (Application.platform != RuntimePlatform.Android)
            {
                HandleGetFreeRandomReward();
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

                }
            }
            return true;
        }

        public void BtnMultiplayer()
        {
            if (!m_DataStorage.CheckInternet())
            {
                UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 1, m_UITextContentsContents.m_Messages[0], m_UIGraphicContents.m_Graphics[4]);
            }
            else if (!m_IsCheckingMulti)
            {
                StartCoroutine(Co_BtnMulti());
            }


            // SoundGallery.PlaySound("Click (15)");

        }

        IEnumerator Co_BtnMulti()
        {
            m_IsCheckingMulti = true;
            m_MultiWait.gameObject.SetActive(true);


            {
                if (string.IsNullOrEmpty(m_PlayerData.m_PlayerEmail) && !m_AllowCreateRandomUser)
                {
                    m_AllowCreateRandomUser = true;

                }

                yield return new WaitForSeconds(1);

                m_GameplayData.m_GameMode = GameModes.Multiplayer;
                m_GameplayData.m_FieldNum = 0;
                UISystem.ShowUI("FindMatchUI");
                UISystem.RemoveUI(gameObject);
            }

            m_MultiWait.gameObject.SetActive(false);
            m_IsCheckingMulti = false;
        }
        public bool ShowCoinShop()
        {
            GameObject obj = UISystem.ShowUI("CoinShopUI");
            obj.GetComponentInChildren<UITabControl>().SelectTab(0);
            //UISystem.RemoveUI("MainMenuUI");
            return true;
        }


        public void BtnExit()
        {
            if (m_DataStorage.m_PlayerCommented)
            {
                Application.Quit();
            }

            //  SoundGallery.PlaySound("Click (15)");
        }



        public bool ExitRate_No()
        {
            Application.Quit();
            return true;
        }

        public void BtnMusicMute()
        {

            if (m_DataStorage.MuteMusic)
            {
                m_DataStorage.MuteMusic = false;
                m_MusicMuteImage.sprite = m_Music[0];
            }
            else
            {
                m_DataStorage.MuteMusic = true;
                m_MusicMuteImage.sprite = m_Music[1];
            }

            MusicPlayer.Current.HandleMute();
            m_DataStorage.SaveData();

        }

        public void Btn_PlayerInfo()
        {
            UISystem.ShowUI("PlayerInfoUI");
            UISystem.RemoveUI(gameObject);
            // SoundGallery.PlaySound("Click (15)");
        }
        public void Btn_RandomReward()
        {
            if (m_DataStorage.m_RandomReward > 0)
            {
                UISystem.ShowUI("RandomBoxUI");
                //UISystem.RemoveUI(gameObject);

            }
            else
            {
                if (TimeControl.m_Main.m_TimeList["time-random-reward-video"].m_ReachedEnd)
                {
                    UIMessage_B msg = UISystem.ShowMessage_B("UIMessage_B", 0, m_UITextContentsContents.m_Messages[25], null);
                    Image img = UISystem.FindImage(msg.gameObject, "MessageImageFade_2");
                    img.sprite = m_UIGraphicContents.m_Graphics[16];
                    img.gameObject.SetActive(true);
                    msg.f_Clicked_WatchVideoToUnlock = WatchUnlockRewardBoxVideo;
                }
            }
            // SoundGallery.PlaySound("Click (15)");
        }
        public void Btn_FreeCoin()
        {
            UIMessage_B msg = UISystem.ShowMessage_B("UIMessage_B", 0, m_UITextContentsContents.m_Messages[35], null);
            Image img = UISystem.FindImage(msg.gameObject, "MessageImageFade_2");
            img.sprite = m_UIGraphicContents.m_Graphics[11];
            img.gameObject.SetActive(true);
            msg.f_Clicked_WatchVideoToUnlock = WatchCoinReward;
            // SoundGallery.PlaySound("Click (15)");
        }

        public void Btn_RewardWheel()
        {
            if (m_DataStorage.m_RewardWheelCount > 0)
            {
                UISystem.ShowUI("RewardWheelUI");
                //UISystem.RemoveUI(gameObject);

            }
            else
            {
                if (TimeControl.m_Main.m_TimeList["time-reward-wheel-video"].m_ReachedEnd)
                {
                    UIMessage_B msg = UISystem.ShowMessage_B("UIMessage_B", 0, m_UITextContentsContents.m_Messages[26], null);
                    Image img = UISystem.FindImage(msg.gameObject, "MessageImageFade_1");
                    img.sprite = m_UIGraphicContents.m_Graphics[1];
                    img.gameObject.SetActive(true);
                    msg.f_Clicked_WatchVideoToUnlock = WatchUnlockRewardWheelVideo;
                }
            }
            // SoundGallery.PlaySound("Click (15)");
        }




        public void HandleGetFreeRewardWheel()
        {
            m_DataStorage.m_RewardWheelCount++;
            UISystem.ShowUI("RewardWheelUI");

            TimeControl.m_Main.m_TimeInstances[1].Reset();
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
        }
        public void HandleGetFreeRandomReward()
        {
            m_DataStorage.m_RandomReward++;
            UISystem.ShowUI("RandomBoxUI");
            TimeControl.m_Main.m_TimeInstances[0].Reset();
        }

        public void BtnStore()
        {
            GameObject obj = UISystem.ShowUI("CoinShopUI");
            obj.GetComponentInChildren<UITabControl>().SelectTab(2);
            // UISystem.RemoveUI("MainMenuUI");
            // SoundGallery.PlaySound("Click (15)");
        }
        public void BtnOfflinePractice()
        {

            m_GameplayData.m_GameMode = GameModes.Singleplayer;
            m_GameplayData.m_MatchMode = MatchModes.Friendly;
            m_GameplayData.m_MyTeam = 0;


            SceneManager.LoadScene("ExplorationScene");
            //UISystem.ShowUI("SquadUI");
        }


        public void BtnTutorial()
        {
            m_GameplayData.m_FieldNum = m_Contents.m_Tutorial.m_LevelTheme;
            m_GameplayData.m_GameMode = GameModes.Tutorial;
            m_GameplayData.m_MatchMode = MatchModes.Friendly;
            m_GameplayData.m_MyTeam = 0;
            SceneManager.LoadScene("FightScene-1");
            //UISystem.ShowUI("SquadUI");
        }


        public void BtnPlayerSquad()
        {
            UISystem.RemoveUI(gameObject);
            UISystem.ShowUI("SquadUI");
            // SoundGallery.PlaySound("Click (15)");

        }
        public void Btn_LeaderBoard()
        {
            if (!m_DataStorage.CheckInternet())
            {
                UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 1, m_UITextContentsContents.m_Messages[0], m_UIGraphicContents.m_Graphics[4]);
                msg.AutoCloseMessage(1);
            }
            else
            {
                UISystem.ShowUI("PlayerRankingUI");
            }
            // SoundGallery.PlaySound("Click (15)");
        }
        public void BtnDailyOffer()
        {
            UISystem.ShowUI("DailyOfferUI");
            // SoundGallery.PlaySound("Click (15)");
        }



        public void BtnAchievements()
        {
            UISystem.ShowUI("AchievementsUI");
            UISystem.RemoveUI("MainMenu");
        }


        public bool CloseUpdateMenu()
        {
            return true;
        }


        public void BtnMenusPanel()
        {
            m_MenusPanel.SetActive(true);
        }

        public void BtnClosePanel()
        {
            m_MenusPanel.SetActive(false);
        }

        public void BtnProgressionUI()
        {
            UISystem.ShowUI("ProgressionUI");
            UISystem.RemoveUI("MainMenu");
        }
    }


}
