using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AgeOfHeroes.UI;
using AgeOfHeroes.ScriptableObjects;
using TMPro;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace AgeOfHeroes.UI
{
    public class PlayerInfoUI : MonoBehaviour
    {
        public Image[] m_IconImages;
        public Image m_IconPanel;
        public Image m_RenamePanel,
            ChangeEmailPanel;
        public TMP_InputField m_NameInput;
        public TMP_InputField[] m_SignupInput;

        public Image m_WaitingPanel;

        public Image m_PlayerImage;
        public Text m_PlayerName;
        public Text m_PlayerLevel;
        public Text m_PlayerEmail;
        public Image m_UploadButton;

        [SerializeField, Space]
        private Contents m_Contents;

        [SerializeField, Space]
        private DataStorage m_DataStorage;

        [SerializeField, Space]
        private PlayerData m_PlayerData;

        [SerializeField, Space]
        private UIGraphicContents m_UIGraphicContents;

        [SerializeField, Space]
        private UITextContents m_UITextContentsContents;
        public static PlayerInfoUI m_Current;
        void Awake()
        {
            m_Current = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            m_IconPanel.gameObject.SetActive(false);
            m_RenamePanel.gameObject.SetActive(false);
            ChangeEmailPanel.gameObject.SetActive(false);
            m_WaitingPanel.gameObject.SetActive(false);
            UpdateInfo();

        }
        void OnDestroy()
        {
        }
        // Update is called once per frame
        void Update()
        {
            // if (m_PlayerData.m_PlayerEmail != "" && m_PlayerData.m_PlayerPassword != "")
            // {
            //     m_UploadButton.gameObject.SetActive(true);
            // }
            // else
            // {
            //     m_UploadButton.gameObject.SetActive(false);
            // }
        }

        public void UpdateInfo()
        {
            m_PlayerName.text = m_PlayerData.m_PlayerName;
            m_PlayerEmail.text = m_PlayerData.m_PlayerEmail;
            m_PlayerLevel.text = m_PlayerData.m_PlayerLevel.ToString();
            m_PlayerImage.sprite = m_PlayerData.m_PlayerAvatarSprite;
            m_PlayerEmail.text = m_PlayerData.m_PlayerEmail;
        }

        public void Btn_ShowLoginPanel()
        {
            UISystem.ShowUI("PlayerAccountUI");
            UISystem.RemoveUI(gameObject);
            SoundGallery.PlaySound("button32");
        }

        public void Btn_ShowRenamePanel()
        {
            SoundGallery.PlaySound("button32");
            m_RenamePanel.gameObject.SetActive(true);
        }

        public void Btn_ShowChangeEmailPanel()
        {
            SoundGallery.PlaySound("button32");
            ChangeEmailPanel.gameObject.SetActive(true);
        }

        public void Btn_CloseRenamePanel()
        {
            SoundGallery.PlaySound("button32");
            m_RenamePanel.gameObject.SetActive(false);
            ChangeEmailPanel.gameObject.SetActive(false);
        }

        public void Btn_ShowIconPanel()
        {
            SoundGallery.PlaySound("button32");
            m_IconPanel.gameObject.SetActive(true);
        }

        public void Btn_CloseIconPanel()
        {
            m_IconPanel.gameObject.SetActive(false);
            SoundGallery.PlaySound("button32");
        }

        public void Btn_CloseNamePanel()
        {
            m_RenamePanel.gameObject.SetActive(false);
            SoundGallery.PlaySound("button32");
        }

        public async void Btn_Register()
        {
            if (!m_DataStorage.CheckInternet())
            {
                UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 1, m_UITextContentsContents.m_Messages[0], m_UIGraphicContents.m_Graphics[4]);
                msg.AutoCloseMessage(1);
            }
            else
            {
                bool invalidNameInput = false;
                for (int i = 0; i < m_SignupInput.Length; i++)
                {
                    if (
                        string.IsNullOrEmpty(m_SignupInput[i].text)
                        || string.IsNullOrWhiteSpace(m_SignupInput[i].text)

                    )
                    {
                        invalidNameInput = true;
                        break;
                    }
                }

                if (invalidNameInput)
                {
                    UIMessage_A msg = UISystem.ShowMessage(
                        "UIMessage_A",
                        1,
                        m_UITextContentsContents.m_Messages[23],
                        m_UIGraphicContents.m_Graphics[5]
                    );
                }
                else
                {
                    m_WaitingPanel.gameObject.SetActive(true);
                    //bool check = await DbControl.m_Current.CheckRepeatedEmail(m_SignupInput[0].text);



                    //if (!check)
                    //{
                    //    Async_UpdateEmailPassword();
                    //}
                    //else
                    //{
                    //    UIMessage_A msg = UISystem.ShowMessage(
                    //        "UIMessage_A",
                    //        1,
                    //        m_UITextContentsContents.m_Messages[24],
                    //        m_UIGraphicContents.m_Graphics[5]
                    //    );
                    //}
                }
            }
            SoundGallery.PlaySound("button32");
        }



        public void HandleDataInserted()
        {
            UIMessage_A msg = UISystem.ShowMessage(
                "UIMessage_A",
                1,
                m_UITextContentsContents.m_Messages[22],
                m_UIGraphicContents.m_Graphics[5]
            );
            msg.f_Clicked_OK = MessageOkClicked;

            UpdateInfo();

            m_RenamePanel.gameObject.SetActive(false);
            ChangeEmailPanel.gameObject.SetActive(false);
        }


        public bool MessageOkClicked()
        {
            UISystem.RemoveUI("PlayerInfoUI");
            SoundGallery.PlaySound("button32"); ;
            return true;
        }



        public void Btn_ApplyName()
        {
            if (!m_DataStorage.CheckInternet())
            {
                UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 1, m_UITextContentsContents.m_Messages[0], m_UIGraphicContents.m_Graphics[4]);
                msg.AutoCloseMessage(1);
            }
            else
            {
                bool invalidNameInput = false;

                if (
                    string.IsNullOrEmpty(m_NameInput.text)
                    || string.IsNullOrWhiteSpace(m_NameInput.text)
                )
                {
                    invalidNameInput = true;
                }

                if (invalidNameInput)
                {
                    UIMessage_A msg = UISystem.ShowMessage(
                        "UIMessage_A",
                        1,
                        m_UITextContentsContents.m_Messages[23],
                        m_UIGraphicContents.m_Graphics[5]
                    );
                }
                else
                {
                    m_PlayerData.m_PlayerName = m_NameInput.text;
                    m_PlayerName.text = m_NameInput.text;
                    XPBar.m_Current.UpdateInfo();

                    m_WaitingPanel.gameObject.SetActive(false);
                    m_RenamePanel.gameObject.SetActive(false);
                }
            }

            SoundGallery.PlaySound("button32");
        }




        public void Btn_SelectIcon(int num)
        {
            if (!m_DataStorage.CheckInternet())
            {
                UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 1, m_UITextContentsContents.m_Messages[0], m_UIGraphicContents.m_Graphics[4]);
                msg.AutoCloseMessage(1);
            }
            else
            {
                StartCoroutine(Co_HandleUpdateIcon(num));
            }

            SoundGallery.PlaySound("button32");
        }
        IEnumerator Co_HandleUpdateIcon(int num)
        {
            m_PlayerData.m_PlayerImageNum = num;
            m_PlayerData.m_PlayerAvatarSprite = m_Contents.m_PlayerAvatars[num];
            m_PlayerData.SaveData();

            UIMessage_B msg = UISystem.ShowMessage_B("UIMessage_B", 5, m_UITextContentsContents.m_Messages[29], null);
            Image img = UISystem.FindImage(msg.gameObject, "MessageImage");
            img.sprite = m_PlayerData.m_PlayerAvatarSprite;
            img.gameObject.SetActive(true);
            msg.f_Clicked_OK = MessageIconOkClicked;
            yield return new WaitForSeconds(.1f);
        }
        public bool MessageIconOkClicked()
        {
            Async_UpdateIconInDatabase();
            SoundGallery.PlaySound("pop1");
            return true;
        }
        async void Async_UpdateIconInDatabase()
        {
            m_WaitingPanel.gameObject.SetActive(true);
            await Task.Delay(1000);
            //await DbControl.m_Current.UpdateIconInDatabaseAsync(m_PlayerData.m_PlayerEmail, m_PlayerData.m_PlayerImageNum);
            UpdateInfo();
            MainMenuTabsUI.m_Main.m_CurrentTabObject.GetComponentInChildren<XPBar>().UpdateInfo();
            m_WaitingPanel.gameObject.SetActive(false);
            m_IconPanel.gameObject.SetActive(false);
            m_WaitingPanel.gameObject.SetActive(false);
            UISystem.RemoveUI(gameObject);
        }

        public void BtnBack()
        {
            UISystem.RemoveUI(gameObject);
            SoundGallery.PlaySound("button32");
        }

        public void Btn_CustomizeAvatar()
        {
            if (m_DataStorage.m_CustomizeProfilePic)
            {
                UISystem.ShowUI("PlayerAvatarUI");
                UISystem.RemoveUI(gameObject);
            }
            else
            {
                UIMessage_B msg = UISystem.ShowMessage_B(
                    "UIMessage_B",
                    4,
                    m_UITextContentsContents.m_Messages[13],
                    m_UIGraphicContents.m_Graphics[9]
                );
                Image img = UISystem.FindImage(msg.gameObject, "MessageImage");
                //ImageManager.m_Current.GetAvatarImageFromSource();
                img.sprite = m_UIGraphicContents.m_Graphics[9];
                img.gameObject.SetActive(true);
                msg.f_Clicked_Buy = MessageBuyClicked;
            }

            SoundGallery.PlaySound("button32");
        }

        public bool MessageBuyClicked()
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
                    m_DataStorage.m_CustomizeProfilePic = true;
                    m_DataStorage.SaveData();
                    UISystem.ShowUI("PlayerAvatarUI");
                    UISystem.RemoveUI(gameObject);
                }
                else
                {
#if UNITY_ANDROID


#endif
                }
            }
            return true;
        }

        public void BtnUploadData()
        {
            if (!m_DataStorage.CheckInternet())
            {
                UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 1, m_UITextContentsContents.m_Messages[0], m_UIGraphicContents.m_Graphics[4]);
                msg.AutoCloseMessage(1);
            }
            else
            {
                //Async_UploadData();

            }
            SoundGallery.PlaySound("button32");
        }


    }
}
