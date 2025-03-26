using AgeOfHeroes.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
namespace AgeOfHeroes
{
    public class ExplorationUI : MonoBehaviour
    {

        public static ExplorationUI m_Current;

        public Button m_ActionButton;
        public Image m_LevelPanel;
        public Image m_LevelLockedPanel;
        public Image m_ChestPanel;
        public Image m_ChestOpenedPanel;
        public Text m_LockedLevelNum;
        public Text m_LevelNum;

        public Image[] m_EnemyImages;

        public Image m_MoveHint;

        public Contents m_Content;
        private void Awake()
        {
            m_Current = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            m_LevelPanel.gameObject.SetActive(false);
            m_LevelLockedPanel.gameObject.SetActive(false);
            m_ChestPanel.gameObject.SetActive(false);
            m_ChestOpenedPanel.gameObject.SetActive(false);

            m_MoveHint.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (Joystick.GeneralJoystick.LeftStick.Hold)
            {
                m_MoveHint.gameObject.SetActive(false);
            }
            else
            {
                m_MoveHint.gameObject.SetActive(true);
            }
        }

        public void BtnLevel()
        {
            ExplorationControl.m_Current.LoadLevel();
            ExplorationControl.m_Current.m_Pawns[0].m_CanMove = false;
        }

        public void BtnChest()
        {
            HideChestPanel();
            ExplorationControl.m_Current.OpenChest();
        }

        public void BtnExit()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void ShowLevelUI()
        {
            m_LevelNum.text = (ExplorationControl.m_Current.m_LevelNum + 1).ToString();
            m_LevelPanel.gameObject.SetActive(true);

            for (int i = 0; i < 3; i++)
            {
                m_EnemyImages[i].sprite = m_Content.m_Levels[ExplorationControl.m_Current.m_LevelNum].m_EnemyCharacters[i].m_Icon;
            }

            StopAllCoroutines();
            StartCoroutine(Co_ShowLevel());
            //BaseScriptAnim.MoveFromTo(m_LevelPanel.transform,)
        }

        IEnumerator Co_ShowLevel()
        {
            float lerp = 0;
            while (lerp < 1)
            {
                m_LevelPanel.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, lerp);
                lerp += Time.deltaTime * 4;
                yield return null;
            }
            m_LevelPanel.transform.localScale = Vector3.one;
        }

        public void ShowLockedLevelUI()
        {
            m_LockedLevelNum.text = (ExplorationControl.m_Current.m_LevelNum + 1).ToString();
            m_LevelLockedPanel.gameObject.SetActive(true);
        }

        public void HideLevelUI()
        {
            m_LevelPanel.gameObject.SetActive(false);
            m_LevelLockedPanel.gameObject.SetActive(false);
        }

        public void ShowChestPanel()
        {
            m_ChestPanel.gameObject.SetActive(true);
        }

        public void ShowChestOpenedPanel()
        {
            m_ChestOpenedPanel.gameObject.SetActive(true);
        }

        public void HideChestPanel()
        {
            m_ChestPanel.gameObject.SetActive(false);
            m_ChestOpenedPanel.gameObject.SetActive(false);
        }
    }
}