using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes.UI
{
    public class AlphaAnim : MonoBehaviour
    {
        public CanvasGroup m_CanvasGroup;
        [HideInInspector]
        public bool m_ChangeActiveOnEnd = false;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void StartFadeOut(bool changeActive)
        {
            m_ChangeActiveOnEnd = changeActive;
            StartCoroutine(Co_StartFadeOut());
        }

        public void StartFadeIn(bool changeActive)
        {
            m_ChangeActiveOnEnd = changeActive;
            StartCoroutine(Co_StartFadeIn());
        }

        IEnumerator Co_StartFadeOut()
        {
            m_CanvasGroup.alpha = 1;
            float fade = 1;
            while (fade > 0f)
            {
                fade -= .1f;// 6 * Time.deltaTime;
                m_CanvasGroup.alpha = fade;
                yield return null;
            }

            m_CanvasGroup.alpha = 0;

            if (m_ChangeActiveOnEnd)
            {
                gameObject.SetActive(false);
            }
        }
        IEnumerator Co_StartFadeIn()
        {
            m_CanvasGroup.alpha = 0;
            float fade = 0;
            while (fade < 1f)
            {
                fade += .1f;// 6 * Time.deltaTime;
                m_CanvasGroup.alpha = fade;
                yield return null;
            }

            m_CanvasGroup.alpha = 1;
        }
    }
}