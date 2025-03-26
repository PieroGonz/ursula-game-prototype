using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AgeOfHeroes.ScriptableObjects;
namespace AgeOfHeroes
{
    public class UITabControl : MonoBehaviour
    {

        public Image[] m_Tabs;
        public Image[] m_TabButtons;
        public int m_CurrentTab = 0;
        [SerializeField, Space]
        private Contents m_Contents;
        [SerializeField, Space]
        private PlayerData m_PlayerData;
        [SerializeField, Space]
        private DataStorage m_DataStorage;
        // Start is called before the first frame update
        void Start()
        {
            SelectTab(m_CurrentTab);

        }


        // Update is called once per frame
        void Update()
        {

        }


        public void SelectTab(int num)
        {
            for (int i = 0; i < m_Tabs.Length; i++)
            {
                m_Tabs[i].gameObject.SetActive(false);
                m_TabButtons[i].color = new Color(0.6f, 0.6f, 0.6f, 1);
            }
            m_Tabs[num].gameObject.SetActive(true);
            m_TabButtons[num].color = Color.white;
            m_CurrentTab = num;
        }


    }
}