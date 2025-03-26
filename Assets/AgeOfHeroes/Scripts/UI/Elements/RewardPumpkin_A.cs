using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AgeOfHeroes.ScriptableObjects;
namespace AgeOfHeroes
{
    public class RewardPumpkin_A : MonoBehaviour
    {
        //public Func<bool> f_;

        public Image m_MainImage;
        public Image m_RewardImage;
        public Image m_light;
        public ParticleSystem[] m_Particles;

        public bool m_IsActivated = false;
        // Start is called before the first frame update
        void Start()
        {
            m_light.gameObject.SetActive(false);
            m_MainImage.color = new Color(0, 0, 0, .2f);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ActivateReward(int type = 0)
        {
            m_IsActivated = true;
            if (type == 0)
            {
                GetComponent<AudioSource>().Play();
                m_light.gameObject.SetActive(true);
            }
            m_MainImage.color = Color.white;
            m_Particles[0].Play();
            GetComponent<Animator>().SetTrigger("activate");
        }

        public void OpenReward()
        {
            GetComponent<Animator>().SetTrigger("open");
            //m_Particles[1].Play();
        }
    }
}
