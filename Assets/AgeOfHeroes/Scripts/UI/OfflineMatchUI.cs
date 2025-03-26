using AgeOfHeroes.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AgeOfHeroes.UI
{
    public class OfflineMatchUI : MonoBehaviour
    {
        [TextAreaAttribute]
        public string[] str_MessageUI;
        public Image[] m_PlayerLogos;
        public Text[] m_PlayerNames;
        public Image[] m_PlayerSquads;



        [SerializeField, Space]
        private PlayerData m_PlayerData;

        [SerializeField]
        private GameplayData m_GameplayData;
        [SerializeField]
        private DataStorage m_DataStorage;

        [SerializeField]
        private OpponentData m_OpponentData;
        [SerializeField, Space]
        private UIGraphicContents m_UIGraphicContents;
        [SerializeField, Space]
        private UITextContents m_UITextContentsContents;

        // Start is called before the first frame update
        void Start()
        {
            m_PlayerLogos[1].gameObject.SetActive(false);
            m_PlayerNames[1].gameObject.SetActive(false);
            m_PlayerSquads[1].gameObject.SetActive(false);

            m_PlayerNames[0].text = m_PlayerData.m_PlayerName;
            m_PlayerLogos[0].sprite = m_PlayerData.m_PlayerAvatarSprite;

            FoundOpponent();
        }


        // Update is called once per frame
        void Update()
        {

        }

        public void FoundOpponent()
        {
            m_PlayerLogos[1].gameObject.SetActive(true);
            m_PlayerNames[1].gameObject.SetActive(true);
            m_PlayerSquads[1].gameObject.SetActive(true);

            m_PlayerNames[1].text = m_OpponentData.m_PlayerName;
            m_PlayerLogos[1].sprite = m_OpponentData.m_PlayerAvatarSprite;
        }

        public void BtnStartMatch()
        {
            if (m_DataStorage.PowerCounts.All(count => count == 0))
            {
                UIMessage_B msg = UISystem.ShowMessage_B("UIMessage_B", 6, m_UITextContentsContents.m_Messages[49], null);
                Image img = UISystem.FindImage(msg.gameObject, "MessageImageFade_2");
                img.sprite = m_UIGraphicContents.m_Graphics[Random.Range(33, 36)];
                img.gameObject.SetActive(true);
                msg.f_Clicked_WatchVideoToUnlock = WatchPowerReward;
                msg.f_Clicked_No = StartMatch;
                SoundGallery.PlaySound("pop1");
            }

            else if (!m_DataStorage.PowerCounts.All(count => count == 0))
            {
                UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 1, m_UITextContentsContents.m_Messages[53], null);
                Image img = UISystem.FindImage(msg.gameObject, "MessageImage");
                img.sprite = m_UIGraphicContents.m_Graphics[Random.Range(33, 36)];
                img.gameObject.SetActive(true);
            }
            else
            {
                StartMatch();
            }


            SoundGallery.PlaySound("pop1");
        }

        public bool StartMatch()
        {
            //m_GameplayData.m_FieldNum = m_DataStorage.m_FieldNumber;

            SceneManager.LoadScene("Soccer_Scene_1");
            return true;
        }
        public bool WatchPowerReward()
        {
            if (Application.platform != RuntimePlatform.Android)
            {
                HandleGetPowerReward();
            }

            return true;
        }
        void HandleGetPowerReward()
        {
            int randomPower = Random.Range(0, 3);
            m_DataStorage.PowerCounts[randomPower]++;
            m_DataStorage.SaveData();
            GameObject obj1 = UISystem.FindOpenUIByName("OfflineMatchUI");
            obj1.GetComponentInChildren<PowerSelectPanel>().BtnPower(randomPower);
            Invoke("StartMatch", 1);
        }
        public void BtnExit()
        {
            if (m_GameplayData.m_MatchMode == MatchModes.OfflineCup)
            {
                UISystem.ShowUI("OfflineCupUI");
                UISystem.RemoveUI(gameObject);
            }
            else if (m_GameplayData.m_MatchMode == MatchModes.AsiaCup)
            {
                UISystem.ShowUI("AsiaCupUI");
                UISystem.RemoveUI(gameObject);
            }
            else if (m_GameplayData.m_MatchMode == MatchModes.IranCup)
            {
                UISystem.ShowUI("IranCupUI");
                UISystem.RemoveUI(gameObject);
            }
            else if (m_GameplayData.m_MatchMode == MatchModes.EnglishCup)
            {
                UISystem.ShowUI("EnglishCupUI");
                UISystem.RemoveUI(gameObject);
            }
            else if (m_GameplayData.m_MatchMode == MatchModes.Friendly)
            {
                UISystem.ShowUI("MainMenuUI");
                UISystem.RemoveUI(gameObject);
            }

            SoundGallery.PlaySound("pop1");
        }

    }
}