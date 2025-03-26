using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgeOfHeroes.ScriptableObjects;
namespace AgeOfHeroes
{
    public class TimeControlInstance : MonoBehaviour
    {
        [SerializeField, Space]
        private DataStorage m_DataStorage;

        public string m_Title = "1";
        // public int m_TimeNumber = 0;
        public float m_DaysCount = 0;
        public float m_HoursCount = 0;
        public float m_MinutesCount = 0;

        [HideInInspector]
        public bool m_ReachedEnd = false;
        public bool m_AutoReset = false;

        [HideInInspector]
        public System.DateTime m_StartTime;
        [HideInInspector]
        public System.TimeSpan m_DeltaTime;

        public string m_TimeString;

        float m_DelayTime = 0;

        public delegate void Del_DBDelegate();
        public Del_DBDelegate OnTimeReached;
        // Start is called before the first frame update
        void Start()
        {
            string d = System.DateTime.Now.AddDays(-10).ToString();
            string s = PlayerPrefs.GetString(m_Title, d);
            m_TimeString = s;
            m_StartTime = System.DateTime.Parse(s);
            Save();
            m_DelayTime = Time.time;
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.time > m_DelayTime + 1) //wait for 2 seconds then check
            {
                System.DateTime endTime = m_StartTime;
                m_DeltaTime = TimeControl.m_Main.m_DateNow - m_StartTime;

                if (m_HoursCount > 0)
                    endTime = m_StartTime.AddHours(m_HoursCount);
                else if (m_DaysCount > 0)
                    endTime = m_StartTime.AddDays(m_DaysCount);
                else if (m_MinutesCount > 0)
                    endTime = m_StartTime.AddMinutes(m_MinutesCount);

                if (TimeControl.m_Main.m_DateNow > endTime)
                {
                    if (m_AutoReset)
                    {
                        OnTimeReached();
                        Reset();
                    }
                    else
                    {
                        m_ReachedEnd = true;
                    }
                }
            }
        }

        public void Reset()
        {
            m_ReachedEnd = false;
            m_StartTime = TimeControl.m_Main.m_DateNow;
            Save();
        }

        public void Save()
        {
            PlayerPrefs.SetString(m_Title, m_StartTime.ToString());
            PlayerPrefs.Save();
        }
    }
}