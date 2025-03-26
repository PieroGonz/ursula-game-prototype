using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgeOfHeroes.ScriptableObjects;
using UnityEngine.UI;
namespace AgeOfHeroes
{
    public class LevelUpUI : MonoBehaviour
    {
        public Text m_CurrentLevelText;
        public Text m_NextLevelText;

        [SerializeField]
        private DataStorage m_DataStorage;

        [SerializeField]
        private PlayerData m_PlayerData;
        // Start is called before the first frame update
        void Start()
        {
            StartUI();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void StartUI()
        {
            StartCoroutine(Co_StartUI());
        }

        IEnumerator Co_StartUI()
        {
            m_CurrentLevelText.text = (m_PlayerData.m_PlayerLevel - 1).ToString();
            m_NextLevelText.text = m_PlayerData.m_PlayerLevel.ToString();
            m_DataStorage.m_RandomReward = 1;
            m_DataStorage.SaveData();

            m_NextLevelText.gameObject.SetActive(false);
            m_CurrentLevelText.gameObject.SetActive(true);

            yield return new WaitForSeconds(1);
            m_CurrentLevelText.gameObject.SetActive(false);
            m_NextLevelText.gameObject.SetActive(true);
            BaseScriptAnim.Grow(m_NextLevelText.transform, 1);

            yield return new WaitForSeconds(4);
            UISystem.RemoveUI(gameObject);
        }
    }
}