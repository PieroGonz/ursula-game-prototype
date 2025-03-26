using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace AgeOfHeroes
{
    public class LifeBar : MonoBehaviour
    {
        [SerializeField]
        private Image Bar;
        public float Amount;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Bar.fillAmount = Amount;
            if (Amount <= 0)
            {
                Bar.fillAmount = 0;
            }
        }
    }
}
