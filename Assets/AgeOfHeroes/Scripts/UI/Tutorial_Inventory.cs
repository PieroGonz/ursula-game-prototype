using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AgeOfHeroes.ScriptableObjects;
using AgeOfHeroes.Gameplay;

namespace AgeOfHeroes.UI
{
    public class Tutorial_Inventory : MonoBehaviour
    {
        [SerializeField, Space]
        private GameplayData m_GameplayData;
        [SerializeField]
        private Contents m_Contents;
        //public InventoryUI m_InventoryUI;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Co_TutorialLoop1());

        }

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator Co_TutorialLoop1()
        {
            yield return new WaitForSeconds(1);
            //ItemsPanel itemsPanel = m_InventoryUI.m_CurrentTabControl.m_Tabs[0].GetComponent<ItemsPanel>();
            //Image back = UISystem.FindImage(gameObject, "img-background");
            //Image img, img1;

            ////tut-1 -- inventory
            //yield return new WaitForSeconds(1);
            //img = UISystem.FindImage(gameObject, "img-tut-1");
            //img.gameObject.SetActive(true);
            //yield return new WaitForSeconds(3);
            //img.gameObject.SetActive(false);

            ////tut - 2-- select block
            //img = UISystem.FindImage(gameObject, "img-tut-2");
            //img.gameObject.SetActive(true);
            //img1 = UISystem.FindImage(img.gameObject, "img-hand-1");
            //img1.transform.position = itemsPanel.m_ItemButtons[2].transform.position + new Vector3(100, -100, 0);

            //while (itemsPanel.m_ItemNum != 2)
            //{
            //    yield return null;
            //}
            //img.gameObject.SetActive(false);

            ////tut - 3-- unlock
            //img = UISystem.FindImage(gameObject, "img-tut-3");
            //img.gameObject.SetActive(true);
            //img1 = UISystem.FindImage(img.gameObject, "img-hand-1");
            //img1.transform.position = itemsPanel.m_BuyBtn.transform.position + new Vector3(170, -70, 0);

            //while (!m_Contents.m_AllEquipment[2].m_Unlocked)
            //{
            //    yield return null;
            //}
            //img.gameObject.SetActive(false);

            ////tut-1 -- inventory
            //yield return new WaitForSeconds(1);
            //img = UISystem.FindImage(gameObject, "img-tut-4");
            //img.gameObject.SetActive(true);
            //yield return new WaitForSeconds(5);
            //img.gameObject.SetActive(false);

            //gameObject.SetActive(false);
        }

    }
}