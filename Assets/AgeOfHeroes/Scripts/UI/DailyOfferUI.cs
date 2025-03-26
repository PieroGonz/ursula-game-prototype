using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AgeOfHeroes.ScriptableObjects;
using UnityEngine.SceneManagement;

namespace AgeOfHeroes.UI
{
    public class DailyOfferUI : MonoBehaviour
    {
        // Start is called before the first frame update


        [TextAreaAttribute]
        public string[] str_Details_Item1;
        public Sprite[] spr_Details_Item1;

        [TextAreaAttribute]
        public string[] str_Details_Item2;
        public Sprite[] spr_Details_Item2;
        public string[] str_RealPrice;
        public string[] str_DiscountPrice;

        private (string, Sprite, string, Sprite, string, string)[] m_OfferDetails;
        private (string, string, string, Sprite)[] m_OfferRewards;

        private int m_ItemSelectedNum;
        [SerializeField]
        private Image m_RewardPanel;
        public static DailyOfferUI Current;

        [SerializeField, Space]
        private Contents m_Contents;
        [SerializeField, Space]
        private DataStorage m_DataStorage;
        [SerializeField, Space]
        private GameplayData m_GameplayData;
        [SerializeField, Space]
        private UIGraphicContents m_UIGraphicContents;
        [SerializeField, Space]
        private UITextContents m_UITextContentsContents;
        void Awake()
        {
            Current = this;
        }
        void Start()
        {
            m_RewardPanel.gameObject.SetActive(false);
            m_ItemSelectedNum = 0;

            m_OfferDetails = new (string, Sprite, string, Sprite, string, string)[10];

            for (int i = 0; i < m_OfferDetails.Length; i++)
            {
                m_OfferDetails[i] = (str_Details_Item1[i], spr_Details_Item1[i], str_Details_Item2[i], spr_Details_Item2[i], str_RealPrice[i], str_DiscountPrice[i]);
            }

            m_OfferRewards = new (string, string, string, Sprite)[5];
            m_OfferRewards[0] = ("+ 1,500", "+ 15", str_Details_Item1[0], spr_Details_Item1[0]);
            m_OfferRewards[1] = ("+ 4,000", "+ 30", str_Details_Item1[1], spr_Details_Item1[1]);
            m_OfferRewards[2] = ("+ 7,000", "+ 60", str_Details_Item1[2], spr_Details_Item1[2]);
            m_OfferRewards[3] = ("+ 500", "+ 7", str_Details_Item1[3], spr_Details_Item1[3]);
            m_OfferRewards[4] = ("+ 9,000", "+ 70", str_Details_Item1[4], spr_Details_Item1[4]);



            FindOffer();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void FindOffer()
        {
            //main offers
            Image itemImage1 = UISystem.FindImage(gameObject, "item-img-1");
            Image itemImage2 = UISystem.FindImage(gameObject, "item-img-2");
            Text itemDesc1 = UISystem.FindText(gameObject, "item-txt-des-1");
            Text itemDesc2 = UISystem.FindText(gameObject, "item-txt-des-2");
            Text realPrice = UISystem.FindText(gameObject, "txt-real-price");
            Text discountPrice = UISystem.FindText(gameObject, "txt-discount-price");




            m_ItemSelectedNum = UnityEngine.Random.Range(0, 10);

            //switch (m_ItemSelectedNum)
            //{
            //    case 0:
            //        if (!m_Contents.m_Players[14].m_Unlocked && !m_Contents.m_Players[15].m_Unlocked && !m_Contents.m_Players[16].m_Unlocked &&
            //        !m_Contents.m_Kits[13].m_Unlocked && !m_Contents.m_Players[17].m_Unlocked && !m_Contents.m_Players[18].m_Unlocked &&
            //        !m_Contents.m_Players[19].m_Unlocked && !m_Contents.m_Kits[14].m_Unlocked)
            //        {
            //            //main offer
            //            itemImage1.sprite = m_OfferDetails[0].Item2;
            //            itemImage2.sprite = m_OfferDetails[0].Item4;
            //            itemDesc1.text = m_OfferDetails[0].Item1;
            //            itemDesc2.text = m_OfferDetails[0].Item3;
            //            realPrice.text = m_OfferDetails[0].Item5;
            //            discountPrice.text = m_OfferDetails[0].Item6;
            //        }
            //        else
            //        {
            //            FindOffer();
            //        }
            //        break;

            //    case 1:
            //        if (!m_Contents.m_Players[14].m_Unlocked && !m_Contents.m_Players[15].m_Unlocked &&
            //    !m_Contents.m_Players[16].m_Unlocked && !m_Contents.m_Kits[13].m_Unlocked)
            //        {
            //            itemImage1.sprite = m_OfferDetails[1].Item2;
            //            itemImage2.sprite = m_OfferDetails[1].Item4;
            //            itemDesc1.text = m_OfferDetails[1].Item1;
            //            itemDesc2.text = m_OfferDetails[1].Item3;
            //            realPrice.text = m_OfferDetails[1].Item5;
            //            discountPrice.text = m_OfferDetails[1].Item6;
            //        }
            //        else
            //        {
            //            FindOffer();
            //        }
            //        break;

            //    case 2:
            //        if (!m_Contents.m_Players[17].m_Unlocked && !m_Contents.m_Players[18].m_Unlocked &&
            //        !m_Contents.m_Players[19].m_Unlocked && !m_Contents.m_Kits[14].m_Unlocked && !m_DataStorage.m_DailyOfferRecieved)
            //        {
            //            itemImage1.sprite = m_OfferDetails[2].Item2;
            //            itemImage2.sprite = m_OfferDetails[2].Item4;
            //            itemDesc1.text = m_OfferDetails[2].Item1;
            //            itemDesc2.text = m_OfferDetails[2].Item3;
            //            realPrice.text = m_OfferDetails[2].Item5;
            //            discountPrice.text = m_OfferDetails[2].Item6;
            //        }
            //        else
            //        {
            //            FindOffer();
            //        }
            //        break;

            //    case 3:
            //        if (!m_DataStorage.m_CustomizeProfilePic)
            //        {
            //            itemImage1.sprite = m_OfferDetails[3].Item2;
            //            itemImage2.sprite = m_OfferDetails[3].Item4;
            //            itemDesc1.text = m_OfferDetails[3].Item1;
            //            itemDesc2.text = m_OfferDetails[3].Item3;
            //            realPrice.text = m_OfferDetails[3].Item5;
            //            discountPrice.text = m_OfferDetails[3].Item6;
            //        }
            //        else
            //        {
            //            FindOffer();
            //        }
            //        break;

            //    case 4:
            //        //if (!m_Contents.m_Balls[5].m_Unlocked && !m_Contents.m_Stadiums[3].m_Unlocked)
            //        //{
            //        //    itemImage1.sprite = m_OfferDetails[4].Item2;
            //        //    itemImage2.sprite = m_OfferDetails[4].Item4;
            //        //    itemDesc1.text = m_OfferDetails[4].Item1;
            //        //    itemDesc2.text = m_OfferDetails[4].Item3;
            //        //    realPrice.text = m_OfferDetails[4].Item5;
            //        //    discountPrice.text = m_OfferDetails[4].Item6;
            //        //}
            //        //else
            //        //{
            //        //    FindOffer();
            //        //}
            //        break;

            //    case 5:

            //        itemImage1.sprite = m_OfferDetails[5].Item2;
            //        itemImage2.sprite = m_OfferDetails[5].Item4;
            //        itemDesc1.text = m_OfferDetails[5].Item1;
            //        itemDesc2.text = m_OfferDetails[5].Item3;
            //        realPrice.text = m_OfferDetails[5].Item5;
            //        discountPrice.text = m_OfferDetails[5].Item6;
            //        break;

            //    case 6:

            //        itemImage1.sprite = m_OfferDetails[6].Item2;
            //        itemImage2.sprite = m_OfferDetails[6].Item4;
            //        itemDesc1.text = m_OfferDetails[6].Item1;
            //        itemDesc2.text = m_OfferDetails[6].Item3;
            //        realPrice.text = m_OfferDetails[6].Item5;
            //        discountPrice.text = m_OfferDetails[6].Item6;
            //        break;
            //    case 7:

            //        itemImage1.sprite = m_OfferDetails[7].Item2;
            //        itemImage2.sprite = m_OfferDetails[7].Item4;
            //        itemDesc1.text = m_OfferDetails[7].Item1;
            //        itemDesc2.text = m_OfferDetails[7].Item3;
            //        realPrice.text = m_OfferDetails[7].Item5;
            //        discountPrice.text = m_OfferDetails[7].Item6;
            //        break;
            //    case 8:

            //        itemImage1.sprite = m_OfferDetails[8].Item2;
            //        itemImage2.sprite = m_OfferDetails[8].Item4;
            //        itemDesc1.text = m_OfferDetails[8].Item1;
            //        itemDesc2.text = m_OfferDetails[8].Item3;
            //        realPrice.text = m_OfferDetails[8].Item5;
            //        discountPrice.text = m_OfferDetails[8].Item6;
            //        break;
            //    case 9:

            //        itemImage1.sprite = m_OfferDetails[9].Item2;
            //        itemImage2.sprite = m_OfferDetails[9].Item4;
            //        itemDesc1.text = m_OfferDetails[9].Item1;
            //        itemDesc2.text = m_OfferDetails[9].Item3;
            //        realPrice.text = m_OfferDetails[9].Item5;
            //        discountPrice.text = m_OfferDetails[9].Item6;
            //        break;

            //}

            //Debug.Log("m_ItemSelectedNum: " + m_ItemSelectedNum);
        }

        public void BtnBuy()
        {
            //rewards
            // Image rewardItemImage = UISystem.FindImage(gameObject, "img-reward-3");
            // Text rewardCoinAmount = UISystem.FindText(gameObject, "txt-reward-1");
            // Text rewardGemAmount = UISystem.FindText(gameObject, "txt-reward-2");
            // Text rewardItemDesc = UISystem.FindText(gameObject, "txt-reward-3");


            if (!m_DataStorage.CheckInternet())
            {
                UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 1, m_UITextContentsContents.m_Messages[0], m_UIGraphicContents.m_Graphics[4]);
                msg.AutoCloseMessage(1);
            }
            else
            {
                if (Application.platform != RuntimePlatform.Android)
                {
                    //test in editor
                    TimeControl.m_Main.m_TimeInstances[4].Reset();
                    HandleRewards(m_ItemSelectedNum);
                    // Debug.Log("ItemSelectedNum" + m_ItemSelectedNum);

                }
                else
                {
#if UNITY_ANDROID


#endif

                }
            }
        }
        public void HandleRewards(int rewardNum)
        {
            TimeControl.m_Main.m_TimeInstances[4].Reset();
            //rewards
            Image rewardItem1Img = UISystem.FindImage(gameObject, "img-reward-1");
            Text rewardItem1Txt = UISystem.FindText(gameObject, "txt-reward-1");
            Image rewardItem2Img = UISystem.FindImage(gameObject, "img-reward-2");
            Text rewardItem2Txt = UISystem.FindText(gameObject, "txt-reward-2");

            //switch (rewardNum)
            //{
            //    case 0:
            //        m_DataStorage.Coin += 5000;
            //        //esteghlal pack
            //        m_Contents.m_Players[14].m_Unlocked = true;
            //        m_Contents.m_Players[15].m_Unlocked = true;
            //        m_Contents.m_Players[16].m_Unlocked = true;
            //        m_Contents.m_Kits[13].m_Unlocked = true;

            //        //perspolis pack
            //        m_Contents.m_Players[17].m_Unlocked = true;
            //        m_Contents.m_Players[18].m_Unlocked = true;
            //        m_Contents.m_Players[19].m_Unlocked = true;
            //        m_Contents.m_Kits[14].m_Unlocked = true;


            //        m_DataStorage.m_DailyOfferRecieved = true;
            //        m_DataStorage.SaveData();
            //        // m_DataStorage.UploadDataonDatabase("Coin");

            //        //rewards

            //        m_RewardPanel.gameObject.SetActive(true);
            //        rewardItem1Img.sprite = m_OfferDetails[0].Item2;
            //        rewardItem1Txt.text = m_OfferDetails[0].Item1;
            //        rewardItem2Img.sprite = m_OfferDetails[0].Item4;
            //        rewardItem2Txt.text = m_OfferDetails[0].Item3;
            //        break;

            //    case 1:
            //        m_DataStorage.Coin += 5000;
            //        //esteghlal pack
            //        m_Contents.m_Players[14].m_Unlocked = true;
            //        m_Contents.m_Players[15].m_Unlocked = true;
            //        m_Contents.m_Players[16].m_Unlocked = true;
            //        m_Contents.m_Kits[13].m_Unlocked = true;

            //        m_DataStorage.m_DailyOfferRecieved = true;
            //        m_DataStorage.SaveData();
            //        // m_DataStorage.UploadDataonDatabase("Coin");
            //        //AppMetricaLogingEvents.m_Current.SendPurchasedEquipment(19);

            //        //rewards

            //        m_RewardPanel.gameObject.SetActive(true);
            //        rewardItem1Img.sprite = m_OfferDetails[1].Item2;
            //        rewardItem1Txt.text = m_OfferDetails[1].Item1;
            //        rewardItem2Img.sprite = m_OfferDetails[1].Item4;
            //        rewardItem2Txt.text = m_OfferDetails[1].Item3;
            //        break;
            //    case 2:
            //        m_DataStorage.Coin += 5000;
            //        //perspolis pack
            //        m_Contents.m_Players[17].m_Unlocked = true;
            //        m_Contents.m_Players[18].m_Unlocked = true;
            //        m_Contents.m_Players[19].m_Unlocked = true;
            //        m_Contents.m_Kits[14].m_Unlocked = true;

            //        m_DataStorage.m_DailyOfferRecieved = true;
            //        m_DataStorage.SaveData();
            //        // m_DataStorage.UploadDataonDatabase("Coin");

            //        //rewards

            //        m_RewardPanel.gameObject.SetActive(true);
            //        rewardItem1Img.sprite = m_OfferDetails[2].Item2;
            //        rewardItem1Txt.text = m_OfferDetails[2].Item1;
            //        rewardItem2Img.sprite = m_OfferDetails[2].Item4;
            //        rewardItem2Txt.text = m_OfferDetails[2].Item3;
            //        break;
            //    case 3:
            //        m_DataStorage.Gem += 50;
            //        m_DataStorage.m_CustomizeProfilePic = true;
            //        m_DataStorage.m_DailyOfferRecieved = true;
            //        m_DataStorage.SaveData();
            //        // m_DataStorage.UploadDataonDatabase("Gem");
            //        //rewards

            //        m_RewardPanel.gameObject.SetActive(true);
            //        rewardItem1Img.sprite = m_OfferDetails[3].Item2;
            //        rewardItem1Txt.text = m_OfferDetails[3].Item1;
            //        rewardItem2Img.sprite = m_OfferDetails[3].Item4;
            //        rewardItem2Txt.text = m_OfferDetails[3].Item3;
            //        break;
            //    case 4:
            //        //m_Contents.m_Balls[5].m_Unlocked = true;
            //        m_Contents.m_Stadiums[3].m_Unlocked = true;

            //        m_DataStorage.m_DailyOfferRecieved = true;
            //        m_DataStorage.SaveData();

            //        //rewards

            //        m_RewardPanel.gameObject.SetActive(true);
            //        rewardItem1Img.sprite = m_OfferDetails[4].Item2;
            //        rewardItem1Txt.text = m_OfferDetails[4].Item1;
            //        rewardItem2Img.sprite = m_OfferDetails[4].Item4;
            //        rewardItem2Txt.text = m_OfferDetails[4].Item3;
            //        break;
            //    case 5:
            //        m_DataStorage.Coin += 5000;
            //        m_DataStorage.Gem += 75;
            //        m_DataStorage.m_DailyOfferRecieved = true;
            //        m_DataStorage.SaveData();
            //        // m_DataStorage.UploadDataonDatabase("Coin");
            //        UISystem.ShowCoinGemReward(5000, 75);
            //        UISystem.RemoveUI("DailyOfferUI");
            //        break;
            //    case 6:
            //        m_DataStorage.Coin += 10000;
            //        m_DataStorage.Gem += 75;
            //        m_DataStorage.m_DailyOfferRecieved = true;
            //        m_DataStorage.SaveData();
            //        //m_DataStorage.UploadDataonDatabase("Coin");
            //        UISystem.ShowCoinGemReward(10000, 75);
            //        UISystem.RemoveUI("DailyOfferUI");
            //        break;
            //    case 7:
            //        m_DataStorage.Coin += 3000;
            //        m_DataStorage.PowerCounts[0] += 10;
            //        m_DataStorage.m_DailyOfferRecieved = true;
            //        m_DataStorage.SaveData();

            //        UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 1, m_UITextContentsContents.m_Messages[51], m_UIGraphicContents.m_Graphics[15]);

            //        UISystem.RemoveUI("DailyOfferUI");
            //        break;
            //    case 8:
            //        m_DataStorage.Coin += 10000;
            //        m_DataStorage.PowerCounts[1] += 10;
            //        m_DataStorage.m_DailyOfferRecieved = true;
            //        m_DataStorage.SaveData();
            //        UIMessage_A msg1 = UISystem.ShowMessage("UIMessage_A", 1, m_UITextContentsContents.m_Messages[51], m_UIGraphicContents.m_Graphics[15]);

            //        UISystem.RemoveUI("DailyOfferUI");
            //        break;
            //    case 9:
            //        m_DataStorage.PowerCounts[2] += 10;
            //        m_DataStorage.Gem += 30;
            //        m_DataStorage.m_DailyOfferRecieved = true;
            //        m_DataStorage.SaveData();
            //        UIMessage_A msg2 = UISystem.ShowMessage("UIMessage_A", 1, m_UITextContentsContents.m_Messages[52], m_UIGraphicContents.m_Graphics[15]);

            //        UISystem.RemoveUI("DailyOfferUI");
            //        break;
            //}
        }

        public void BtnBack()
        {
            UISystem.RemoveUI(gameObject);
            SoundGallery.PlaySound("button32");
        }
    }

}
