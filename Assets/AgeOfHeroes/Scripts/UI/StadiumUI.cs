using AgeOfHeroes.ScriptableObjects;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
namespace AgeOfHeroes.UI
{

    public class StadiumUI : MonoBehaviour
    {
        public RectTransform m_ItemsPanel;

        private ItemListButton[] m_Buttons;
        int m_ButtonCount = 0;
        private int m_TempItemsNum = 0;
        public GameObject m_FieldItemButton;
        public GameObject m_BallItemButton;

        public Image m_BallImage;
        public Image m_WeatherImage;

        public int m_ItemTypeNum = 0;

        [SerializeField] private Button m_FreeCoinButton;

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
            //TapsellPlusControl.m_Current.m_VideoObjectsList["Tapsell_UnlockMiniGameFootball"].OnHandleReward += HandleGetMiniGameFootball;
            //TapsellPlusControl.m_Current.m_VideoObjectsList["Tapsell_RandomRewardPack"].OnHandleReward += HandleGetFreeRandomReward;
            //TapsellPlusControl.m_Current.m_VideoObjectsList["Tapsell_RewardWheel"].OnHandleReward += HandleGetFreeRewardWheel;
            // TapsellPlusControl.m_Current.m_VideoObjectsList["Tapsell_FreeCoin"].OnHandleReward += HandleGetFreeCoin;
            //MainMenuFieldsControl.m_Main.SetField(m_DataStorage.m_FieldNumber);
            //MainMenuFieldsControl.m_Main.SetWeather(m_DataStorage.m_WeatherNumber);
            //m_BallImage.sprite = m_Contents.m_Balls[m_DataStorage.m_BallNumber].m_Icon;
            // m_WeatherImage.sprite = m_Contents.m_Weathers[m_DataStorage.m_WeatherNumber].m_Icon;
            UpdateListButtons(0);

            for (int i = 0; i < m_Contents.m_Stages.Length; i++)
            {

            }
        }


