using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes
{
    public class RandomShake : MonoBehaviour
    {
        // Start is called before the first frame update
        public float m_Radius = 10;
        public float m_Speed = 1;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Vector2 offset = Vector2.zero;
            offset.x = m_Radius * Mathf.Cos(m_Speed * 10f * Time.time);
            offset.y = m_Radius * Mathf.Sin(m_Speed * 5f * Time.time);

            transform.localPosition = offset;
        }
    }
}