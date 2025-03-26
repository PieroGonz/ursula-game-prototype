using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes
{
    public class UiAnim_Loop_ScaleA : MonoBehaviour
    {
        [HideInInspector]
        public float m_Lerp = 0;

        public float m_LoopDelay = 1;
        public float m_Duration = 1;
        public float m_StartDelay = 1;

        public float m_Radius = .06f;

        [Range(0, 2)]
        public int m_Type = 0;

        public Coroutine m_LoopCoroutine;
        // Start is called before the first frame update
        void Start()
        {
            //m_LoopCoroutine=StartCoroutine(Co_Loop());
        }
        private void OnEnable()
        {

            m_LoopCoroutine = StartCoroutine(Co_Loop());
        }

        // Update is called once per frame
        void Update()
        {
            if (m_LoopCoroutine == null)
            {
                m_LoopCoroutine = StartCoroutine(Co_Loop());
            }

            Vector3 scale = Vector3.one;
            switch (m_Type)
            {
                case 0:
                    scale.x += m_Radius * Mathf.Sin(4 * m_Lerp * Mathf.PI);
                    break;
                case 1:
                    scale.y += m_Radius * Mathf.Cos(4 * m_Lerp * Mathf.PI);
                    break;
                case 2:
                    scale.x += m_Radius * Mathf.Sin(4 * m_Lerp * Mathf.PI);
                    scale.y += m_Radius * Mathf.Cos(4 * m_Lerp * Mathf.PI);
                    break;
            }

            transform.localScale = scale;
        }

        IEnumerator Co_Loop()
        {
            yield return new WaitForSeconds(m_StartDelay);
            while (true)
            {
                m_Lerp = 0;
                float speed = 1f / m_Duration;
                while (true)
                {
                    m_Lerp += speed * Time.deltaTime;
                    if (m_Lerp >= 1)
                        break;
                    yield return null;
                }
                m_Lerp = 0;
                yield return new WaitForSeconds(m_LoopDelay);
            }
        }
    }
}