using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgeOfHeroes.ScriptableObjects;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Threading.Tasks;


namespace AgeOfHeroes.UI
{

    public class SquadUI : MonoBehaviour
    {
        public AccessorySelectPanel m_AccessoryPanel;
        public RectTransform m_ItemsPanel;


        public int m_TeamPlayerSelectNum = 0;
        public int[] m_TempCharacterNums;
        public Text m_PlayerName;

        public Text[] m_CharacterAtrTexts;
        public Text m_LevelUpPrice;
        public Button m_UpgradeButton;
        public Sprite[] m_StatIcons;
        public Sprite[] m_Levels;

        private ItemListButton[] m_Buttons;
        int m_ButtonCount = 0;

        public GameObject m_PlayerItemButton;

        public PlayerPowersBar[] m_PlayerPowerBars;
        public Image m_PlayerImage;
        public Image m_PlayerLevel;
        public Image m_PlayerSelectPanel;
        public Image m_WeaponImage;
        public Image m_SkinImage;
        private bool m_PurchaseMode;

        public Material m_GrayscaleUIMat;

        public Vector3[] m_PlayerImagePositions;
        public float[] m_PlayerImageScales;

        [SerializeField, Space]
        private Contents m_Contents;
        [SerializeField, Space]
        private DataStorage m_DataStorage;
        //[SerializeField, Space]
        // private GameplayData m_GameplayData;
        [SerializeField, Space]
        private PlayerData m_PlayerData;
        [SerializeField, Space]
        private UIGraphicContents m_UIGraphicContents;
        [SerializeField, Space]
        private UITextContents m_UITextContentsContents;


        void Start()
        {
            m_TempCharacterNums = new int[3];
            for (int i = 0; i < m_TempCharacterNums.Length; i++)
            {
                m_TempCharacterNums[i] = m_DataStorage.m_CharactersNumber[i];
            }
            m_TeamPlayerSelectNum = 0;
            UpdatePlayerInfo();
            UpdateCharactersList();
            m_AccessoryPanel.gameObject.SetActive(false);


        }

