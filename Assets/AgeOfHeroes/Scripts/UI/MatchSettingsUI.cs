using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AgeOfHeroes.ScriptableObjects;

namespace AgeOfHeroes.UI
{
    public class MatchSettingsUI : MonoBehaviour
    {
        float m_RotateSpeed = 0;
        float m_ArrowRotation = 0;

        float m_DeltaAngle = 0;
        int m_SelectField = 0;
        int m_SelectBall = 0;
        public Image m_ArrowBase1;
        public Image m_ArrowBase2;

        bool m_Spinning = false;

        int m_ItemNum = 0;
        // Start is called before the first frame update
        void Start()
        {
            m_Spinning = false;
            m_RotateSpeed = 100;
            m_SelectField = Random.Range(0, 2);
            m_SelectBall = Random.Range(0, 2);

            StartCoroutine(Co_Randomize());
        }

        // Update is called once per frame
        void Update()
        {

            if (m_Spinning)
            {
                m_ArrowRotation -= .6f * m_ArrowRotation * Time.deltaTime;
                if (m_ArrowRotation < 0)
                {
                    m_ArrowRotation = 0;
                }

                if (m_ItemNum == 0)
                    m_ArrowBase1.transform.rotation = Quaternion.Euler(0, 0, m_ArrowRotation + m_DeltaAngle);
                else
                    m_ArrowBase2.transform.rotation = Quaternion.Euler(0, 0, m_ArrowRotation + m_DeltaAngle);
            }
        }

        IEnumerator Co_Randomize()
        {
            yield return new WaitForSeconds(2);

            m_SelectField = Random.Range(0, 2);
            m_SelectBall = Random.Range(0, 2);

            if (m_SelectField == 0)
            {
                m_ArrowRotation = (4 * 360);
                m_DeltaAngle = 180;
            }
            else
            {
                m_ArrowRotation = (4 * 360);
                m_DeltaAngle = 0;
            }

            m_Spinning = true;

            while (m_ArrowRotation > 50)
            {
                yield return null;
            }

            m_Spinning = false;
            m_ItemNum = 1;
            if (m_SelectBall == 0)
            {
                m_ArrowRotation = (4 * 360);
                m_DeltaAngle = 180;
            }
            else
            {
                m_ArrowRotation = (4 * 360);
                m_DeltaAngle = 0;
            }

            yield return new WaitForSeconds(2);

            m_Spinning = true;
        }
    }
}
