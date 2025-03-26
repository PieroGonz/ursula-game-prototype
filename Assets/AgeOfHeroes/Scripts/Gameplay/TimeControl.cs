using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgeOfHeroes.ScriptableObjects;

namespace AgeOfHeroes
{
    public class TimeControl : MonoBehaviour
    {
        [SerializeField, Space]
        private DataStorage m_DataStorage;
        [SerializeField, Space]
        private Contents m_Contents;
        [SerializeField, Space]
        private PlayerData m_PlayerData;

        float m_StartTime = 0;

        [HideInInspector]
        public System.DateTime m_DateNow;
        [HideInInspector]

        public static TimeControl m_Main;

        public TimeControlInstance[] m_TimeInstances;

        public Dictionary<string, TimeControlInstance> m_TimeList;


        void Awake()
        {
            m_Main = this;
            m_DateNow = System.DateTime.Now;
        }
        // Start is called before the first frame update
        void Start()
        {
            m_StartTime = Time.time;
            m_DateNow = System.DateTime.Now;

            m_TimeList = new Dictionary<string, TimeControlInstance>();
            for (int i = 0; i < m_TimeInstances.Length; i++)
            {
                m_TimeList.Add(m_TimeInstances[i].m_Title, m_TimeInstances[i]);
            }

        }
        void OnDestroy()
        {
        }


        public void HandleUploadData()
        {
        }
        public void Time_UpdateDataBase()
        {
            if (m_DataStorage.CheckInternet())
            {
                if (m_PlayerData.m_PlayerEmail != "" && m_PlayerData.m_PlayerPassword != "")
                {


                }
            }

        }

        // Update is called once per frame
        void Update()
        {
            m_DateNow = System.DateTime.Now;
            if (Time.time > m_StartTime + 1) //wait for 2 seconds then check
            {

            }
        }
    }
}