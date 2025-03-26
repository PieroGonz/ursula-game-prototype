using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgeOfHeroes.ScriptableObjects;
using UnityEngine.UI;
namespace AgeOfHeroes
{
    public class XPUpUI : MonoBehaviour
    {
        public Text m_CurrentLevelText;
        public Text m_NextLevelText;
        public Image m_Bar;
        public Image m_Base;

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
            m_CurrentLevelText.text = (m_PlayerData.m_PlayerLevel).ToString();
            m_NextLevelText.text = (m_PlayerData.m_PlayerLevel + 1).ToString();
            m_Base.rectTransform.anchoredPosition = new Vector2(0, 100);

            BaseScriptAnim.MoveUIFromTo(m_Base.rectTransform, new Vector2(0, 100), new Vector2(0, -160), .5f);

            int start = XPBarAdd.Current.m_LastXP;
            int end = XPBarAdd.Current.m_LastXP + XPBarAdd.Current.m_LastXPAdd;
            float startSize = ((float)start) / XPBarAdd.Current.m_MaxXP * 350;
            float endSize = ((float)end) / XPBarAdd.Current.m_MaxXP * 350;
            m_Bar.rectTransform.sizeDelta = new Vector2(startSize, 20);

            yield return new WaitForSeconds(.2f);

            float lerp = 0;
            while (true)
            {
                m_Bar.rectTransform.sizeDelta = Vector2.Lerp(new Vector2(startSize, 20), new Vector2(endSize, 20), lerp);
                lerp += .01f;
                yield return null;
                if (lerp >= 1)
                    break;
            }

            m_Bar.rectTransform.sizeDelta = new Vector2(endSize, 20);

            yield return new WaitForSeconds(.5f);
            BaseScriptAnim.MoveUIFromTo(m_Base.rectTransform, new Vector2(0, -160), new Vector2(0, 100), .5f);
            yield return new WaitForSeconds(.6f);
            UISystem.RemoveUI(gameObject);
        }
    }
}