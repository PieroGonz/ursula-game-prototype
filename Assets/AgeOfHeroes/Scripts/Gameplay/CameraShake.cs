using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes
{
    public class CameraShake : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        public void StartShake(float time)
        {
            StartCoroutine(Co_StartShake(time));
        }
        IEnumerator Co_StartShake(float time)
        {
            float lerp = 0;
            float speed = 1f / time;
            Vector3 initPosition = transform.localPosition;
            while (true)
            {
                transform.localPosition = initPosition + 20 * Mathf.Sin(Time.time * 40) * Vector3.right;
                lerp += speed * Time.deltaTime;
                if (lerp >= 1)
                    break;
                yield return null;
            }
            transform.localPosition = initPosition;
        }
    }
}