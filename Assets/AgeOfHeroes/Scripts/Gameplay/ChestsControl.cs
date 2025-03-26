using AgeOfHeroes.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AgeOfHeroes.Chest;

namespace AgeOfHeroes
{
    public class ChestsControl : MonoBehaviour
    {
        [HideInInspector]
        public Chest m_CurrentChest;
        public static ChestsControl m_Main;
        [SerializeField, Space]
        private Contents m_Contents;
        [SerializeField, Space]
        private DataStorage m_DataStorage;
        [SerializeField, Space]
        private UIGraphicContents m_UIGraphicContents;
        [SerializeField, Space]
        private UITextContents m_UITextContentsContents;
        void Awake()
        {
            m_Main = this;
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }



        public void ShowVideoMessage()
        {
            UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 0, m_UITextContentsContents.m_Messages[56], m_UIGraphicContents.m_Graphics[14]);
            msg.f_Clicked_Yes = BtnVideo;
        }

        public void HandleGetReward()
        {

            if (m_CurrentChest.m_ChestTypes == ChestTypes.m_OpenByVideo)
            {
                m_CurrentChest.Open();
            }
            //else
            //{
            //    m_DataStorage.m_Chests[m_ID] = true;
            //    if (m_RewardTypes == RewardTypes.m_Coin)
            //    {
            //        m_DataStorage.Coin += m_Amount;
            //        m_DataStorage.SaveData();
            //        UISystem.ShowCoinReward(m_Amount);
            //    }
            //    else if (m_RewardTypes == RewardTypes.m_Gem)
            //    {
            //        m_DataStorage.Gem += m_Amount;
            //        m_DataStorage.SaveData();
            //        UISystem.ShowGemReward(m_Amount);
            //    }
            //}
        }

        public bool BtnVideo()
        {
            if (Application.platform != RuntimePlatform.Android)
            {
                HandleGetReward();
            }


            return true;
        }
    }
}