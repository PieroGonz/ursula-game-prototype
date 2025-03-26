using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes
{
    public class TouchControl : MonoBehaviour
    {
        private Vector3 v_InitPosition = Vector3.zero;
        public Vector3 InitPosition
        {
            get { return v_InitPosition; }
        }
        private Vector3 v_CurrentPosition = Vector3.zero;
        public Vector3 CurrentPosition
        {
            get { return v_CurrentPosition; }
        }

        private Vector3 m_InputDirection;
        public Vector3 InputDirection
        {
            get { return m_InputDirection; }
        }

        public bool m_Touched = false;
        public bool m_Tabed = false;

        private bool m_IsTouching = false;
        public bool IsTouching
        {
            get { return m_IsTouching; }
        }

        public static TouchControl m_Current;

        void Awake()
        {
            m_Current = this;
        }

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            m_IsTouching = false;

            if (Input.GetMouseButton(0))
            {
                m_IsTouching = true;
                v_CurrentPosition = Input.mousePosition;
            }

            if (Input.touchCount == 1)
            {
                m_IsTouching = true;
                v_CurrentPosition = Input.touches[0].position;
            }

            if (IsTouching)
            {

                if (!m_Touched)
                {
                    v_InitPosition = v_CurrentPosition;
                    m_Touched = true;
                    m_Tabed = true;
                }
                else
                {
                    m_Tabed = false;
                }
                m_InputDirection = v_CurrentPosition - v_InitPosition;
                float maxDistance = 0.05f * Screen.width;
                m_InputDirection = m_InputDirection / maxDistance;
                m_InputDirection = Vector3.ClampMagnitude(m_InputDirection, 1);

                v_InitPosition = Vector3.Lerp(v_InitPosition, v_CurrentPosition, .5f * Time.deltaTime);
            }
            else
            {
                m_InputDirection = Vector3.zero;
                m_Touched = false;
                m_Tabed = false;
            }
        }
    }

}