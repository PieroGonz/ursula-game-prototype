using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using AgeOfHeroes.ScriptableObjects;
using AgeOfHeroes.Gameplay;
namespace AgeOfHeroes.UI
{


    public class PauseUI : MonoBehaviour
    {
        [SerializeField]
        private DataStorage m_DataStorage;
        [SerializeField]
        private GameplayData m_GameplayData;

        void Start()
        {

        }

        void Update()
        {



        }
        public void BtnContinue()
        {
            m_GameplayData.m_PauseState = false;
            GameControl.m_Current.ResumeGame();
            UISystem.RemoveUI(gameObject);
            SoundGallery.PlaySound("button32");
        }

        void LoadMainMenuScene()
        {
            SceneManager.LoadScene("MainMenu");
        }
        public void BtnExit()
        {
            m_GameplayData.m_PauseState = false;
            Time.timeScale = 1;
            if (!m_DataStorage.m_NotFirstTimeInGame)
            {
                LoadMainMenuScene();
            }

            else
            {
                LoadMainMenuScene();
            }

            SoundGallery.PlaySound("button32");
        }
        public void BtnRetry()
        {
            m_GameplayData.m_PauseState = false;
            GameControl.m_Current.RestartLevel();
            Time.timeScale = 1;

            SoundGallery.PlaySound("button32");

        }

    }
}
