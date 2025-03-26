using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes
{
    public class UIAnim_Start : MonoBehaviour
    {
        Vector3 m_OriginPosition;
        Vector3 m_StartPosition;
        public Vector3 m_StartOffset;
        public float m_Speed = 10;
        public float m_Delay = 0;
        // Start is called before the first frame update
        void Start()
        {
            m_OriginPosition = transform.localPosition;
            m_StartPosition = m_OriginPosition + m_StartOffset;
            transform.localPosition = m_StartPosition;
            Invoke("StartMove", m_Delay);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void StartMove()
        {
            StartCoroutine(Co_StartMove());
        }

        IEnumerator Co_StartMove()
        {
            float lerp = 0;
            while (lerp < 1f)
            {
                lerp += m_Speed * (1 - (lerp - .1f)) * Time.deltaTime;
                transform.localPosition = Vector3.Lerp(m_StartPosition, m_OriginPosition, lerp);
                yield return null;
            }

            transform.localPosition = m_OriginPosition;
        }
    }
}
