using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes
{
    public class UIAnimation_Shake_B : MonoBehaviour
    {
        [SerializeField]
        private float m_Radius = 0.1f;
        [SerializeField]
        private float m_Speed = 10;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            transform.localScale = (1 + m_Radius * Mathf.Sin(m_Speed * Time.time)) * Vector3.one;
        }
    }
}
