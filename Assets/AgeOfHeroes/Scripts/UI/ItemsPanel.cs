using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AgeOfHeroes.ScriptableObjects;

namespace AgeOfHeroes.UI
{
    public class ItemsPanel : MonoBehaviour
    {
        // Start is called before the first frame update


        [SerializeField, Space]
        private Contents m_Contents;
        [SerializeField, Space]
        private DataStorage m_DataStorage;
        [SerializeField, Space]
        private PlayerData m_PlayerData;
        [SerializeField]
        private GameplayData m_GameplayData;



        public Transform Defence_Items_Parent;
        public GameObject ItemBtnPrefab1;
        public GameObject m_MainItemPanel;
        public Image m_MainItemPanelImage;


        public Image m_BuyBtn;


        public Text m_MainItemPanelName;
        public Text m_MainItemPanelInfo;
        public Text m_MainItemLevelText;
        public Text m_AttackTitleText;
        public Text m_ItemPriceText;
        public Text m_ItemLevelText;
        public Text m_ItemTypeText;

        public Image m_ItemTypeIcon;
        public Image m_ItemSelectBack;
        public Image[] m_ItemButtons;


        public string[] m_ItemTypeStrings;
        public Sprite[] m_ItemTypeSprites;

        public GameObject m_UpgradeParticle;
        //public Button upgradebtn;
        public int m_ItemNum;
        [SerializeField, Space]
        private UIGraphicContents m_UIGraphicContents;
        [SerializeField, Space]
        private UITextContents m_UITextContentsContents;
        public List<string> m_ItemTitles;
        private static ItemsPanel m_Current;
        public static ItemsPanel Current
        { get { return m_Current; } }
        void Awake()
        {
            m_Current = this;
        }
        void Start()
        {

            Button upgradebtn = GetComponent<Button>();

            m_MainItemPanel.gameObject.SetActive(false);
            //int itemcount = 6;
            //int itemcount = m_Contents.m_DefendEquipment.Length + m_Contents.m_AttackEquipment.Length + m_Contents.m_Powers.Length;
            m_ItemTitles = new List<string>();
            //m_ItemButtons = new Image[m_Contents.m_AllEquipment.Length];


            m_ItemNum = 0;
            Debug.Log("is reading");
            ShowItems();
            UpdateItemInfo();
            UpdateObject();
            m_ItemSelectBack.transform.position = new Vector3(0, 10000, 0);
            // m_AttackTitleText.transform.position += new Vector2(0,BtnParent.transform.position.y);
        }

        // Update is called once per frame
        void Update()
        {
            //m_ItemLevelText.text = m_Contents.m_AllEquipment[m_ItemNum].m_ItemLevel.ToString();



            //m_ItemTypeText.text = m_ItemTypeStrings[(int)m_Contents.m_AllEquipment[m_ItemNum].m_UnitType];
            //m_ItemTypeIcon.sprite = m_ItemTypeSprites[(int)m_Contents.m_AllEquipment[m_ItemNum].m_UnitType];


        }

