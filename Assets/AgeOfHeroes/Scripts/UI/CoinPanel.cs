using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AgeOfHeroes.ScriptableObjects;

namespace AgeOfHeroes.UI
{
    public class CoinPanel : MonoBehaviour
    {
        [SerializeField]
        private DataStorage m_DataStorage;
        [SerializeField, Space]
        private GameplayData m_GameplayData;

        public Text m_CoinCount;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            m_CoinCount.text = m_DataStorage.Coin.ToString();
        }
        public void ShowCoinShopUI(int num)
        {
            GameObject obj = UISystem.ShowUI("CoinShopUI");
            obj.GetComponentInChildren<UITabControl>().SelectTab(num);

            // SoundGallery.PlaySound("pop1");

        }

    }
}