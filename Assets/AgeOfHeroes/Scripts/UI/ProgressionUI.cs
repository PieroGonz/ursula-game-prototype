using AgeOfHeroes.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace AgeOfHeroes.UI
{

    public class ProgressionUI : MonoBehaviour
    {
        public RectTransform m_ItemsPanel;

        [SerializeField]
        private GameObject m_LevelButton;

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
        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < m_Contents.m_ProgressionRewards.Length; i++)
            {
                GameObject obj = Instantiate(m_LevelButton);
                obj.transform.SetParent(m_ItemsPanel);
                obj.GetComponent<ProgressionRewardBtn>().m_Level.text = (5 * (i + 1)).ToString();
                obj.GetComponent<ProgressionRewardBtn>().m_ID = i;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void BtnBack()
        {
            UISystem.ShowUI("MainMenu");
            UISystem.RemoveUI("ProgressionUI");
        }
    }
}
