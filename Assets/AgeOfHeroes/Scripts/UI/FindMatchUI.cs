using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AgeOfHeroes.ScriptableObjects;
using TMPro;
using System.Linq;
namespace AgeOfHeroes.UI
{
    public class FindMatchUI : MonoBehaviour
    {
        [TextAreaAttribute]
        public string[] str_MessageUI;
        public Image[] m_PlayerLogos;
        public Text[] m_PlayerNames;
        public Image[] m_PlayerSquadsBase;
        public Image[] m_CharacterImages;
        public Image m_LoadingImage;
        public Text m_FindingText;
        public Image m_FriendPanel;
        public Image m_InvitedPanel;
        public Text m_UserId;

        public Image m_StageImage;
        public Image m_StageLock;
        public GameObject m_StagePanel;
        public Button m_ChooseStage;
        public Button[] m_Arrows;
        public Button m_OpenStagePanel;

        public Text m_InvitingName;

        public TMP_InputField m_NameInput;

        [SerializeField, Space]
        private PlayerData m_PlayerData;

        [SerializeField]
        private GameplayData m_GameplayData;
        [SerializeField]
        private DataStorage m_DataStorage;
        [SerializeField]
        private Contents m_Content;
        [SerializeField]
        private OpponentData m_OpponentData;
        [SerializeField, Space]
        private UITextContents m_UITextContentsContents;
        [SerializeField, Space]
        private UIGraphicContents m_UIGraphicContents;

        // Start is called before the first frame update
        void Start()
        {
            m_PlayerLogos[1].gameObject.SetActive(false);
            m_PlayerNames[1].gameObject.SetActive(false);
            m_PlayerSquadsBase[1].gameObject.SetActive(false);
            m_LoadingImage.gameObject.SetActive(false);
            m_FindingText.gameObject.SetActive(false);
            m_FriendPanel.gameObject.SetActive(false);
            m_InvitedPanel.gameObject.SetActive(false);

            m_PlayerNames[0].text = m_PlayerData.m_PlayerName;
            m_PlayerLogos[0].sprite = m_PlayerData.m_PlayerAvatarSprite;

            for (int i = 0; i < 3; i++)
            {
                Character c = m_Content.m_Characters[m_DataStorage.m_CharactersNumber[i]];
                m_CharacterImages[i].sprite = c.m_Skins[c.m_SkinNum].m_Icon;
            }

        }

        // Update is called once per frame
        void Update()
        {
            if (m_Content.m_Stages[m_GameplayData.m_FieldNum].m_Unlocked)
            {
                m_OpenStagePanel.image.sprite = m_Content.m_Stages[m_GameplayData.m_FieldNum].m_Icon;
            }


            if (m_Content.m_Stages[m_GameplayData.m_FieldNum].m_Unlocked)
            {
                m_StageLock.gameObject.SetActive(false);
            }
            else
            {
                m_StageLock.gameObject.SetActive(true);
            }

            if (m_GameplayData.m_FieldNum == 0)
            {
                m_Arrows[0].gameObject.SetActive(false);
            }
            else
            {
                m_Arrows[0].gameObject.SetActive(true);
            }

            if (m_GameplayData.m_FieldNum == m_Content.m_Stages.Length - 1)
            {
                m_Arrows[1].gameObject.SetActive(false);
            }
            else
            {
                m_Arrows[1].gameObject.SetActive(true);
            }

            m_StageImage.sprite = m_Content.m_Stages[m_GameplayData.m_FieldNum].m_Icon;
        }
        void OnDestroy()
        {
        }

        public void FoundOpponent()
        {

            m_PlayerLogos[1].gameObject.SetActive(true);
            m_PlayerNames[1].gameObject.SetActive(true);
            m_PlayerSquadsBase[1].gameObject.SetActive(true);

            for (int i = 0; i < 3; i++)
            {
                Character c = m_Content.m_Characters[m_OpponentData.m_CharactersNumber[i]];
                m_CharacterImages[3 + i].sprite = c.m_Skins[c.m_SkinNum].m_Icon;
            }

            m_PlayerNames[1].text = m_OpponentData.m_PlayerName;
            m_PlayerLogos[1].sprite = m_OpponentData.m_PlayerAvatarSprite;

            m_LoadingImage.gameObject.SetActive(false);
            m_FindingText.gameObject.SetActive(false);
            m_FriendPanel.gameObject.SetActive(false);
            m_InvitedPanel.gameObject.SetActive(false);
        }

