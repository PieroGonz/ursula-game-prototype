using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes
{
    public class SwingAngle : MonoBehaviour
    {
        [HideInInspector]
        public Quaternion m_InitRotation;
        public float m_Speed = 10;
        public float m_Range = 10;
        // Start is called before the first frame update
        void Start()
        {
            m_InitRotation = transform.localRotation;
        }

        // Update is called once per frame
        void Update()
        {
            transform.localRotation = m_InitRotation * Quaternion.Euler(0f, 0f, m_Range * Mathf.Sin(m_Speed * Time.time));
        }
    }
}