        public void ShowItems()
        {
            //for (int i = 0; i < m_Contents.m_AllEquipment.Length; i++)
            //{
            //    if (m_Contents.m_AllEquipment[i].m_UnitType== UnitTypes.Defense || m_Contents.m_AllEquipment[i].m_UseInMulti) //before launching online, only upgrade defense objects
            //    {
            //        if (m_Contents.m_AllEquipment[i].m_SpecialPackage && !m_Contents.m_AllEquipment[i].m_Unlocked)
            //            continue;

            //        GameObject btn = Instantiate(ItemBtnPrefab1);
            //        btn.transform.SetParent(Defence_Items_Parent);
            //        Btn_Item b = btn.GetComponent<Btn_Item>();
            //        b.LevelNum = i;
            //        b.m_InSkinsUI = false;
            //        m_ItemButtons[i] = btn.GetComponent<Image>();
            //        m_ItemTitles.Add(m_Contents.m_AllEquipment[i].m_TitleEnglish);
            //        Debug.Log(i + "itemcount defend");
            //        RectTransform rt = btn.GetComponent<RectTransform>();
            //        rt.anchoredPosition = Vector2.zero;

            //    }
            //}
        }
        public void UpdateItemInfo()
        {
            //Equipment currentEq = m_Contents.m_AllEquipment[m_ItemNum];
            //m_ItemSelectBack.transform.position = m_ItemButtons[m_ItemNum].transform.position;
            ////m_MainItemPanelImage.sprite = m_Contents.m_AllEquipment[m_ItemNum].m_Icon;
            //m_MainItemPanelName.text = currentEq.m_TitleFarsi;
            ////m_MainItemLevelText.text = m_Contents.m_AllEquipment[itemNum].m_UpgradeLevel.ToString() + " ﺢﻄﺳ";
            //m_MainItemPanelInfo.text = currentEq.m_Info.ToString();


            //for (int i = 0; i < m_Contents.m_UpgradeTitles.Length; i++)
            //{
            //    if (currentEq.m_HavePowers[i])
            //    {
            //        m_ItemUpgradePanels[i].gameObject.SetActive(true);
            //        m_ItemUpgradePanels[i].m_TargetEquipment = currentEq;
            //        m_ItemUpgradePanels[i].ResetBar();
            //    }
            //    else
            //    {
            //        m_ItemUpgradePanels[i].gameObject.SetActive(false);
            //    }
            //}

            //if (currentEq.m_Unlocked || currentEq.m_UnlockedAtStart)
            //{
            //    m_MainItemPanelImage.color = Color.white;

            //}
            //else
            //{
            //    m_MainItemPanelImage.color = new Color(0, 0, 0, .6f);
            //    //for (int i = 0; i < 3; i++)
            //    //{
            //    //    m_ItemUpgradePanels[i].gameObject.SetActive(false);
            //    //}
            //}

            ////if (powerLevels[0] < 8)
            ////    m_UpgradePriceTexts[0].text = (currentEq.m_SpeedPrice[powerLevels[0]]).ToString();
            ////if (powerLevels[1] < 8)
            ////    m_UpgradePriceTexts[1].text = (currentEq.m_DamagePowerPrice[powerLevels[1]]).ToString();
            ////if (powerLevels[2] < 8)
            ////    m_UpgradePriceTexts[2].text = (currentEq.m_DefencePowerPrice[powerLevels[2]]).ToString();

            //m_MainItemPanel.gameObject.SetActive(true);

            //if (currentEq.m_Unlocked || currentEq.m_UnlockedAtStart)
            //{
            //    m_BuyBtn.gameObject.SetActive(false);

            //}
            //else
            //{
            //    m_ItemPriceText.text = currentEq.m_PriceInCoin.ToString();
            //    m_BuyBtn.gameObject.SetActive(true);
            //}
        }
        public void ShowMainItemPanel(int itemNum, int state) // state = 0 means defence / state = 1 means attack / state = 2 means power
        {
            m_ItemNum = itemNum;
            UpdateItemInfo();
            UpdateObject();
        }
        public void BuyUpgradeBtn()
        {

            //if (m_DataStorage.Coin >= m_Contents.m_AllEquipment[m_ItemNum].m_PriceInCoin)
            //{
            //    m_Contents.m_AllEquipment[m_ItemNum].m_Unlocked = true;
            //    m_DataStorage.Coin -= m_Contents.m_AllEquipment[m_ItemNum].m_PriceInCoin;
            //    XPBarAdd.Current.EarnXP(20);
            //    SoundGallery.PlaySound("You Win (1)");
            //    UpdateItemInfo();
            //    m_PlayerData.SaveData();
            //    m_DataStorage.SaveData();

            //    Debug.Log(m_ItemNum + "number sent");
            //    GameObject obj = UISystem.FindOpenUIByName("InventoryUI");
            //    if (obj != null)
            //    {
            //        obj.GetComponent<InventoryUI>().UpdateTabAlertsInventory();
            //    }
            //    MainMenuTabsUI.m_Main.UpdateTabAlerts();
            //    AppMetricaLogingEvents.m_Current.SendPurchasedEquipment(m_ItemNum);
            //}
            //else
            //{
            //    UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 0, m_UITextContentsContents.m_Messages[2], m_UIGraphicContents.m_Graphics[7]);
            //    msg.f_Clicked_Yes = ShowStore;

            //}
        }
        public bool ShowStore()
        {
            if (MainMenuTabsUI.m_Main != null)
            {
                MainMenuTabsUI.m_Main.SelectTab(3);
                MainMenuTabsUI.m_Main.m_CurrentTabObject.GetComponentInChildren<UITabControl>().SelectTab(2);
            }
            else
            {
                GameObject obj = UISystem.ShowUI("StoreUI");
                obj.GetComponentInChildren<UITabControl>().SelectTab(2);
                //UISystem.RemoveUI("MainMenuUI");
                SoundGallery.PlaySound("button32");
            }
            return true;
        }
        public void CreateUpgradeParticle()
        {
            GameObject obj = Instantiate(m_UpgradeParticle);
            obj.transform.position = m_MainItemPanelImage.transform.position;
            Destroy(obj, 3);
            SoundGallery.PlaySound("Coin1");
        }




        public void UpdateObject()
        {
            //Equipment eq = m_Contents.m_AllEquipment[m_ItemNum];
            //AnimObjectControl.m_Main.CreateObject(eq.m_AnimObj);

            //int hatNum = eq.m_HatNum;
            //EqAnimObject obj = AnimObjectControl.m_Main.m_CurrentObj.GetComponent<EqAnimObject>();
            //obj.SetHat(m_Contents.m_AnimalSkin[hatNum].m_HatSprite);
        }

    }

}
