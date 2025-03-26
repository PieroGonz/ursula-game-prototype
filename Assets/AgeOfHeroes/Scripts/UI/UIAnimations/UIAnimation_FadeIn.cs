using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace AgeOfHeroes
{
    public class UIAnimation_FadeIn : MonoBehaviour
    {
        public float m_AnimSpeed = 2;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Co_FadeIn());
        }

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator Co_FadeIn()
        {
            Image img = GetComponent<Image>();
            float lerp = 0;
            Color initColor = img.color;
            while (lerp < 1)
            {
                Color c = initColor;
                c.a = lerp * c.a;
                img.color = c;
                lerp += m_AnimSpeed * Time.deltaTime;
                yield return null;
            }
            img.color = initColor;
        }
    }
}