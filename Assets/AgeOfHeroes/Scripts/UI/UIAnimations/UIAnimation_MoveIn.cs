using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace AgeOfHeroes
{
    public class UIAnimation_MoveIn : MonoBehaviour
    {
        public UIData m_UIData;
        // Start is called before the first frame update
        public float m_Time = 2;
        public Vector3 m_Offset;
        [HideInInspector]
        public Vector3 m_InitPosition;

        public bool m_AutoStart = true;
        // Start is called before the first frame update
        void Start()
        {
            m_InitPosition = transform.localPosition;
            transform.localPosition = m_InitPosition + m_Offset;

            if (m_AutoStart)
                StartMove();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void StartMove()
        {
            StopAllCoroutines();
            StartCoroutine(Co_MoveIn());

        }

        IEnumerator Co_MoveIn()
        {
            //yield return new WaitForSeconds(.1f);
            Vector3 start = m_InitPosition + m_Offset;
            Vector3 end = m_InitPosition;
            transform.localPosition = start;
            float speed = 1f / m_Time;
            float lerp = 0;
            while (lerp < 1)
            {
                transform.localPosition = Vector3.Lerp(start, end, m_UIData.m_AnimInCurve_1.Evaluate(lerp));
                lerp += speed * Time.deltaTime;
                yield return null;
            }
            transform.localPosition = end;
        }
    }
}