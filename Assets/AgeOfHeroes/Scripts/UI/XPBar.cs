using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AgeOfHeroes.ScriptableObjects;
namespace AgeOfHeroes
{
    public class XPBar : MonoBehaviour
    {
        [SerializeField, Space]
        private DataStorage m_DataStorage;
        [SerializeField]
        private PlayerData m_PlayerData;
        [SerializeField]
        private Contents m_Contents;
        [SerializeField]
        private Image Bar;
        [SerializeField]
        private Image ProfileImage;
        [SerializeField]
        private Text PlayerName;
        [SerializeField]
        private Text PlayerLevel;
        public float Amount;
        [SerializeField]
        private GameObject m_EditBtn;

        [SerializeField]
        private Image m_Flash;
        public static XPBar m_Current;
        void Awake()
        {
            m_Current = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            UpdateInfo();
        }
        public void UpdateInfo()
        {
            float maxXP = XPBarAdd.Current.m_MaxXP;
            Bar.fillAmount = (float)m_DataStorage.m_PlayerXP / maxXP;

            PlayerName.text = m_PlayerData.m_PlayerName;
            PlayerLevel.text = m_PlayerData.m_PlayerLevel.ToString();
            ProfileImage.sprite = m_PlayerData.m_PlayerAvatarSprite;

        }

        // Update is called once per frame
        void Update()
        {
            ProfileImage.sprite = m_PlayerData.m_PlayerAvatarSprite;
        }
        public void XPBarClick()
        {
            if (string.IsNullOrEmpty(m_PlayerData.m_PlayerEmail) || string.IsNullOrEmpty(m_PlayerData.m_PlayerPassword))
            {
                UISystem.ShowUI("PlayerAccountUI");
            }
            else
            {
                UISystem.ShowUI("PlayerInfoUI");
            }
            //UISystem.RemoveUI("MainMenuUI");
            SoundGallery.PlaySound("pop1");
        }
    }
}