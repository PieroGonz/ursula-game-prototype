using AgeOfHeroes.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using AgeOfHeroes.ScriptableObjects;
using AgeOfHeroes.Gameplay;
using UnityEngine.SceneManagement;
using System.Security.Principal;
using System;
using UnityEngine.UI;

namespace AgeOfHeroes.Gameplay
{
    public class Tutorial : MonoBehaviour
    {
        public static Tutorial m_Current;

        [HideInInspector]
        public bool[] m_Passed = new bool[4];

        private void Awake()
        {
            m_Current = this;
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void StartTut()
        {
            StartCoroutine(Co_Tutorial());
        }

        public void StartTut_2()
        {
            StartCoroutine(Co_Tutorial_2());
        }

        public void StartTut_3()
        {
            StartCoroutine(Co_Tutorial_3());
        }

        public void StartTut_4()
        {
            StartCoroutine(Co_Tutorial_4());
        }

        IEnumerator Co_Tutorial()
        {
            yield return new WaitForSeconds(1.5f);
            GameUI.m_Current.m_TutorialPanels[0].gameObject.SetActive(true);
            yield return new WaitForSeconds(2.5f);
            GameUI.m_Current.m_TutorialPanels[0].gameObject.SetActive(false);
            GameUI.m_Current.m_TutorialPanels[1].gameObject.SetActive(true);
            yield return new WaitForSeconds(3);
            GameUI.m_Current.m_TutorialPanels[1].gameObject.SetActive(false);
            GameUI.m_Current.m_TutorialPanels[2].gameObject.SetActive(true);
            GameControl.m_Current.m_GameState = GameControl.GameStates.ChooseHero;

        }

        IEnumerator Co_Tutorial_2()
        {
            GameUI.m_Current.m_TutorialPanels[2].gameObject.SetActive(false);
            GameUI.m_Current.m_TutorialPanels[3].gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            m_Passed[0] = true;
            GameControl.m_Current.m_GameState = GameControl.GameStates.ChooseAction;
        }

        IEnumerator Co_Tutorial_3()
        {
            GameUI.m_Current.m_TutorialPanels[3].gameObject.SetActive(false);
            GameUI.m_Current.m_TutorialPanels[4].gameObject.SetActive(true);
            m_Passed[1] = true;
            yield return new WaitForSeconds(5);
            GameUI.m_Current.m_TutorialPanels[4].gameObject.SetActive(false);
            GameUI.m_Current.m_TutorialPanels[5].gameObject.SetActive(true);
            GameControl.m_Current.m_GameState = GameControl.GameStates.ChooseEnemy;
        }

        IEnumerator Co_Tutorial_4()
        {

            GameUI.m_Current.m_TutorialPanels[6].gameObject.SetActive(true);
            yield return new WaitForSeconds(4);
            GameUI.m_Current.m_TutorialPanels[6].gameObject.SetActive(false);
            GameControl.m_Current.TutorialAIAction();
            yield return new WaitForSeconds(4);
            GameUI.m_Current.m_TutorialPanels[7].gameObject.SetActive(true);
            m_Passed[2] = true;
            yield return new WaitForSeconds(4);
            GameUI.m_Current.m_TutorialPanels[7].gameObject.SetActive(false);
        }
    }
}
