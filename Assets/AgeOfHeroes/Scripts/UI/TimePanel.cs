using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using AgeOfHeroes.ScriptableObjects;
using System;


namespace AgeOfHeroes.UI
{


    public class TimePanel : MonoBehaviour
    {
        public int m_HoursForUnlock = 6;
        [SerializeField]
        private Text m_TimeText;
        [SerializeField]
        private DataStorage m_DataStorage;


        void Start()
        {
            DateTime now = DateTime.Now;
            //m_StoppageTime = now.AddMinutes(m_HoursForUnlock);
            //m_DataStorage.m_StoppageRewardTime = m_StoppageTime.Hour;
            m_DataStorage.SaveData();

            TimeCalculation();

        }
        void Update()
        {

            TimeCalculation();

        }


        public void TimeCalculation()
        {
            //DateTime dateNow = System.DateTime.Now;
            //DateTime endTime = System.DateTime.Parse(m_DataStorage.m_StoppageRewardTime);

            //if (m_DataStorage.m_HaveTempReward)
            //{
            //    if (dateNow < endTime)
            //    {
            //        TimeSpan delta = endTime - dateNow;
            //        int hourleft = (int)delta.TotalHours;
            //        m_TimeText.text = "ﺖﻋﺎﺳ" + hourleft;
            //    }
            //}
            //else
            //{
            //    m_TimeText.text = "00:00";
            //}



        }

        public void BtnClose()
        {
            SoundGallery.PlaySound("button32");
            gameObject.SetActive(false);
        }
    }
}
