using AgeOfHeroes.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AgeOfHeroes.UI
{
    public class AccessorySelectPanel : MonoBehaviour
    {
        [HideInInspector]
        public string m_StrTitle;
        public RectTransform m_ItemsPanel;
        private ItemListButton[] m_Buttons;
        int m_ButtonCount = 0;
        public GameObject m_ItemButtonPrefab;

        [HideInInspector]
        public Character m_SelectedCharacter;

        public SquadUI m_ParentSquadUI;

        [SerializeField, Space]
        private Contents m_Contents;
        [SerializeField]
        private DataStorage m_DataStorage;
        //[SerializeField, Space]
        // private GameplayData m_GameplayData;
        [SerializeField]
        private PlayerData m_PlayerData;
        [SerializeField]
        private UIGraphicContents m_UIGraphicContents;
        [SerializeField]
        private UITextContents m_UITextContentsContents;
        // Start is called before the first frame update
        void Start()
        {
            Text txt1 = UISystem.FindText(gameObject, "txt-title");
            switch (m_StrTitle)
            {
                case "Skin":
                    txt1.text = "ﯽﻧﺎﻣﺮﻬﻗ ﯼﺎﻫ ﺱﺎﺒﻟ";
                    break;
                case "Weapon":
                    txt1.text = "ﯽﻧﺎﻣﺮﻬﻗ ﯼﺎﻫ ﻪﺤﻠﺳﺍ";
                    break;

            }
            UpdateButtonList();
        }
        // Update is called once per frame
        void Update()
        {

        }

        public void UpdateButtonList()
        {
            switch (m_StrTitle)
            {
                case "Skin":
                    RemoveButtons();
                    m_ButtonCount = 0;
                    m_Buttons = new ItemListButton[m_SelectedCharacter.m_Skins.Length];
                    for (int i = 0; i < m_Buttons.Length; i++)
                    {

                        CharacterSkins characterskin = m_SelectedCharacter.m_Skins[i];
                        GameObject btn = Instantiate(m_ItemButtonPrefab);
                        m_Buttons[m_ButtonCount] = btn.GetComponent<ItemListButton>();
                        m_Buttons[m_ButtonCount].transform.SetParent(m_ItemsPanel);
                        m_Buttons[m_ButtonCount].m_FarsiTitles.text = characterskin.m_TitleEnglish;
                        m_Buttons[m_ButtonCount].m_ItemNum = i;
                        m_Buttons[m_ButtonCount].f_Clicked = SelectItem;
                        Image img1;
                        Text txt1;

                        img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-icon");
                        img1.sprite = characterskin.m_Icon;
                        if (characterskin.m_Unlocked || characterskin.m_UnlockedAtStart)
                            img1.color = Color.white;
                        else
                            img1.color = new Color(.6f, .6f, .6f, 1);



                        txt1 = UISystem.FindText(m_Buttons[m_ButtonCount].gameObject, "text-title");
                        txt1.text = characterskin.m_TitleEnglish;

                        if (characterskin.m_Unlocked || characterskin.m_UnlockedAtStart)
                        {
                            img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-lock");
                            img1.gameObject.SetActive(false);

                            img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-check");
                            img1.gameObject.SetActive(false);

                            img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-priceback-coin");
                            img1.gameObject.SetActive(false);

                            img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-priceback-gem");
                            img1.gameObject.SetActive(false);

                            img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-priceback-cash");
                            img1.gameObject.SetActive(false);
                        }
                        else
                        {
                            img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-lock");
                            img1.gameObject.SetActive(true);

                            img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-check");
                            img1.gameObject.SetActive(false);

                            if (characterskin.m_PriceInCoin > 0)
                            {
                                img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-priceback-coin");
                                img1.gameObject.SetActive(true);

                                img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-priceback-gem");
                                img1.gameObject.SetActive(false);

                                img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-priceback-cash");
                                img1.gameObject.SetActive(false);

                                txt1 = UISystem.FindText(m_Buttons[m_ButtonCount].gameObject, "text-price-coin");
                                txt1.text = characterskin.m_PriceInCoin.ToString();
                            }
                            else if (characterskin.m_PriceInGem > 0)
                            {
                                img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-priceback-coin");
                                img1.gameObject.SetActive(false);

                                img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-priceback-gem");
                                img1.gameObject.SetActive(true);

                                img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-priceback-cash");
                                img1.gameObject.SetActive(false);

                                txt1 = UISystem.FindText(m_Buttons[m_ButtonCount].gameObject, "text-price-gem");
                                txt1.text = characterskin.m_PriceInGem.ToString();
                            }
                            else
                            {
                                img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-priceback-coin");
                                img1.gameObject.SetActive(false);

                                img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-priceback-gem");
                                img1.gameObject.SetActive(false);

                                img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-priceback-cash");
                                img1.gameObject.SetActive(false);
                            }
                        }

                        m_ButtonCount++;
                    }
                    break;
                case "Weapon":
                    RemoveButtons();
                    m_ButtonCount = 0;
                    m_Buttons = new ItemListButton[m_SelectedCharacter.m_Weapons.Length];
                    for (int i = 0; i < m_Buttons.Length; i++)
                    {

                        CharacterEquipment charactersequip = m_SelectedCharacter.m_Weapons[i];
                        GameObject btn = Instantiate(m_ItemButtonPrefab);
                        m_Buttons[m_ButtonCount] = btn.GetComponent<ItemListButton>();
                        m_Buttons[m_ButtonCount].transform.SetParent(m_ItemsPanel);
                        m_Buttons[m_ButtonCount].m_FarsiTitles.text = charactersequip.m_TitleEnglish;
                        m_Buttons[m_ButtonCount].m_ItemNum = i;
                        m_Buttons[m_ButtonCount].f_Clicked = SelectItem;
                        Image img1;
                        Text txt1;

                        img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-icon");
                        img1.sprite = charactersequip.m_Icon;
                        if (charactersequip.m_Unlocked || charactersequip.m_UnlockedAtStart)
                            img1.color = Color.white;
                        else
                            img1.color = new Color(.6f, .6f, .6f, 1);



                        txt1 = UISystem.FindText(m_Buttons[m_ButtonCount].gameObject, "text-title");
                        txt1.text = charactersequip.m_TitleEnglish;

                        if (charactersequip.m_Unlocked || charactersequip.m_UnlockedAtStart)
                        {
                            img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-lock");
                            img1.gameObject.SetActive(false);

                            img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-check");
                            img1.gameObject.SetActive(false);

                            img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-priceback-coin");
                            img1.gameObject.SetActive(false);

                            img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-priceback-gem");
                            img1.gameObject.SetActive(false);

                            img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-priceback-cash");
                            img1.gameObject.SetActive(false);
                        }
                        else
                        {
                            img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-lock");
                            img1.gameObject.SetActive(true);

                            img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-check");
                            img1.gameObject.SetActive(false);

                            if (charactersequip.m_PriceInCoin > 0)
                            {
                                img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-priceback-coin");
                                img1.gameObject.SetActive(true);

                                img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-priceback-gem");
                                img1.gameObject.SetActive(false);

                                img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-priceback-cash");
                                img1.gameObject.SetActive(false);

                                txt1 = UISystem.FindText(m_Buttons[m_ButtonCount].gameObject, "text-price-coin");
                                txt1.text = charactersequip.m_PriceInCoin.ToString();
                            }
                            else if (charactersequip.m_PriceInGem > 0)
                            {
                                img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-priceback-coin");
                                img1.gameObject.SetActive(false);

                                img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-priceback-gem");
                                img1.gameObject.SetActive(true);

                                img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-priceback-cash");
                                img1.gameObject.SetActive(false);

                                txt1 = UISystem.FindText(m_Buttons[m_ButtonCount].gameObject, "text-price-gem");
                                txt1.text = charactersequip.m_PriceInGem.ToString();
                            }
                            else
                            {
                                img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-priceback-coin");
                                img1.gameObject.SetActive(false);

                                img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-priceback-gem");
                                img1.gameObject.SetActive(false);

                                img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-priceback-cash");
                                img1.gameObject.SetActive(false);
                            }
                        }

                        m_ButtonCount++;
                    }
                    break;

            }

        }

        public void RemoveButtons()
        {
            if (m_Buttons != null && m_Buttons.Length > 1)
            {
                for (int i = 0; i < m_Buttons.Length; i++)
                {
                    Destroy(m_Buttons[i].gameObject);
                }
            }
            //  Debug.Log("btn");
            m_Buttons = new ItemListButton[1];
        }

        public bool SelectItem(int num, int num2 = 0)
        {
            switch (m_StrTitle)
            {
                case "Skin":

                    CharacterSkins chsk = m_SelectedCharacter.m_Skins[num];

                    if (chsk.m_Unlocked || chsk.m_UnlockedAtStart)
                    {
                        m_SelectedCharacter.m_SkinNum = num;
                        UISystem.m_Main.m_SquadSettings[0].m_CharSkinNums[m_ParentSquadUI.m_TeamPlayerSelectNum] = num;
                        for (int i = 0; i < m_Buttons.Length; i++)
                        {
                            Image img = UISystem.FindImage(m_Buttons[i].gameObject, "img-check");

                            if (i == num)
                            {
                                img.gameObject.SetActive(true);
                            }
                            else
                            {
                                Image img1 = UISystem.FindImage(m_Buttons[i].gameObject, "img-check");
                                img1.gameObject.SetActive(false);
                            }
                        }

                        m_DataStorage.SaveData();
                        UISystem.m_Main.m_SquadSettings[0].CreatePlayer(m_ParentSquadUI.m_TeamPlayerSelectNum);
                        m_ParentSquadUI.UpdatePlayerInfo();
                        m_ParentSquadUI.CloseAccessories();
                        //update character with skin info

                    }
                    else
                    {
                        if (chsk.m_SpecialPackage)
                        {
                            GameObject obj = UISystem.ShowUI("CoinShopUI");
                            obj.GetComponentInChildren<UITabControl>().SelectTab(2);
                            UISystem.RemoveUI(gameObject);
                        }
                        else if (m_DataStorage.Coin >= chsk.m_PriceInCoin)
                        {
                            //m_TempItemsNum = num;
                            UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 5, null, null);
                            Image type = UISystem.FindImage(msg.gameObject, "CoinAmountObj");
                            Image img = UISystem.FindImage(msg.gameObject, "MessageImageUnlockItems");
                            Text maintxt = UISystem.FindText(msg.gameObject, "main-txt");
                            maintxt.text = chsk.m_TitleEnglish;
                            img.sprite = chsk.m_Icon;
                            type.gameObject.SetActive(true);
                            img.gameObject.SetActive(true);
                            Text pricetxt = UISystem.FindText(msg.gameObject, "CoinCount");
                            pricetxt.text = chsk.m_PriceInCoin.ToString();
                            pricetxt.gameObject.SetActive(true);
                            msg.f_Clicked_Yes = () => PurchaseItem(num);


                        }
                        else
                        {
                            UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 0, m_UITextContentsContents.m_Messages[2], m_UIGraphicContents.m_Graphics[11]);
                            msg.f_Clicked_Yes = () => ShowStore(0);
                        }
                    }

                    break;
                case "Weapon":

                    CharacterEquipment cheq = m_SelectedCharacter.m_Weapons[num];

                    if (cheq.m_Unlocked || cheq.m_UnlockedAtStart)
                    {
                        m_SelectedCharacter.m_WeaponNum = num;
                        UISystem.m_Main.m_SquadSettings[0].m_CharWeaponNums[m_ParentSquadUI.m_TeamPlayerSelectNum] = num;

                        for (int i = 0; i < m_Buttons.Length; i++)
                        {
                            Image img = UISystem.FindImage(m_Buttons[i].gameObject, "img-check");

                            if (i == num)
                            {
                                img.gameObject.SetActive(true);
                            }
                            else
                            {
                                Image img1 = UISystem.FindImage(m_Buttons[i].gameObject, "img-check");
                                img1.gameObject.SetActive(false);
                            }
                        }

                        m_DataStorage.SaveData();
                        UISystem.m_Main.m_SquadSettings[0].CreatePlayer(m_ParentSquadUI.m_TeamPlayerSelectNum);
                        m_ParentSquadUI.UpdatePlayerInfo();
                        m_ParentSquadUI.CloseAccessories();
                        //update character with weapon info
                    }
                    else
                    {
                        if (cheq.m_SpecialPackage)
                        {
                            GameObject obj = UISystem.ShowUI("CoinShopUI");
                            obj.GetComponentInChildren<UITabControl>().SelectTab(2);
                            UISystem.RemoveUI(gameObject);
                        }
                        else if (m_DataStorage.Coin >= cheq.m_PriceInCoin)
                        {

                            UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 5, null, null);
                            Image type = UISystem.FindImage(msg.gameObject, "CoinAmountObj");
                            Image img = UISystem.FindImage(msg.gameObject, "MessageImageUnlockItems");
                            Text maintxt = UISystem.FindText(msg.gameObject, "main-txt");
                            maintxt.text = cheq.m_TitleEnglish;
                            img.sprite = cheq.m_Icon;
                            type.gameObject.SetActive(true);
                            img.gameObject.SetActive(true);
                            Text pricetxt = UISystem.FindText(msg.gameObject, "CoinCount");
                            pricetxt.text = cheq.m_PriceInCoin.ToString();
                            pricetxt.gameObject.SetActive(true);
                            msg.f_Clicked_Yes = () => PurchaseItem(num);


                        }
                        else
                        {
                            UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 0, m_UITextContentsContents.m_Messages[2], m_UIGraphicContents.m_Graphics[11]);
                            msg.f_Clicked_Yes = () => ShowStore(0);
                        }
                    }

                    break;
            }


            SoundGallery.PlaySound("button32");

            return true;
        }
        public bool PurchaseItem(int itemnum)
        {
            switch (m_StrTitle)
            {
                case "Skin":
                    m_DataStorage.Coin -= m_SelectedCharacter.m_Skins[itemnum].m_PriceInCoin;
                    m_SelectedCharacter.m_Skins[itemnum].m_Unlocked = true;
                    XPBarAdd.Current.EarnXP(10);
                    m_DataStorage.SaveData();
                    StartCoroutine(Co_UnlockEffect(itemnum));


                    UpdateButtonList();

                    //update character with skin info

                    break;
                case "Weapon":
                    m_DataStorage.Coin -= m_SelectedCharacter.m_Weapons[itemnum].m_PriceInCoin;
                    m_SelectedCharacter.m_Weapons[itemnum].m_Unlocked = true;
                    XPBarAdd.Current.EarnXP(10);
                    m_DataStorage.SaveData();
                    StartCoroutine(Co_UnlockEffect(itemnum));
                    UpdateButtonList();

                    //update character with weapon info
                    break;
            }

            SoundGallery.PlaySound("You Win (1)");

            return true;
        }
        IEnumerator Co_UnlockEffect(int num)
        {
            float lerp = 0;
            while (true)
            {
                lerp += 4 * Time.deltaTime;
                yield return null;
                if (lerp >= 2 * Math.PI)
                {
                    break;
                }
            }
        }
        public bool ShowStore(int num)
        {
            GameObject obj = UISystem.ShowUI("CoinShopUI");
            obj.GetComponentInChildren<UITabControl>().SelectTab(num);
            //UISystem.RemoveUI("SquadUI");
            return true;
        }

    }
}