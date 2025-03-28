using AgeOfHeroes.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes
{
    public class ExplorationCamera : MonoBehaviour
    {
        public Transform m_Target;
        public static ExplorationCamera m_Main;

        public float m_TargetSize = 100;

        public Camera m_Camera;

        private void Awake()
        {
            m_Main = this;
            m_Camera = GetComponent<Camera>();
        }
        // Start is called before the first frame update
        void Start()
        {
            m_TargetSize = 150;
        }

        void Update()
        {
            m_Camera.orthographicSize = Mathf.Lerp(m_Camera.orthographicSize, m_TargetSize, 5 * Time.deltaTime);

            Vector3 pos = m_Target.position;
            pos.z = -1000;
            pos.y += 50;
            transform.position = Vector3.Lerp(transform.position, pos, 10 * Time.deltaTime);

        }

        // Update is called once per frame

    }
}