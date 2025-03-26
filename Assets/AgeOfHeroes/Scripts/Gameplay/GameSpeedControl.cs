using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes
{
    public class GameSpeedControl : MonoBehaviour
    {
        public static GameSpeedControl m_Main;
        public float m_Speed = 1;

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

        public void ToggleSpeed()
        {
            if (m_Speed == 1)
            {
                m_Speed = 1.5f;
            }
            else if (m_Speed == 1.5f)
            {
                m_Speed = 2f;
            }
            else if (m_Speed == 2)
            {
                m_Speed = 1f;
            }
        }
    }
}