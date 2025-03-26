using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes
{

    public class UIAnimation_Shake_A : MonoBehaviour
    {
        [SerializeField]
        private float m_Radius = 10;
        [SerializeField]
        private float m_Speed = 10;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            transform.localRotation = Quaternion.Euler(0, 0, m_Radius * Mathf.Sin(m_Speed * Time.time));
        }
    }
}
