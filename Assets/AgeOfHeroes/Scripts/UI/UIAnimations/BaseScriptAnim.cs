using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace AgeOfHeroes
{
    public class BaseScriptAnim : MonoBehaviour
    {
        public static BaseScriptAnim m_Main;

        void Awake()
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

        public static void MoveFromTo(Transform targetTransform, Vector3 start, Vector3 end, float time)
        {
            m_Main.StartCoroutine(Co_MoveFromTo(targetTransform, start, end, time));
        }

        static IEnumerator Co_MoveFromTo(Transform targetTransform, Vector3 start, Vector3 end, float time)
        {
            float lerp = 0;
            float speed = 1f / time;

            while (true)
            {
                targetTransform.localPosition = Vector3.Lerp(start, end, lerp);
                lerp += speed * Time.deltaTime;
                if (lerp >= 1)
                    break;
                yield return null;
            }
            targetTransform.localPosition = end;
        }


        public static void MoveUIFromTo(RectTransform targetTransform, Vector2 start, Vector2 end, float time)
        {
            m_Main.StartCoroutine(Co_MoveUIFromTo(targetTransform, start, end, time));
        }

        static IEnumerator Co_MoveUIFromTo(RectTransform targetTransform, Vector2 start, Vector2 end, float time)
        {
            float lerp = 0;
            float speed = 1f / time;

            while (true)
            {
                targetTransform.anchoredPosition = Vector2.Lerp(start, end, lerp);
                lerp += speed * Time.deltaTime;
                if (lerp >= 1)
                    break;
                yield return null;
            }
            targetTransform.anchoredPosition = end;
        }

        public static void FadeImageFromTo(Image uiImage, float start, float end, float time)
        {
            m_Main.StartCoroutine(Co_FadeImageFromTo(uiImage, start, end, time));
        }

        static IEnumerator Co_FadeImageFromTo(Image uiImage, float start, float end, float time)
        {
            float lerp = 0;
            float speed = 1f / time;

            Color color = uiImage.color;

            while (true)
            {
                color.a = Mathf.Lerp(start, end, lerp);
                uiImage.color = color;
                lerp += speed * Time.deltaTime;
                if (lerp >= 1)
                    break;
                yield return null;
            }
            color.a = end;
            uiImage.color = color;
        }

        public static void Shrink(Transform targetTransform, float time)
        {
            m_Main.StartCoroutine(Co_Resize(targetTransform, 1, 0, time));
        }
        public static void Grow(Transform targetTransform, float time)
        {
            m_Main.StartCoroutine(Co_Resize(targetTransform, 0, 1, time));
        }

        public static void Resize(Transform targetTransform, float startSize, float endSize, float time)
        {
            m_Main.StartCoroutine(Co_Resize(targetTransform, startSize, endSize, time));
        }

        static IEnumerator Co_Resize(Transform targetTransform, float startSize, float endSize, float time)
        {
            float lerp = 0;
            float speed = 1f / time;
            Vector3 start = startSize * Vector3.one;
            Vector3 end = endSize * Vector3.one;
            while (true)
            {
                targetTransform.localScale = Vector3.Lerp(start, end, lerp);
                lerp += speed * Time.deltaTime;
                if (lerp >= 1)
                    break;
                yield return null;
            }
            targetTransform.localScale = end;
        }

        public static void Shake(Transform targetTransform, float time)
        {
            m_Main.StartCoroutine(Co_Shake(targetTransform, time));
        }

        static IEnumerator Co_Shake(Transform targetTransform, float time)
        {
            float lerp = 0;
            float speed = 1f / time;
            Vector3 initPosition = targetTransform.localPosition;
            while (true)
            {
                targetTransform.localPosition = initPosition + 8 * Mathf.Sin(Time.time * 60) * Vector3.right;
                lerp += speed * Time.deltaTime;
                if (lerp >= 1)
                    break;
                yield return null;
            }
            targetTransform.localPosition = initPosition;
        }
    }
}