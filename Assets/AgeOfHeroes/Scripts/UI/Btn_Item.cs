using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using AgeOfHeroes.ScriptableObjects;
namespace AgeOfHeroes.UI
{
    public class Btn_Item : MonoBehaviour
    {
        [HideInInspector]
        public int LevelNum;

        public Text m_ItemLevelNumText;
        public Text m_ItemName;
        public Image ItemImage;
        public Image LockImage;
        public Image PassedImage;

        [HideInInspector]
        public bool m_InSkinsUI = false;

        public Sprite[] BtnBackImages;

        [HideInInspector]
        public bool ItemUnlocked = false;

        // Start is called before the first frame update
        [SerializeField, Space]
        private Contents m_Contents;
        [SerializeField, Space]
        private DataStorage m_DataStorage;
        [SerializeField, Space]
        private GameplayData m_GameplayData;



        private void Awake()
        {

        }

        void Start()
        {
            Button b = GetComponent<Button>();
            b.onClick.AddListener(BtnClicked);

            Debug.Log("recieved level num" + LevelNum);

            //m_ItemLevelNumText.text = m_Contents.m_DefendEquipment[LevelNum].m_UpgradeLevel.ToString() + " ﺢﻄﺳ";

        }

        public void StartLevel_Delayed()
        {
            //SceneManager.LoadScene("LevelDesign_Sadeq");
        }
        // Update is called once per frame
        void Update()
        {
            //if (m_Contents.m_AllEquipment[LevelNum].m_Unlocked || m_Contents.m_AllEquipment[LevelNum].m_UnlockedAtStart)
            //{
            //    //Debug.Log("Unlock True");
            //    GetComponent<Image>().sprite = BtnBackImages[0];
            //    LockImage.gameObject.SetActive(false);
            //    PassedImage.gameObject.SetActive(false);
            //    ItemImage.color = Color.white;
            //    //m_ItemLevelNumText.gameObject.SetActive(true);
            //}
            //else
            //{
            //    //Debug.Log("Unlock False");
            //    GetComponent<Image>().sprite = BtnBackImages[1];
            //    ItemImage.color = new Color(.5f, .5f, .5f, 1);
            //    LockImage.gameObject.SetActive(true);
            //    PassedImage.gameObject.SetActive(false);
            //    //m_ItemLevelNumText.gameObject.SetActive(false);
            //}
        }

        public void BtnClicked()
        {
            //if (!m_InSkinsUI)
            //{
            //    ItemsPanel.Current.ShowMainItemPanel(LevelNum, 0);
            //}
            //else
            //{
            //    SkinsPanel.Current.ShowMainItemPanel(LevelNum, 0);
            //}
            //SoundGallery.PlaySound("pop1");

        }
    }
}