        // Update is called once per frame
        void Update()
        {
            Image imgTimebarFreeCoin = UISystem.FindImage(m_FreeCoinButton.gameObject, "img-time-bar-freecoin");
            Image imgFreeCoinVideo = UISystem.FindImage(m_FreeCoinButton.gameObject, "img-freecoin-video");
            Image imgFlashFreeCoin = UISystem.FindImage(m_FreeCoinButton.gameObject, "img-flash-freecoin");
            Text textTimeFreeCoin = UISystem.FindText(m_FreeCoinButton.gameObject, "txt-time-freecoin");

            if (m_DataStorage.VideoCountSeen >= 4)
            {
                if (TimeControl.m_Main.m_TimeList["time-free-coin-video"].m_ReachedEnd)
                {
                    Debug.Log("ReachedEnd");
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
                    int hourleft = 12 - (TimeControl.m_Main.m_TimeList["time-free-coin-video"].m_DeltaTime.Hours) - 1;
                    int minuteleft = 60 - TimeControl.m_Main.m_TimeList["time-free-coin-video"].m_DeltaTime.Minutes;

                    imgTimebarFreeCoin.gameObject.SetActive(true);
                    textTimeFreeCoin.gameObject.SetActive(true);
                    m_FreeCoinButton.interactable = false;
                    imgFlashFreeCoin.gameObject.SetActive(false);
                    m_FreeCoinButton.GetComponent<UiAnim_Loop_ScaleA>().enabled = false;
                    imgFreeCoinVideo.color = Color.gray;
                    textTimeFreeCoin.text = "ﻪﻘﯿﻗﺩ " + minuteleft + " و " + "ﺖﻋﺎﺳ " + hourleft;

                }
            }
            else
            {
                m_FreeCoinButton.interactable = true;
                imgFreeCoinVideo.color = Color.white;
                m_FreeCoinButton.GetComponent<UiAnim_Loop_ScaleA>().enabled = true;
                imgTimebarFreeCoin.gameObject.SetActive(false);
                textTimeFreeCoin.gameObject.SetActive(false);
                imgFlashFreeCoin.gameObject.SetActive(true);

                // m_FreeCoinTxt.text = "ﻮﺋﺪﯾﻭ ﯼﺎﺷﺎﻤﺗ";
                // m_FreeCoinBtn.interactable = true;
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
            m_Buttons = new ItemListButton[1];
        }

        public void UpdateListButtons(int num)
        {
            m_ItemTypeNum = num;
            ScriptableObjBase[] items = m_Contents.m_Characters;
            switch (m_ItemTypeNum)
            {
                case 0:
                    items = m_Contents.m_Characters;
                    m_ItemsPanel.sizeDelta = new Vector2(m_ItemsPanel.sizeDelta.x, 1200);
                    break;

                case 1:
                    //items = m_Contents.m_Balls;
                    m_ItemsPanel.sizeDelta = new Vector2(m_ItemsPanel.sizeDelta.x, 1350);
                    break;

                case 2:
                    items = m_Contents.m_Characters;
                    m_ItemsPanel.sizeDelta = new Vector2(m_ItemsPanel.sizeDelta.x, 600);
                    break;
            }

            RemoveButtons();
            m_ButtonCount = 0;
            m_Buttons = new ItemListButton[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                ScriptableObjBase item = items[i];
                GameObject btn = Instantiate(m_FieldItemButton);
                m_Buttons[m_ButtonCount] = btn.GetComponent<ItemListButton>();
                m_Buttons[m_ButtonCount].transform.SetParent(m_ItemsPanel);
                m_Buttons[m_ButtonCount].m_FarsiTitles.text = item.m_TitleEnglish;
                m_Buttons[m_ButtonCount].m_ItemNum = i;
                m_Buttons[m_ButtonCount].f_Clicked = SelectItem;
                Image img1;
                Text txt1;
                img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-icon");
                img1.sprite = item.m_Icon;
                if (item.m_Unlocked || item.m_UnlockedAtStart)
                    img1.color = Color.white;
                else
                    img1.color = new Color(.6f, .6f, .6f, 1);


                txt1 = UISystem.FindText(m_Buttons[m_ButtonCount].gameObject, "text-title");
                txt1.text = item.m_TitleEnglish;

                if (item.m_Unlocked || item.m_UnlockedAtStart)
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

                    if (item.m_PriceInCoin > 0)
                    {
                        img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-priceback-coin");
                        img1.gameObject.SetActive(true);

                        img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-priceback-gem");
                        img1.gameObject.SetActive(false);

                        img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-priceback-cash");
                        img1.gameObject.SetActive(false);

                        txt1 = UISystem.FindText(m_Buttons[m_ButtonCount].gameObject, "text-price-coin");
                        txt1.text = item.m_PriceInCoin.ToString();
                    }
                    else if (item.m_PriceInGem > 0)
                    {
                        img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-priceback-coin");
                        img1.gameObject.SetActive(false);

                        img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-priceback-gem");
                        img1.gameObject.SetActive(true);

                        img1 = UISystem.FindImage(m_Buttons[m_ButtonCount].gameObject, "img-priceback-cash");
                        img1.gameObject.SetActive(false);

                        txt1 = UISystem.FindText(m_Buttons[m_ButtonCount].gameObject, "text-price-gem");
                        txt1.text = item.m_PriceInGem.ToString();
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
        }

        public bool SelectField(int num, int num2 = 0)
        {
            if (m_Contents.m_Characters[num].m_Unlocked || m_Contents.m_Characters[num].m_UnlockedAtStart)
            {
                //m_DataStorage.m_FieldNumber = num;
                MainMenuFieldsControl.m_Main.SetField(num);
                Image img1;
                img1 = UISystem.FindImage(m_Buttons[num].gameObject, "img-check");
                img1.gameObject.SetActive(true);
            }
            else
            {
                if (m_Contents.m_Characters[num].m_SpecialPackage)
                {

                    GameObject obj = UISystem.ShowUI("CoinShopUI");
                    obj.GetComponentInChildren<UITabControl>().SelectTab(2);
                    UISystem.RemoveUI(gameObject);
                }
                else if (m_DataStorage.Coin >= m_Contents.m_Characters[num].m_PriceInCoin)
                {
                    m_TempItemsNum = num;
                    UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 5, null, null);
                    Image type = UISystem.FindImage(msg.gameObject, "CoinAmountObj");
                    Image img = UISystem.FindImage(msg.gameObject, "MessageImageUnlockItems");
                    Text maintxt = UISystem.FindText(msg.gameObject, "main-txt");
                    maintxt.text = m_Contents.m_Characters[num].m_TitleEnglish;
                    img.sprite = m_Contents.m_Characters[num].m_Icon;
                    type.gameObject.SetActive(true);
                    img.gameObject.SetActive(true);
                    Text pricetxt = UISystem.FindText(msg.gameObject, "CoinCount");
                    pricetxt.text = m_Contents.m_Characters[num].m_PriceInCoin.ToString();
                    pricetxt.gameObject.SetActive(true);
                    msg.f_Clicked_Yes = PurchaseStadiums;

                }
                else
                {
                    UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 0, m_UITextContentsContents.m_Messages[2], m_UIGraphicContents.m_Graphics[7]);
                    msg.f_Clicked_Yes = () => ShowStore(0);
                }
            }
            SoundGallery.PlaySound("button32");

            return true;

        }
        public bool PurchaseStadiums()
        {
            m_DataStorage.Coin -= m_Contents.m_Characters[m_TempItemsNum].m_PriceInCoin;
            m_Contents.m_Characters[m_TempItemsNum].m_Unlocked = true;
            XPBarAdd.Current.EarnXP(20);
            StartCoroutine(Co_UnlockEffect(m_TempItemsNum));
            //m_DataStorage.m_FieldNumber = m_TempItemsNum;
            m_DataStorage.SaveData();
            MainMenuFieldsControl.m_Main.SetField(m_TempItemsNum);
            UpdateListButtons(0);
            SoundGallery.PlaySound("You Win (1)");
            return true;
        }

        //public bool SelectBall(int num, int num2 = 0)
        //{
        //    if (m_Contents.m_Balls[num].m_Unlocked || m_Contents.m_Balls[num].m_UnlockedAtStart)
        //    {
        //        m_DataStorage.m_BallNumber = num;
        //        m_BallImage.sprite = m_Contents.m_Balls[num].m_Icon;
        //        UISystem.m_Main.m_SquadSettings[0].SetBall();

        //        for (int i = 0; i < m_Buttons.Length; i++)
        //        {
        //            Image img = UISystem.FindImage(m_Buttons[i].gameObject, "img-check");

        //            if (i == num)
        //            {
        //                img.gameObject.SetActive(true);
        //            }
        //            else
        //            {
        //                Image img1 = UISystem.FindImage(m_Buttons[i].gameObject, "img-check");
        //                img1.gameObject.SetActive(false);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (m_Contents.m_Balls[num].m_SpecialPackage)
        //        {

        //            GameObject obj = UISystem.ShowUI("CoinShopUI");
        //            obj.GetComponentInChildren<UITabControl>().SelectTab(2);
        //            UISystem.RemoveUI(gameObject);
        //        }
        //        else if (m_DataStorage.Gem >= m_Contents.m_Balls[num].m_PriceInGem)
        //        {
        //            m_TempItemsNum = num;
        //            UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 5, null, null);
        //            Image type = UISystem.FindImage(msg.gameObject, "GemAmountObj");
        //            Image img = UISystem.FindImage(msg.gameObject, "MessageImageUnlockItems");
        //            Text maintxt = UISystem.FindText(msg.gameObject, "main-txt");
        //            maintxt.text = m_Contents.m_Balls[num].m_TitleFarsi;
        //            img.sprite = m_Contents.m_Balls[num].m_Icon;
        //            type.gameObject.SetActive(true);
        //            img.gameObject.SetActive(true);
        //            Text pricetxt = UISystem.FindText(msg.gameObject, "GemCount");
        //            pricetxt.text = m_Contents.m_Balls[num].m_PriceInGem.ToString();
        //            pricetxt.gameObject.SetActive(true);
        //            msg.f_Clicked_Yes = PurchaseBall;
        //        }
        //        else
        //        {
        //            UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 0, m_UITextContentsContents.m_Messages[1], m_UIGraphicContents.m_Graphics[6]);
        //            msg.f_Clicked_Yes = () => ShowStore(1);
        //        }
        //    }

        //    SoundGallery.PlaySound("button32");

        //    return true;

        //}
        //public bool PurchaseBall()
        //{
        //    m_DataStorage.Gem -= m_Contents.m_Balls[m_TempItemsNum].m_PriceInGem;
        //    m_Contents.m_Balls[m_TempItemsNum].m_Unlocked = true;
        //    XPBarAdd.Current.EarnXP(20);
        //    StartCoroutine(Co_UnlockEffect(m_TempItemsNum));
        //    m_DataStorage.m_BallNumber = m_TempItemsNum;
        //    m_DataStorage.SaveData();
        //    m_BallImage.sprite = m_Contents.m_Balls[m_TempItemsNum].m_Icon;
        //    UpdateListButtons(1);
        //    SoundGallery.PlaySound("You Win (1)");

        //    return true;
        //}


        public bool SelectItem(int num, int num2 = 0)
        {
            switch (m_ItemTypeNum)
            {
                case 0:
                    SelectField(num, 0);
                    break;

                case 1:
                    //SelectBall(num, 0);
                    break;

                case 2:
                    SelectWeather(num, 0);
                    break;
            }

            return false;
        }
        public bool SelectWeather(int num, int num2 = 0)
        {
            if (m_Contents.m_Characters[num].m_Unlocked || m_Contents.m_Characters[num].m_UnlockedAtStart)
            {
                //m_DataStorage.m_WeatherNumber = num;
                m_WeatherImage.sprite = m_Contents.m_Characters[num].m_Icon;
                MainMenuFieldsControl.m_Main.SetWeather(num);

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
            }
            else
            {
                if (m_Contents.m_Characters[num].m_SpecialPackage)
                {

                    GameObject obj = UISystem.ShowUI("CoinShopUI");
                    obj.GetComponentInChildren<UITabControl>().SelectTab(2);
                    UISystem.RemoveUI(gameObject);
                }
                else if (m_DataStorage.Gem >= m_Contents.m_Characters[num].m_PriceInGem)
                {
                    m_TempItemsNum = num;
                    UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 5, null, null);
                    Image type = UISystem.FindImage(msg.gameObject, "GemAmountObj");
                    Image img = UISystem.FindImage(msg.gameObject, "MessageImageUnlockItems");
                    Text maintxt = UISystem.FindText(msg.gameObject, "main-txt");
                    maintxt.text = m_Contents.m_Characters[num].m_TitleEnglish;
                    img.sprite = m_Contents.m_Characters[num].m_Icon;
                    type.gameObject.SetActive(true);
                    img.gameObject.SetActive(true);
                    Text pricetxt = UISystem.FindText(msg.gameObject, "GemCount");
                    pricetxt.text = m_Contents.m_Characters[num].m_PriceInGem.ToString();
                    pricetxt.gameObject.SetActive(true);
                    msg.f_Clicked_Yes = PurchaseWeather;
                }
                else
                {
                    UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 0, m_UITextContentsContents.m_Messages[1], m_UIGraphicContents.m_Graphics[6]);
                    msg.f_Clicked_Yes = () => ShowStore(1);
                }
            }

