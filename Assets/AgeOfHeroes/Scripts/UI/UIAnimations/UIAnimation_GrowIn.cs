using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace AgeOfHeroes
{
    public class UIAnimation_GrowIn : MonoBehaviour
    {
        public UIData m_UIData;
        // Start is called before the first frame update
        public float m_AnimSpeed = 2;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Co_GrowIn());
        }

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator Co_GrowIn()
        {
            //yield return new WaitForSeconds(.1f);
            transform.localScale = Vector3.zero;
            float lerp = 0;
            while (lerp < 1)
            {
                transform.localScale = m_UIData.m_AnimInCurve_2.Evaluate(lerp) * Vector3.one;
                lerp += m_AnimSpeed * Time.deltaTime;
                yield return null;
            }
            transform.localScale = Vector3.one;
        }
    }
}