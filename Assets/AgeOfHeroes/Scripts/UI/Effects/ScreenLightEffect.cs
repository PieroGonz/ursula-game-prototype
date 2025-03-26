using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace AgeOfHeroes
{
    public class ScreenLightEffect : MonoBehaviour
    {
        public static ScreenLightEffect m_Main;
        public Image m_ScreenImage;
        private void Awake()
        {
            m_Main = this;
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetColor(Color c)
        {
            c.a = 0;
            m_ScreenImage.color = c;
        }

        public void StartEffect()
        {
            StartCoroutine(Co_StartEffect());
        }

        IEnumerator Co_StartEffect()
        {

            float lerp = 0;
            Color c = m_ScreenImage.color;
            while (lerp <= 1)
            {

                c.a = .6f * Mathf.Sin(lerp * Mathf.PI);
                m_ScreenImage.color = c;
                lerp += 3 * Time.deltaTime;
                yield return null;
            }

            m_ScreenImage.color = new Color(1, 1, 1, 0);
        }
    }
}