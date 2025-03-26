using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AgeOfHeroes.ScriptableObjects;
using System.Threading.Tasks;

namespace AgeOfHeroes.UI
{
    public class PlayerAvatarUI : MonoBehaviour
    {

        public Transform m_ListPanel;
        public Image m_WaitingPanel;
        [HideInInspector]
        public ItemListButton[] m_Buttons;
        public RenderTexture renderTexture;
        [HideInInspector]
        public string outputFilePath = "";
        [SerializeField, Space]
        private DataStorage m_DataStorage;
        [SerializeField, Space]
        private Contents m_Contents;
        [SerializeField]
        private PlayerData m_PlayerData;
        [SerializeField, Space]
        private UIGraphicContents m_UIGraphicContents;
        [SerializeField, Space]
        private UITextContents m_UITextContentsContents;

        public Image m_TestImage;

        // Start is called before the first frame update
        void Start()
        {
            m_WaitingPanel.gameObject.SetActive(false);
            outputFilePath = Application.dataPath + "/Art/GeneratedAvatars/GeneratedAvatar_1.png";



            m_Buttons = new ItemListButton[200];
            for (int i = 0; i < m_Contents.m_AvatarPartList.Length; i++)
            {
                for (int j = 0; j < m_Contents.m_AvatarPartList[i].m_Parts.Length; j++)
                {
                    AvatarPart part = m_Contents.m_AvatarPartList[i].m_Parts[j];
                    GameObject btn = Instantiate(UISystem.m_Main.m_UIData.m_UIElementsPrefabs[0]);
                    m_Buttons[i] = btn.GetComponent<ItemListButton>();
                    m_Buttons[i].transform.SetParent(m_ListPanel);
                    m_Buttons[i].m_FarsiTitles.text = part.m_TitleEnglish;
                    m_Buttons[i].m_ItemNum = i;
                    m_Buttons[i].m_ItemNum2 = j;
                    m_Buttons[i].f_Clicked = SelectPart;
                    Image img1 = UISystem.FindImage(m_Buttons[i].gameObject, "img-icon");
                    if (part.m_PartSprite == null)
                    {
                        img1.sprite = part.m_Icon;
                    }
                    else
                    {
                        img1.sprite = part.m_PartSprite;
                    }
                    img1 = UISystem.FindImage(m_Buttons[i].gameObject, "img-lock");
                    img1.gameObject.SetActive(false);
                    //img1 = UISystem.FindImage(m_Buttons[i].gameObject, "img-titleback");
                    //img1.gameObject.SetActive(false);

                }
                //img1.sprite = 
            }
        }
        // Update is called once per frame
        void Update()
        {

        }

        public void HandleDataInserted()
        {
            UIMessage_A msg = UISystem.ShowMessage(
                "UIMessage_A",
                4,
                m_UITextContentsContents.m_Messages[22],
                m_UIGraphicContents.m_Graphics[3]
            );
            msg.f_Clicked_OK = MessageOkClicked;

        }
        public bool MessageOkClicked()
        {
            UISystem.RemoveUI("PlayerAvatarUI");
            XPBar.m_Current.UpdateInfo();
            //UISystem.ShowUI("MainMenu");
            // MainMenuTabsUI.m_Main.m_CurrentTabObject.GetComponentInChildren<XPBar>().UpdateInfo();
            // MainMenuTabsUI.m_Main.SelectTab(0);
            return true;
        }


        public bool SelectPart(int part, int num)
        {
            AvatarSystem.m_Main.SetSprite(part, num);

            return true;
        }
        public void Btn_SaveImage()
        {
            if (!m_DataStorage.CheckInternet())
            {
                UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 1, m_UITextContentsContents.m_Messages[0], m_UIGraphicContents.m_Graphics[4]);
                msg.AutoCloseMessage(1);
            }
            else
            {
                StartCoroutine(Co_HandleUpdateAvatar());
            }

            SoundGallery.PlaySound("pop1");
        }
        IEnumerator Co_HandleUpdateAvatar()
        {
            //m_WaitingPanel.gameObject.SetActive(true);

            Sprite avatarSprite = AvatarSystem.m_Main.GenerateAvatar();
            AvatarSystem.m_Main.m_GeneratedAvatar = avatarSprite;
            m_PlayerData.m_PlayerAvatarSprite = avatarSprite;
            m_PlayerData.m_PlayerImageNum = -1;
            m_PlayerData.SaveData();
            yield return new WaitForSeconds(.1f);

            Debug.Log("Render Texture converted to PNG and saved at: " + outputFilePath);
            //yield return new WaitForSeconds(.1f);            


            //m_WaitingPanel.gameObject.SetActive(false);

            UIMessage_B msg = UISystem.ShowMessage_B("UIMessage_B", 5, m_UITextContentsContents.m_Messages[12], null);
            Image img = UISystem.FindImage(msg.gameObject, "MessageImage");
            img.sprite = m_PlayerData.m_PlayerAvatarSprite;
            img.gameObject.SetActive(true);
            msg.f_Clicked_OK = MessageAvatarOkClicked;
            //yield return new WaitForSeconds(2f);

        }

        public bool MessageAvatarOkClicked()
        {
            SoundGallery.PlaySound("pop1");
            return true;
        }

        public void BtnBack()
        {
            UISystem.RemoveUI("PlayerAvatarUI");
            //            MainMenuTabsUI.m_Main.SelectTab(0);
            SoundGallery.PlaySound("pop1");

        }
    }
}