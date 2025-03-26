using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using AgeOfHeroes.ScriptableObjects;
using AgeOfHeroes.Gameplay;
using System;
using TMPro;

namespace AgeOfHeroes.UI
{
    public class LevelEndUI : MonoBehaviour
    {
        [HideInInspector]
        public int m_CoinNum;

        public GameObject m_CoinPanel;

        [SerializeField]
        private DataStorage m_DataStorage;
        [SerializeField]
        private GameplayData m_GameplayData;
        [SerializeField]
        private Contents m_Contents;
        [SerializeField]
        private OpponentData m_OpponentData;
        [SerializeField]
        private PlayerData m_PlayerData;
        [SerializeField, Space]
        private UIGraphicContents m_UIGraphicContents;
        [SerializeField, Space]
        private UITextContents m_UITextContentsContents;


        // Start is called before the first frame update
        void Start()
        {
            Team[] teams = GameControl.m_Current.m_Teams;
            // MusicPlayer.Current.StopMusic();
            if (GameControl.m_Current.m_FirstTime)
            {
                m_CoinPanel.gameObject.SetActive(true);
                XPBarAdd.Current.EarnXP(10);
                m_CoinNum = 100;
                m_DataStorage.m_WinCount++;
                m_Contents.m_Achievements[3].AddCount();
                m_Contents.m_Achievements[9].AddCount();
                m_DataStorage.Coin += m_CoinNum;
                m_DataStorage.SaveData();
            }
            else
            {
                m_CoinPanel.gameObject.SetActive(false);
            }



            if (m_DataStorage.CheckInternet() && m_DataStorage.m_NotFirstTimeInGame)
            {
                if (!m_DataStorage.m_RemoveAds)
                {
                    if (m_DataStorage.m_LastLevelNum % 5 == 0 && !m_DataStorage.m_PlayerCommented)
                    {
                        Invoke("ShowRateUI", 2);
                    }
                    else
                    {
                        if (m_DataStorage.m_LastLevelNum % 2 != 0)
                        {
                            Invoke("ShowBannerAd", 2);
                        }
                        else if (m_DataStorage.m_LastLevelNum % 2 == 0)
                        {
                            Invoke("ShowVideoAd", 2);
                        }
                    }
                }
                else if (!m_DataStorage.m_PlayerCommented)
                {
                    Invoke("ShowRateUI", 2);
                }

            }




        }

        // Update is called once per frame
        void Update()
        {

        }


        public void BtnFreeCoin()
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
                    HandleGetFreeCoin();
                }

            }
            SoundGallery.PlaySound("button32");
        }

        public void HandleGetFreeCoin()
        {
            m_GameplayData.m_CheckReward = true;
            m_DataStorage.VideoCountSeen++;
            if (m_DataStorage.VideoCountSeen >= 4)
            {
                TimeControl.m_Main.m_TimeInstances[2].Reset();
            }

            m_DataStorage.Coin += 100;
            UISystem.ShowCoinReward(100);
            m_DataStorage.SaveData();
        }

        public void BtnContinue()
        {
            if (!m_DataStorage.m_NotFirstTimeInGame || m_GameplayData.m_GameMode == GameModes.Tutorial)
            {
                m_DataStorage.m_NotFirstTimeInGame = true;
                m_DataStorage.SaveData();
                Invoke("LoadMainScene", 1);
            }
            else if (m_DataStorage.CheckInternet() && !m_DataStorage.m_RemoveAds)
            {

                Invoke("LoadNextScene", 1);
            }
            else
            {
                LoadNextScene();
            }

            SoundGallery.PlaySound("button32");
        }

        private void LoadNextScene()
        {

            SceneManager.LoadScene("ExplorationScene");
        }

        public void BtnRestart()
        {
            if (!m_DataStorage.m_NotFirstTimeInGame)
            {
                m_DataStorage.m_NotFirstTimeInGame = true;
                m_DataStorage.SaveData();
                Invoke("RestartScene", 1);
            }
            else if (m_DataStorage.CheckInternet() && !m_DataStorage.m_RemoveAds)
            {

                Invoke("RestartScene", 1);
            }
            else
            {
                RestartScene();
            }
            SoundGallery.PlaySound("button32");

        }
        private void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        private void LoadMainScene()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}