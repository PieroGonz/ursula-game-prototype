using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AgeOfHeroes.ScriptableObjects;

namespace AgeOfHeroes.UI
{
    public class GemPanel : MonoBehaviour
    {
        [SerializeField]
        private DataStorage m_DataStorage;

        public Text m_GemCount;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            m_GemCount.text = m_DataStorage.Gem.ToString();
        }
        public void ShowCoinShopUI(int num)
        {
            GameObject obj = UISystem.ShowUI("CoinShopUI");
            obj.GetComponentInChildren<UITabControl>().SelectTab(num);
            // UISystem.RemoveUI("MainMenuUI");
            //  UISystem.RemoveUI("MainMenuUI");
            //  UISystem.RemoveUI("SquadUI");
            //  UISystem.RemoveUI("StadiumUI");
            // SoundGallery.PlaySound("pop1");
            // if (MainMenuTabsUI.m_Main != null)
            // {
            //     MainMenuTabsUI.m_Main.SelectTab(3);
            //     MainMenuTabsUI.m_Main.m_CurrentTabObject.GetComponentInChildren<UITabControl>().SelectTab(2);
            // }
            // else
            // {
            //     GameObject obj = UISystem.ShowUI("StoreUI");
            //     obj.GetComponentInChildren<UITabControl>().SelectTab(2);
            //     UISystem.RemoveUI("MainMenuUI");
            //     SoundGallery.PlaySound("button32");
            // }
        }

    }
}