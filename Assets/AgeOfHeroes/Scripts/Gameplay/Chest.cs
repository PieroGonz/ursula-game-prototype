using System.Collections;
using System.Collections.Generic;
using AgeOfHeroes.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;
namespace AgeOfHeroes
{
    public class Chest : MonoBehaviour
    {
        public enum ChestTypes
        {
            m_OpenByVideo,
            m_HasVideoInside,
            m_NoVideo

        }
        public enum RewardTypes
        {
            m_Coin,
            m_Gem,
            m_NoRewards
        }
        public int m_Amount = 0;
        // Create a variable of the enum type to show as a dropdown in the inspector
        public RewardTypes m_RewardTypes;
        public ChestTypes m_ChestTypes;
        public int m_ID;

        [HideInInspector]
        public Animator m_Animator;

        public GameObject m_OpenParticle;
        public GameObject m_IdleParticle;

        [HideInInspector]
        public bool m_Opened = false;

        public bool m_GaveReward = false;

        [SerializeField, Space]
        private Contents m_Contents;
        [SerializeField, Space]
        private DataStorage m_DataStorage;
        [SerializeField, Space]
        private GameplayData m_GameplayData;

        // Start is called before the first frame update
        void Start()
        {
            m_Animator = GetComponent<Animator>();

            if (m_DataStorage.m_Chests[m_ID])
            {
                m_Animator.Play("chest-1-allreadyopen-1");
                m_IdleParticle.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }


        public void Open()
        {
            StartCoroutine(Co_OpenChest());
        }

        IEnumerator Co_OpenChest()
        {
            ExplorationCamera.m_Main.m_Target = transform;
            ExplorationCamera.m_Main.m_TargetSize = 130;

            m_Opened = true;
            m_Animator.Play("chest-1-open-1");

            yield return new WaitForSeconds(2);

            ExplorationCamera.m_Main.m_Target = ExplorationControl.m_Current.m_Pawns[0].transform;
            ExplorationCamera.m_Main.m_TargetSize = 200;

            m_DataStorage.m_Chests[m_ID] = true;
        }

        public void OpenEvent()
        {
            GameObject obj = Instantiate(m_OpenParticle);
            obj.transform.position = transform.position + new Vector3(0, 50, -4);
            m_IdleParticle.SetActive(false);
            GiveReward();

            //if (m_RewardTypes != RewardTypes.m_NoRewards)
            //{
            //    Invoke("ShowMessage", 1f);
            //}
            //else
            //{
            //    //empty, get 50 free coin by video

            //    UIMessage_B msg = UISystem.ShowMessage_B("UIMessage_B", 0, m_UITextContentsContents.m_Messages[54], null);
            //    Image img = UISystem.FindImage(msg.gameObject, "MessageImageFade_2");
            //    img.sprite = m_UIGraphicContents.m_Graphics[11];
            //    img.gameObject.SetActive(true);
            //    msg.f_Clicked_WatchVideoToUnlock = WatchCoinReward;
            //    SoundGallery.PlaySound("Click (15)");
            //}
        }


        //public bool WatchCoinReward()
        //{
        //    if (Application.platform != RuntimePlatform.Android)
        //    {
        //        HandleGetFreeCoin();
        //    }
        //    else
        //    {
        //        if (!m_DataStorage.CheckInternet())
        //        {
        //            UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 1, m_UITextContentsContents.m_Messages[0], m_UIGraphicContents.m_Graphics[4]);
        //            msg.AutoCloseMessage(1);
        //        }
        //        else
        //        {
        //            TapsellPlusControl.m_Current.m_VideoObjectsList["Tapsell_FreeCoin"].RequestVideoAd();
        //        }
        //    }
        //    return true;
        //}
        //public void HandleGetFreeCoin()
        //{
        //    m_DataStorage.Coin += 50;
        //    m_DataStorage.SaveData();
        //    UISystem.ShowCoinReward(50);
        //}
        //public void HandleGetReward()
        //{
        //        if (m_ChestTypes == ChestTypes.m_OpenByVideo)
        //        {
        //            StartCoroutine(Co_OpenChest());
        //        }
        //        else
        //        {
        //            m_DataStorage.m_Chests[m_ID] = true;
        //            if (m_RewardTypes == RewardTypes.m_Coin)
        //            {
        //                m_DataStorage.Coin += m_Amount;
        //                m_DataStorage.SaveData();
        //                UISystem.ShowCoinReward(m_Amount);
        //            }
        //            else if (m_RewardTypes == RewardTypes.m_Gem)
        //            {
        //                m_DataStorage.Gem += m_Amount;
        //                m_DataStorage.SaveData();
        //                UISystem.ShowGemReward(m_Amount);
        //            }
        //        }
        //    }


        public void GiveReward()
        {
            if (m_RewardTypes == RewardTypes.m_Coin)
            {
                m_DataStorage.Coin += m_Amount;
                m_DataStorage.SaveData();
                UISystem.ShowCoinReward(m_Amount);
            }
            else if (m_RewardTypes == RewardTypes.m_Gem)
            {
                m_DataStorage.Gem += m_Amount;
                m_DataStorage.SaveData();
                UISystem.ShowGemReward(m_Amount);
            }
            m_GaveReward = true;
        }
        //public void ShowMessage()
        //{
        //    switch (m_ChestTypes)
        //    {
        //        case ChestTypes.m_OpenByVideo:
        //        case ChestTypes.m_NoVideo:
        //            if (!m_GaveReward)
        //            {
        //                if (m_RewardTypes == RewardTypes.m_Coin)
        //                {
        //                    m_DataStorage.Coin += m_Amount;
        //                    m_DataStorage.SaveData();
        //                    UISystem.ShowCoinReward(m_Amount);
        //                }
        //                else if (m_RewardTypes == RewardTypes.m_Gem)
        //                {
        //                    m_DataStorage.Gem += m_Amount;
        //                    m_DataStorage.SaveData();
        //                    UISystem.ShowGemReward(m_Amount);
        //                }
        //                m_GaveReward = true;
        //            }
        //            break;

        //        case ChestTypes.m_HasVideoInside:

        //            UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 5, null, null);
        //            Image type = null;
        //            Image img = UISystem.FindImage(msg.gameObject, "MessageImageUnlockItems");
        //            Text pricetxt = null;
        //            string rewardType = "";

        //            switch (m_RewardTypes)
        //            {
        //                case RewardTypes.m_Coin:
        //                    rewardType = "Coin";
        //                    img.sprite = m_UIGraphicContents.m_Graphics[11];
        //                    break;
        //                case RewardTypes.m_Gem:
        //                    rewardType = "Gem";
        //                    img.sprite = m_UIGraphicContents.m_Graphics[13];
        //                    break;
        //            }

        //            if (!string.IsNullOrEmpty(rewardType))
        //            {
        //                type = UISystem.FindImage(msg.gameObject, $"{rewardType}AmountObj");
        //                type.gameObject.SetActive(true);

        //                pricetxt = UISystem.FindText(msg.gameObject, $"{rewardType}Count");
        //                pricetxt.text = m_Amount.ToString();
        //                pricetxt.gameObject.SetActive(true);
        //            }

        //            Text maintxt = UISystem.FindText(msg.gameObject, "main-txt");
        //            maintxt.text = m_UITextContentsContents.m_Messages[55];
        //            img.gameObject.SetActive(true);
        //            msg.f_Clicked_Yes = BtnVideo;
        //            break;
        //    }
        //}


    }

}