        public void BtnStartSearch()
        {
            if (m_DataStorage.PowerCounts.All(count => count == 0))
            {
                UIMessage_B msg = UISystem.ShowMessage_B("UIMessage_B", 6, m_UITextContentsContents.m_Messages[49], null);
                Image img = UISystem.FindImage(msg.gameObject, "MessageImageFade_2");
                img.sprite = m_UIGraphicContents.m_Graphics[Random.Range(18, 22)];
                img.gameObject.SetActive(true);
                msg.f_Clicked_WatchVideoToUnlock = WatchPowerReward;
                msg.f_Clicked_No = StartOnlineSearch;
                SoundGallery.PlaySound("pop1");
            }

            else
            {
                StartOnlineSearch();
            }

        }
        public void BtnExit()
        {

            UISystem.ShowUI("MainMenuUI");
            UISystem.RemoveUI(gameObject);
            SoundGallery.PlaySound("pop1");
        }
        public void BtnPlayWithFriend()
        {
            m_UserId.text = m_PlayerData.m_UserID.ToString();
            m_FriendPanel.gameObject.SetActive(true);
            SoundGallery.PlaySound("pop1");
        }
        public void BtnExitFriendPanel()
        {
            m_FriendPanel.gameObject.SetActive(false);
            SoundGallery.PlaySound("pop1");
        }
        public void BtnShareGame()
        {
            string shareMessage = "";



            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>(
                "setAction",
                intentClass.GetStatic<string>("ACTION_SEND")
            );
            intentObject.Call<AndroidJavaObject>("setType", "text/plain");
            intentObject.Call<AndroidJavaObject>(
                "putExtra",
                intentClass.GetStatic<string>("EXTRA_SUBJECT"),
                "SUBJECT"
            );
            intentObject.Call<AndroidJavaObject>(
                "putExtra",
                intentClass.GetStatic<string>("EXTRA_TEXT"),
                shareMessage
            );
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>(
                "currentActivity"
            );
            currentActivity.Call("startActivity", intentObject);
        }
        public void BtnInvitePlayer()
        {

            SoundGallery.PlaySound("button32");

        }

        public void HandleInvited(string playerName)
        {
            m_InvitedPanel.gameObject.SetActive(true);
            m_InvitingName.text = playerName;
        }

        public void Btn_AcceptInvite()
        {
            m_InvitedPanel.gameObject.SetActive(false);

            SoundGallery.PlaySound("pop1");
        }

        public void Btn_CancelInvite()
        {
            m_InvitedPanel.gameObject.SetActive(false);
            SoundGallery.PlaySound("pop1");
        }
        public bool WatchPowerReward()
        {

            if (Application.platform != RuntimePlatform.Android)
            {
                HandleGetPowerReward();
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
        public bool StartOnlineSearch()
        {
            if (!m_DataStorage.CheckInternet())
            {
                UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 1, m_UITextContentsContents.m_Messages[0], m_UIGraphicContents.m_Graphics[4]);
                msg.AutoCloseMessage(1);
            }
            else
            {

                m_LoadingImage.gameObject.SetActive(true);
                m_FindingText.gameObject.SetActive(true);
            }

            return true;
        }
        void HandleGetPowerReward()
        {
            int randomPower = Random.Range(0, 3);
            m_DataStorage.PowerCounts[randomPower]++;
            m_DataStorage.SaveData();
            //GameObject obj1 = UISystem.FindOpenUIByName("FindMatchUI");
            //obj1.GetComponentInChildren<PowerSelectPanel>().BtnPower(randomPower);
            Invoke("StartOnlineSearch", 1);
        }

        public void BtnStage()
        {
            m_StagePanel.gameObject.SetActive(true);
        }

        public void BtnLArrow()
        {
            if (m_GameplayData.m_FieldNum > 0)
            {
                m_GameplayData.m_FieldNum--;
            }
        }

        public void BtnRArrow()
        {
            if (m_GameplayData.m_FieldNum < m_Content.m_Stages.Length - 1)
            {
                m_GameplayData.m_FieldNum++;
            }
        }

        public void BtnChooseStage()
        {
            if (m_Content.m_Stages[m_GameplayData.m_FieldNum].m_Unlocked)
            {
                m_StagePanel.gameObject.SetActive(false);
            }
        }

    }
}