            SoundGallery.PlaySound("button32");

            return true;

        }
        public bool PurchaseWeather()
        {
            m_DataStorage.Gem -= m_Contents.m_Characters[m_TempItemsNum].m_PriceInGem;
            m_Contents.m_Characters[m_TempItemsNum].m_Unlocked = true;
            XPBarAdd.Current.EarnXP(20);
            StartCoroutine(Co_UnlockEffect(m_TempItemsNum));
            //m_DataStorage.m_WeatherNumber = m_TempItemsNum;
            m_DataStorage.SaveData();
            m_WeatherImage.sprite = m_Contents.m_Characters[m_TempItemsNum].m_Icon;
            UpdateListButtons(2);
            SoundGallery.PlaySound("You Win (1)");

            return true;
        }
        public void Btn_FreeCoin()
        {
            UIMessage_B msg = UISystem.ShowMessage_B("UIMessage_B", 0, m_UITextContentsContents.m_Messages[35], null);
            Image img = UISystem.FindImage(msg.gameObject, "MessageImageFade_2");
            img.sprite = m_UIGraphicContents.m_Graphics[21];
            img.gameObject.SetActive(true);
            msg.f_Clicked_WatchVideoToUnlock = WatchCoinReward;
        }
        public bool WatchCoinReward()
        {
            if (Application.platform != RuntimePlatform.Android)
            {
                HandleGetFreeCoin();
            }

            return true;
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
            UISystem.ShowCoinReward(50);

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
            UISystem.RemoveUI("StadiumUI");
            return true;
        }
        public void BtnExit()
        {
            UISystem.ShowUI("MainMenuUI");
            UISystem.RemoveUI(gameObject);
            SoundGallery.PlaySound("pop1");
        }

    }


}
