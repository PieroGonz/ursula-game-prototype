using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes
{
    public class UIAnim_StartGrow : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void StartGrow()
        {
            StartCoroutine(Co_Start());
        }

        IEnumerator Co_Start()
        {
            transform.localScale = Vector3.zero;
            float lerp = 0;
            while (lerp <= 1)
            {
                transform.localScale = lerp * Vector3.one;
                lerp += 4 * Time.deltaTime;
                yield return null;
            }

            transform.localScale = Vector3.one;

        }
    }
}