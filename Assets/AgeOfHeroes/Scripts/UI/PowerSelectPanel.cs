using AgeOfHeroes.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AgeOfHeroes.UI
{
    public class PowerSelectPanel : MonoBehaviour
    {
        public int[] m_SelectedPowers;
        public Image[] m_PowerButtons;

        public Text[] m_PowerCounts;

        int m_Last = 0;

        [SerializeField, Space]
        private Contents m_Contents;
        [SerializeField, Space]
        private DataStorage m_DataStorage;
        // Start is called before the first frame update
        void Start()
        {
            m_SelectedPowers = new int[2];
            m_SelectedPowers[0] = -1;
            m_SelectedPowers[1] = -1;
            //m_DataStorage.PowerNums = new int[2];
            //m_DataStorage.PowerNums[0] = -1;
            //m_DataStorage.PowerNums[1] = -1;

            //for (int i = 0; i < m_PowerButtons.Length; i++)
            //{
            //    Image frameObj = UISystem.FindImage(m_PowerButtons[i].gameObject, "img-frame");
            //    frameObj.gameObject.SetActive(false);
            //}
        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < m_PowerButtons.Length; i++)
            {
                m_PowerCounts[i].text = m_DataStorage.PowerCounts[i].ToString();
            }
        }

        public void BtnPower(int num)
        {
            GameObject obj = UISystem.ShowUI("CoinShopUI");
            obj.GetComponentInChildren<UITabControl>().SelectTab(3);
            //if (m_DataStorage.PowerCounts[num] <= 0)
            //    return;


            //if (m_SelectedPowers[0] == num || m_SelectedPowers[1] == num)
            //    return;

            //if (m_SelectedPowers[0] == -1)
            //{
            //    m_SelectedPowers[0] = num;
            //    m_Last = 0;
            //}
            //else if (m_SelectedPowers[1] == -1)
            //{
            //    m_SelectedPowers[1] = num;
            //    m_Last = 1;
            //}
            //else
            //{
            //    if (m_Last == 0)
            //    {
            //        m_SelectedPowers[1] = num;
            //        m_Last = 1;
            //    }
            //    else
            //    {
            //        m_SelectedPowers[0] = num;
            //        m_Last = 0;
            //    }
            //}

            //for (int i = 0; i < m_PowerButtons.Length; i++)
            //{
            //    Image frameObj = UISystem.FindImage(m_PowerButtons[i].gameObject, "img-frame");

            //    if (m_SelectedPowers[0] == i)
            //    {
            //        frameObj.gameObject.SetActive(true);
            //    }
            //    else if (m_SelectedPowers[1] == i)
            //    {
            //        frameObj.gameObject.SetActive(true);
            //    }
            //    else
            //    {
            //        frameObj.gameObject.SetActive(false);
            //    }
            //}

            //m_DataStorage.PowerNums[0] = m_SelectedPowers[0];
            //m_DataStorage.PowerNums[1] = m_SelectedPowers[1];

            SoundGallery.PlaySound("Btn1");
        }
        public void BtnAdd()
        {
            GameObject obj1 = UISystem.ShowUI("CoinShopUI");
            obj1.GetComponentInChildren<UITabControl>().SelectTab(3);
            UISystem.RemoveUI(gameObject);
        }
    }
}