        // Update is called once per frame
        void Update()
        {
            Vector3[] imgPos = new Vector3[3];
            imgPos[0] = new Vector3(0, 0, 0);
            imgPos[1] = new Vector3(-300, 60, 0);
            imgPos[2] = new Vector3(300, 60, 0);

            //if (m_TeamPlayerSelectNum == 0)
            //{
            //    m_PlayerImages[0].color = Color.white;
            //    m_PlayerImages[1].color = new Color(.3f, .3f, .3f, 1);
            //    m_PlayerImages[2].color = new Color(.3f, .3f, .3f, 1);
            //}
            //else if (m_TeamPlayerSelectNum == 1)
            //{
            //    m_PlayerImages[1].color = Color.white;
            //    m_PlayerImages[0].color = new Color(.3f, .3f, .3f, 1);
            //    m_PlayerImages[2].color = new Color(.3f, .3f, .3f, 1);
            //}
            //else if (m_TeamPlayerSelectNum == 2)
            //{
            //    m_PlayerImages[2].color = Color.white;
            //    m_PlayerImages[0].color = new Color(.3f, .3f, .3f, 1);
            //    m_PlayerImages[1].color = new Color(.3f, .3f, .3f, 1);
            //}
            float[] scales = new float[3] { 1, .6f, .6f };
            SquadCustomize sc = UISystem.m_Main.m_SquadSettings[0];
            for (int i = 0; i < 3; i++)
            {
                int num = i - sc.m_CharacterSelectNum;
                if (num == -1)
                    num = 2;
                if (num == -2)
                    num = 1;
                //sc.m_CharacterPoints[i].localPosition = sc.m_Positions[num];

                sc.m_CharacterPoints[i].localPosition = Vector3.Lerp(sc.m_CharacterPoints[i].localPosition, sc.m_Positions[num], 8 * Time.deltaTime);
                sc.m_CharacterPoints[i].localScale = Vector3.Lerp(sc.m_CharacterPoints[i].localScale, scales[num] * Vector3.one, 8 * Time.deltaTime);
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
            // Debug.Log("btn");
            m_Buttons = new ItemListButton[1];
        }
        public void BtnAccessories(string itemname)
        {
            if (m_Contents.m_Characters[m_TempCharacterNums[m_TeamPlayerSelectNum]].m_Unlocked)
            {
                m_AccessoryPanel.m_StrTitle = itemname;
                m_AccessoryPanel.m_SelectedCharacter = m_Contents.m_Characters[m_TempCharacterNums[m_TeamPlayerSelectNum]];
                m_AccessoryPanel.gameObject.SetActive(true);
                m_AccessoryPanel.UpdateButtonList();
            }
        }
        public void CloseAccessories()
        {
            m_AccessoryPanel.gameObject.SetActive(false);
        }

        public void UpdateCharactersList()
        {
            float size = 1500;
            m_ItemsPanel.sizeDelta = new Vector2(m_ItemsPanel.sizeDelta.x, size);
            m_ItemsPanel.anchoredPosition = new Vector2(0, -1 * (size / 2f));
            RemoveButtons();
            m_ButtonCount = 0;

            int[] listSort = new int[10] { 0, 2, 3, 1, 4, 5, 6, 7, 8, 9 };

            m_Buttons = new ItemListButton[m_Contents.m_Characters.Length];
            for (int i = 0; i < m_Contents.m_Characters.Length; i++)
            {
                Character character = m_Contents.m_Characters[listSort[i]];
                GameObject btn = Instantiate(m_PlayerItemButton);
                m_Buttons[m_ButtonCount] = btn.GetComponent<ItemListButton>();
                m_Buttons[m_ButtonCount].transform.SetParent(m_ItemsPanel);
                m_Buttons[m_ButtonCount].m_FarsiTitles.text = character.m_TitleEnglish;
                m_Buttons[m_ButtonCount].m_ItemNum = listSort[i];
                m_Buttons[m_ButtonCount].m_Level.sprite = m_Levels[character.m_ItemLevel];
                m_Buttons[m_ButtonCount].f_Clicked = SelectPlayer;
                Image img1;
                Text txt1;
                img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-icon");


                img1.sprite = character.m_Icon;

                if (!character.m_Unlocked && !character.m_UnlockedAtStart)
                {
                    img1.material = m_Buttons[m_ButtonCount].m_GrayscaleMat;
                    img1.color = new Color(.6f, .6f, .6f, 1);
                }

                txt1 = UISystem.FindText(m_Buttons[m_ButtonCount].gameObject, "text-title");
                txt1.text = character.m_TitleEnglish;

                img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-check");
                img1.gameObject.SetActive(false);

                img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-lock");
                img1.gameObject.SetActive(false);

                img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-priceback-coin");
                img1.gameObject.SetActive(false);

                img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-priceback-gem");
                img1.gameObject.SetActive(false);

                img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-priceback-cash");
                img1.gameObject.SetActive(false);

                m_ButtonCount++;
            }
        }


        public bool SelectPlayer(int num, int num2 = 0)
        {
            int foundUsed = -1;
            for (int i = 0; i < 3; i++)
            {
                if (i == m_TeamPlayerSelectNum)
                    continue;

                if (m_TempCharacterNums[i] == num)
                {
                    foundUsed = i;
                    break;
                }
            }

            if (foundUsed == -1)
            {

                UISystem.m_Main.m_SquadSettings[0].m_CharSkinNums[m_TeamPlayerSelectNum] = m_Contents.m_Characters[num].m_SkinNum;
                UISystem.m_Main.m_SquadSettings[0].m_CharWeaponNums[m_TeamPlayerSelectNum] = m_Contents.m_Characters[num].m_WeaponNum;

                m_TempCharacterNums[m_TeamPlayerSelectNum] = num;
                UISystem.m_Main.m_SquadSettings[0].m_CharacterNums[m_TeamPlayerSelectNum] = num;
                UISystem.m_Main.m_SquadSettings[0].CreatePlayer(m_TeamPlayerSelectNum);
            }
            else
            {

                int a = m_TempCharacterNums[m_TeamPlayerSelectNum];
                if (!m_Contents.m_Characters[a].m_Unlocked)
                {
                    m_TempCharacterNums[m_TeamPlayerSelectNum] = m_DataStorage.m_CharactersNumber[m_TeamPlayerSelectNum];
                    a = m_TempCharacterNums[m_TeamPlayerSelectNum];
                }
                int b = m_TempCharacterNums[foundUsed];

                m_TempCharacterNums[m_TeamPlayerSelectNum] = num;
                m_TempCharacterNums[foundUsed] = a;

                int skinA = UISystem.m_Main.m_SquadSettings[0].m_CharSkinNums[m_TeamPlayerSelectNum];
                int skinB = UISystem.m_Main.m_SquadSettings[0].m_CharSkinNums[foundUsed];
                UISystem.m_Main.m_SquadSettings[0].m_CharSkinNums[m_TeamPlayerSelectNum] = skinB;
                UISystem.m_Main.m_SquadSettings[0].m_CharSkinNums[foundUsed] = skinA;

                UISystem.m_Main.m_SquadSettings[0].m_CharacterNums[m_TeamPlayerSelectNum] = num;
                UISystem.m_Main.m_SquadSettings[0].m_CharacterNums[foundUsed] = a;

                UISystem.m_Main.m_SquadSettings[0].CreatePlayer(m_TeamPlayerSelectNum);
                UISystem.m_Main.m_SquadSettings[0].CreatePlayer(foundUsed);

            }

            if (m_Contents.m_Characters[num].m_Unlocked || m_Contents.m_Characters[num].m_UnlockedAtStart)
            {
                m_PurchaseMode = false;

                for (int i = 0; i < 3; i++)
                {
                    m_DataStorage.m_CharactersNumber[i] = m_TempCharacterNums[i];
                }
            }
            else
            {
                m_PurchaseMode = true;
            }
            UpdatePlayerInfo();
            SoundGallery.PlaySound("button32");

            return true;
        }

        public void UpdatePlayerInfo()
        {
            Image characterlock = UISystem.FindImage(m_PlayerSelectPanel.gameObject, "lock-panel");

            Character character = m_Contents.m_Characters[m_TempCharacterNums[m_TeamPlayerSelectNum]];
            m_PlayerName.text = character.m_TitleEnglish;
            UpdateCharacterStats(character);
            if (!m_PurchaseMode)
            {

                UpdateCharacterPowerIcons(character);
                UpdateCharacterLock(characterlock, character);

                m_UpgradeButton.gameObject.SetActive(character.m_ItemLevel < 4);
                m_LevelUpPrice.text = ((character.m_ItemLevel + 1) * 5).ToString();
            }
            else
            {
                UpdatePurchaseModeStats(character);
                UpdateCharacterPowerIcons(character);
                UpdateCharacterLock(characterlock, character);
            }
        }

        private void UpdateCharacterStats(Character character)
        {
            float power = (character.m_ItemLevel + 1);
            m_CharacterAtrTexts[0].text = ((int)power).ToString();
            power = character.m_Health + 10f * character.m_ItemLevel;
            m_CharacterAtrTexts[1].text = ((int)power).ToString();
            power = character.m_Damage + 4f * character.m_ItemLevel;
            m_CharacterAtrTexts[2].text = ((int)power).ToString();

            power = character.m_Defence + 10f * character.m_ItemLevel;
            m_CharacterAtrTexts[3].text = ((int)power).ToString();

            UpdateCharactersList();
            m_PlayerLevel.sprite = m_Levels[character.m_ItemLevel];

        }

        private void UpdateCharacterPowerIcons(Character character)
        {
            string[] powerImageNames = { "img-power-1", "img-power-2", "img-power-3" };
            string[] statImageNames = { "img-stat-effect-1", "img-stat-effect-2", "img-stat-effect-3" };

            for (int i = 0; i < powerImageNames.Length; i++)
            {
                Image powerImage = UISystem.FindImage(m_PlayerSelectPanel.gameObject, powerImageNames[i]);
                Image statImage = UISystem.FindImage(m_PlayerSelectPanel.gameObject, statImageNames[i]);
                if (powerImage != null)// && character.m_Abilities.Length > i)
                {
                    powerImage.sprite = character.m_Abilities[i].m_Icon;
                    statImage.gameObject.SetActive(true);
                    if (character.m_Abilities[i].m_Heal)
                    {
                        statImage.sprite = m_StatIcons[0];
                    }
                    else if (character.m_Abilities[i].m_Stun)
                    {
                        statImage.sprite = m_StatIcons[1];
                    }
                    else if (character.m_Abilities[i].m_Buff)
                    {
                        statImage.sprite = m_StatIcons[2];
                    }
                    else if (character.m_Abilities[i].m_Bleed)
                    {
                        statImage.sprite = m_StatIcons[3];
                    }
                    else
                    {
                        statImage.gameObject.SetActive(false);
                    }
                    if (!character.m_Unlocked)
                    {
                        powerImage.material = m_GrayscaleUIMat;
                        statImage.material = m_GrayscaleUIMat;
                    }
                    else
                    {
                        powerImage.material = null;
                        statImage.material = null;
                    }
                }
            }

            Text powerprice = UISystem.FindText(m_PlayerSelectPanel.gameObject, "txt-power3-price");
            Image power3lockImage = UISystem.FindImage(m_PlayerSelectPanel.gameObject, "img-lock-power");

            if (!character.m_Abilities[2].m_Unlocked)
            {
                if (character.m_Unlocked)
                {
                    power3lockImage.gameObject.SetActive(true);
                    powerprice.text = character.m_Abilities[2].m_PriceInGem.ToString();
                    Image powerImage = UISystem.FindImage(m_PlayerSelectPanel.gameObject, powerImageNames[2]);
                    powerImage.material = m_GrayscaleUIMat;
                }
                else
                {
                    power3lockImage.gameObject.SetActive(false);
                }
            }
            else
            {
                power3lockImage.gameObject.SetActive(false);
            }

            CharacterEquipment wpn = character.m_Weapons[character.m_WeaponNum];
            m_WeaponImage.sprite = wpn.m_Icon;
            m_SkinImage.sprite = character.m_Skins[character.m_SkinNum].m_Icon;

            if (character.m_Unlocked)
            {
                m_WeaponImage.material = null;
                m_SkinImage.material = null;
            }
            else
            {
                m_WeaponImage.material = m_GrayscaleUIMat;
                m_SkinImage.material = m_GrayscaleUIMat;
            }
        }

        private void UpdateCharacterLock(Image characterlock, Character character)
        {
            characterlock.gameObject.SetActive(!character.m_Unlocked && !character.m_UnlockedAtStart);

            if (!character.m_Unlocked && !character.m_UnlockedAtStart)
            {
                Image imagecharprice = UISystem.FindImage(m_PlayerSelectPanel.gameObject, "img-char-price");
                Text charprice = UISystem.FindText(m_PlayerSelectPanel.gameObject, "txt-char-price");

                imagecharprice.gameObject.SetActive(!character.m_SpecialPackage);
                charprice.text = character.m_PriceInCoin.ToString();
            }
        }


        private void UpdatePurchaseModeStats(Character character)
        {
            m_UpgradeButton.gameObject.SetActive(false);

            float power = character.m_Health + .05f * character.m_ItemLevel;
            power = character.m_Damage + .02f * character.m_ItemLevel;

        }

        public void BtnActivate(string itemname)
        {
            switch (itemname)
            {
                case "Character":
                    if (m_Contents.m_Characters[m_TempCharacterNums[m_TeamPlayerSelectNum]].m_SpecialPackage)
                    {
                        GameObject obj = UISystem.ShowUI("CoinShopUI");
                        obj.GetComponentInChildren<UITabControl>().SelectTab(2);
                        //UISystem.RemoveUI(gameObject);
                    }
                    else if (m_DataStorage.Coin >= m_Contents.m_Characters[m_TempCharacterNums[m_TeamPlayerSelectNum]].m_PriceInCoin)
                    {

                        UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 0, m_UITextContentsContents.m_Messages[64], null);
                        Image img = UISystem.FindImage(msg.gameObject, "MessageImage");
                        img.sprite = m_Contents.m_Characters[m_TempCharacterNums[m_TeamPlayerSelectNum]].m_Icon;
                        img.gameObject.SetActive(true);
                        msg.f_Clicked_Yes = () => PurchaseItem("Character");
                        BtnBack();

                    }
                    else
                    {
                        UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 0, m_UITextContentsContents.m_Messages[2], m_UIGraphicContents.m_Graphics[11]);
                        msg.f_Clicked_Yes = () => ShowStore(0);
                    }
                    break;

                case "Power":
                    if (m_Contents.m_Characters[m_TempCharacterNums[m_TeamPlayerSelectNum]].m_Unlocked)
                    {
                        if (!m_Contents.m_Characters[m_TempCharacterNums[m_TeamPlayerSelectNum]].m_Abilities[2].m_Unlocked)
                        {
                            if (m_DataStorage.Gem >= m_Contents.m_Characters[m_TempCharacterNums[m_TeamPlayerSelectNum]].m_Abilities[2].m_PriceInGem)
                            {

                                UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 0, m_UITextContentsContents.m_Messages[64], null);
                                Image img = UISystem.FindImage(msg.gameObject, "MessageImage");
                                img.sprite = m_Contents.m_Characters[m_TempCharacterNums[m_TeamPlayerSelectNum]].m_Abilities[2].m_Icon;
                                img.gameObject.SetActive(true);
                                msg.f_Clicked_Yes = () => PurchaseItem("Power");

                                BtnBack();
                            }

                            else
                            {
                                UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 0, m_UITextContentsContents.m_Messages[1], m_UIGraphicContents.m_Graphics[13]);
                                msg.f_Clicked_Yes = () => ShowStore(1);
                            }
                        }
                    }
                    break;

            }
        }
        public bool PurchaseItem(string itemname)
        {
            switch (itemname)
            {
                case "Character":
                    m_DataStorage.Coin -= m_Contents.m_Characters[m_TempCharacterNums[m_TeamPlayerSelectNum]].m_PriceInCoin;
                    m_Contents.m_Characters[m_TempCharacterNums[m_TeamPlayerSelectNum]].m_Unlocked = true;
                    XPBarAdd.Current.EarnXP(20);
                    m_DataStorage.SaveData();
                    StartCoroutine(Co_UnlockEffect(m_TempCharacterNums[m_TeamPlayerSelectNum]));
                    UpdateCharactersList();
                    UpdatePlayerInfo();
                    break;

                case "Power":
                    m_DataStorage.Gem -= m_Contents.m_Characters[m_TempCharacterNums[m_TeamPlayerSelectNum]].m_Abilities[2].m_PriceInGem;
                    m_Contents.m_Characters[m_TempCharacterNums[m_TeamPlayerSelectNum]].m_Abilities[2].m_Unlocked = true;
                    m_Contents.m_Achievements[4].AddCount();
                    XPBarAdd.Current.EarnXP(20);
                    m_DataStorage.SaveData();
                    //StartCoroutine(Co_UnlockEffect(m_DataStorage.m_CharactersNumber[m_TeamPlayerSelectNum].m_Abilities[2]));
                    UpdateCharactersList();
                    UpdatePlayerInfo();

                    break;

            }

            SoundGallery.PlaySound("You Win (1)");

            return true;
        }
        public void BtnUpgradeLevel()
        {
            Character character = m_Contents.m_Characters[m_TempCharacterNums[m_TeamPlayerSelectNum]];
            if (character.m_ItemLevel < 4)
            {
                int price = (character.m_ItemLevel + 1) * 5;
                if (m_DataStorage.Gem >= price)
                {
                    m_DataStorage.Gem = m_DataStorage.Gem - price;
                    character.m_ItemLevel++;
                    UpdatePlayerInfo();
                    if (character.m_ItemLevel == 4)
                    {
                        m_Contents.m_Achievements[7].AddCount();
                    }
                    m_DataStorage.SaveData();
                }
                else
                {
                    UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 0, m_UITextContentsContents.m_Messages[1], m_UIGraphicContents.m_Graphics[13]);
                    msg.f_Clicked_Yes = () => ShowStore(1);
                }
            }
        }

        public void BtnPlayerNext()
        {
            if (m_Contents.m_Characters[m_TempCharacterNums[m_TeamPlayerSelectNum]].m_Unlocked)
            {
                m_DataStorage.m_CharactersNumber[m_TeamPlayerSelectNum] = m_TempCharacterNums[m_TeamPlayerSelectNum];
            }
            else
            {
                //Debug.Log("change");
                SelectPlayer(m_DataStorage.m_CharactersNumber[m_TeamPlayerSelectNum]);
            }

            m_TeamPlayerSelectNum++;
            if (m_TeamPlayerSelectNum > 2)
            {
                m_TeamPlayerSelectNum = 0;
            }

            UISystem.m_Main.m_SquadSettings[0].m_CharacterSelectNum = m_TeamPlayerSelectNum;

            //SwitchAnimation();

            UpdatePlayerInfo();
        }

        public void BtnPlayerPrevious()
        {
            if (m_Contents.m_Characters[m_TempCharacterNums[m_TeamPlayerSelectNum]].m_Unlocked)
            {
                m_DataStorage.m_CharactersNumber[m_TeamPlayerSelectNum] = m_TempCharacterNums[m_TeamPlayerSelectNum];
            }
            else
            {
                SelectPlayer(m_DataStorage.m_CharactersNumber[m_TeamPlayerSelectNum]);
            }

            m_TeamPlayerSelectNum--;
            if (m_TeamPlayerSelectNum < 0)
            {
                m_TeamPlayerSelectNum = 2;
            }
            UISystem.m_Main.m_SquadSettings[0].m_CharacterSelectNum = m_TeamPlayerSelectNum;
            //SwitchAnimation();
            UpdatePlayerInfo();
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
            // UISystem.RemoveUI("SquadUI");
            return true;
        }
        public void BtnExit()
        {
            UISystem.ShowUI("MainMenuUI");
            UISystem.RemoveUI(gameObject);
            SoundGallery.PlaySound("pop1");
        }
        public void BtnBack()
        {
            // m_PurchasePlayerPanel.gameObject.SetActive(false);
            m_PurchaseMode = false;
            UpdatePlayerInfo();
        }


        //public void SwitchAnimation()
        //{
        //    if (m_TeamPlayerSelectNum == 0)
        //    {
        //        BaseScriptAnim.MoveFromTo(m_PlayerImages[0].transform, m_PlayerImages[0].transform.localPosition, m_PlayerImagePositions[0], .2f);
        //        BaseScriptAnim.MoveFromTo(m_PlayerImages[1].transform, m_PlayerImages[1].transform.localPosition, m_PlayerImagePositions[1], .2f);
        //        BaseScriptAnim.MoveFromTo(m_PlayerImages[2].transform, m_PlayerImages[2].transform.localPosition, m_PlayerImagePositions[2], .2f);

        //        BaseScriptAnim.Resize(m_PlayerImages[0].transform, m_PlayerImages[0].transform.localScale.x, m_PlayerImageScales[0], .2f);
        //        BaseScriptAnim.Resize(m_PlayerImages[1].transform, m_PlayerImages[1].transform.localScale.x, m_PlayerImageScales[1], .2f);
        //        BaseScriptAnim.Resize(m_PlayerImages[2].transform, m_PlayerImages[2].transform.localScale.x, m_PlayerImageScales[2], .2f);
        //    }
        //    else if (m_TeamPlayerSelectNum == 1)
        //    {
        //        BaseScriptAnim.MoveFromTo(m_PlayerImages[0].transform, m_PlayerImages[0].transform.localPosition, m_PlayerImagePositions[1], .2f);
        //        BaseScriptAnim.MoveFromTo(m_PlayerImages[1].transform, m_PlayerImages[1].transform.localPosition, m_PlayerImagePositions[0], .2f);
        //        BaseScriptAnim.MoveFromTo(m_PlayerImages[2].transform, m_PlayerImages[2].transform.localPosition, m_PlayerImagePositions[2], .2f);

        //        BaseScriptAnim.Resize(m_PlayerImages[0].transform, m_PlayerImages[0].transform.localScale.x, m_PlayerImageScales[1], .2f);
        //        BaseScriptAnim.Resize(m_PlayerImages[1].transform, m_PlayerImages[1].transform.localScale.x, m_PlayerImageScales[0], .2f);
        //        BaseScriptAnim.Resize(m_PlayerImages[2].transform, m_PlayerImages[2].transform.localScale.x, m_PlayerImageScales[2], .2f);
        //    }
        //    else if (m_TeamPlayerSelectNum == 2)
        //    {
        //        BaseScriptAnim.MoveFromTo(m_PlayerImages[0].transform, m_PlayerImages[0].transform.localPosition, m_PlayerImagePositions[1], .2f);
        //        BaseScriptAnim.MoveFromTo(m_PlayerImages[1].transform, m_PlayerImages[1].transform.localPosition, m_PlayerImagePositions[2], .2f);
        //        BaseScriptAnim.MoveFromTo(m_PlayerImages[2].transform, m_PlayerImages[2].transform.localPosition, m_PlayerImagePositions[0], .2f);

        //        BaseScriptAnim.Resize(m_PlayerImages[0].transform, m_PlayerImages[0].transform.localScale.x, m_PlayerImageScales[1], .2f);
        //        BaseScriptAnim.Resize(m_PlayerImages[1].transform, m_PlayerImages[1].transform.localScale.x, m_PlayerImageScales[2], .2f);
        //        BaseScriptAnim.Resize(m_PlayerImages[2].transform, m_PlayerImages[2].transform.localScale.x, m_PlayerImageScales[0], .2f);
        //    }
        //}

    }


}
