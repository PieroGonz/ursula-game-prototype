using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AgeOfHeroes.UI
{
    public class SprintChargeBar : MonoBehaviour
    {
        public Image m_BarImage;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetBar(float amount)
        {
            m_BarImage.fillAmount = amount;
        }
    }
}
