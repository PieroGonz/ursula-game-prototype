using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AgeOfHeroes.ScriptableObjects;

namespace AgeOfHeroes.UI
{
    public class ItemUpgradeBar : MonoBehaviour
    {
        [SerializeField]
        private Contents m_Contents;
        [SerializeField]
        private DataStorage m_DataStorage;
        [SerializeField]
        private GameplayData m_GameplayData;

        public ItemsPanel m_ItemsPanel;

        public Image m_ItemDataBar;
        public Image m_ItemDataBarBack;
        public Text m_UpgradeLabelText;
        public Text m_UpgradePriceText;
        public Text m_UpgradeNum;
        public Image m_UpgradeBtn;

        public int m_TargetUpgrade;
        //[HideInInspector]
        //public Equipment m_TargetEquipment;
        [SerializeField, Space]
        private UIGraphicContents m_UIGraphicContents;
        [SerializeField, Space]
        private UITextContents m_UITextContentsContents;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //float power = m_TargetEquipment.m_PowerMinAmount[m_TargetUpgrade] + m_TargetEquipment.m_PowerLevels[m_TargetUpgrade] * m_TargetEquipment.m_AddPowerAmount[m_TargetUpgrade];
            //m_UpgradeLabelText.text = m_Contents.m_UpgradeTitles[m_TargetUpgrade];
            //float fill1 = (power) / m_Contents.m_UpgradeMax[m_TargetUpgrade];
            //m_ItemDataBar.fillAmount = Mathf.Lerp(m_ItemDataBar.fillAmount, fill1, 2 * Time.deltaTime);
            //m_UpgradeNum.text = (power).ToString();

            //if (m_TargetEquipment.m_PowerLevels[m_TargetUpgrade] < 10)
            //{
            //    m_ItemDataBarBack.gameObject.SetActive(true);
            //    float fill = (m_TargetEquipment.m_PowerMinAmount[m_TargetUpgrade] + (m_TargetEquipment.m_PowerLevels[m_TargetUpgrade] + 1) * m_TargetEquipment.m_AddPowerAmount[m_TargetUpgrade]) / m_Contents.m_UpgradeMax[m_TargetUpgrade];
            //    m_ItemDataBarBack.fillAmount = Mathf.Lerp(m_ItemDataBarBack.fillAmount, fill, 2 * Time.deltaTime);

            //    int price = m_Contents.m_UpgradePriceStart[m_TargetUpgrade] + (m_Contents.m_UpgradePriceAdd[m_TargetUpgrade] * m_TargetEquipment.m_PowerLevels[m_TargetUpgrade]);
            //    m_UpgradePriceText.text = (price).ToString();

            //    Debug.Log("inside");
            //}
            //else
            //{
            //    m_ItemDataBarBack.gameObject.SetActive(false);
            //}

            //if (m_TargetEquipment.m_Unlocked || m_TargetEquipment.m_UnlockedAtStart)
            //{
            //    if (m_TargetEquipment.m_PowerLevels[m_TargetUpgrade] < 10)
            //    {
            //        m_UpgradeBtn.gameObject.SetActive(true);
            //    }
            //    else
            //    {
            //        m_UpgradeBtn.gameObject.SetActive(false);
            //    }
            //}
            //else
            //{
            //    m_UpgradeBtn.gameObject.SetActive(false);
            //}
        }

        public void ResetBar()
        {
            //m_ItemDataBar.fillAmount = 0;
            m_ItemDataBarBack.fillAmount = 0;
        }

        public void Btn_Upgrade()
        {
            //print("click");
            //if (m_TargetEquipment.m_Unlocked || m_TargetEquipment.m_UnlockedAtStart)
            //{
            //    int price = m_Contents.m_UpgradePriceStart[m_TargetUpgrade] + (m_Contents.m_UpgradePriceAdd[m_TargetUpgrade] * m_TargetEquipment.m_PowerLevels[m_TargetUpgrade]);
            //    print("click");
            //    if (m_DataStorage.Coin >= price)
            //    {
            //        m_TargetEquipment.m_PowerLevels[m_TargetUpgrade]++;
            //        m_DataStorage.Coin -= price;
            //        ResetBar();

            //        //UpdateItemInfo();
            //        XPBarAdd.Current.EarnXP(20);

            //        int sum = 0;
            //        int count = 0;
            //        for (int i = 0; i < 8; i++)
            //        {
            //            if (m_TargetEquipment.m_HavePowers[i])
            //            {
            //                count++;
            //                sum += m_TargetEquipment.m_PowerLevels[i];
            //            }
            //        }
            //        int level = sum / count;
            //        print("level-" + level);
            //        m_TargetEquipment.m_ItemLevel = level;

            //        m_DataStorage.SaveData();

            //        m_ItemsPanel.CreateUpgradeParticle();
            //        GameObject obj = UISystem.FindOpenUIByName("InventoryUI");
            //        if (obj != null)
            //        {
            //            obj.GetComponent<InventoryUI>().UpdateTabAlertsInventory();
            //        }
            //        MainMenuTabsUI.m_Main.UpdateTabAlerts();
            //    }
            //    else
            //    {
            //        UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 0, m_UITextContentsContents.m_Messages[2], m_UIGraphicContents.m_Graphics[7]);
            //        msg.f_Clicked_Yes = ShowStore;

            //    }

            //}
            //else
            //{
            //    SoundGallery.PlaySound("button32");
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
                //  UISystem.RemoveUI("MainMenuUI");
                SoundGallery.PlaySound("button32");
            }
            return true;
        }
    }
}