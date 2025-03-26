using AgeOfHeroes.Gameplay;
using AgeOfHeroes.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AgeOfHeroes.UI
{
    public class LevelLoseUI : MonoBehaviour
    {

        public GameObject m_CoinPanel;

        [HideInInspector]
        public int m_CoinNum;
        [SerializeField]
        private DataStorage m_DataStorage;
        [SerializeField]
        private GameplayData m_GameplayData;
        [SerializeField]
        private Contents m_Contents;
        [SerializeField]
        private OpponentData m_OpponentData;
        [SerializeField]
        private PlayerData m_PlayerData;
        [SerializeField, Space]
        private UIGraphicContents m_UIGraphicContents;
        [SerializeField, Space]
        private UITextContents m_UITextContentsContents;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        }

        public void BtnExit()
        {

            {
                LoadNextScene();
            }

            SoundGallery.PlaySound("button32");
        }

        private void LoadNextScene()
        {
            SceneManager.LoadScene("ExplorationScene");
        }

        public void BtnRestart()
        {

            RestartScene();

            SoundGallery.PlaySound("button32");

        }
        private void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}