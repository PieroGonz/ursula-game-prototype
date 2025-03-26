using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace AgeOfHeroes
{
    public class FadingText : MonoBehaviour
    {
        public Text m_Text;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Co_Fade());
        }

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator Co_Fade()
        {
            float lerp = 0;
            float speed = 2f;
            Vector3 initPosition = transform.position;
            Vector3 endPosition = initPosition + new Vector3(0, 40, 0);
            Color color1 = m_Text.color;
            Color color2 = Color.white;
            color2.a = 0;

            while (true)
            {
                transform.position = Vector3.Lerp(initPosition, endPosition, lerp);
                lerp += speed * Time.deltaTime;
                if (lerp >= 1)
                    break;
                yield return null;
            }

            lerp = 0;
            speed = 2f;
            while (true)
            {
                m_Text.color = Color.Lerp(color1, color2, lerp);
                lerp += speed * Time.deltaTime;
                if (lerp >= 1)
                    break;
                yield return null;
            }


            Destroy(gameObject);
        }
    }
}