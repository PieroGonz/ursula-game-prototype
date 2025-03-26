using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgeOfHeroes.ScriptableObjects;
namespace AgeOfHeroes
{
    public class MainMenuFieldsControl : MonoBehaviour
    {
        [SerializeField, Space]
        private DataStorage m_DataStorage;
        [SerializeField]
        private Contents m_Contents;
        public static MainMenuFieldsControl m_Main;

        [HideInInspector]
        public GameObject m_FieldObject;
        [HideInInspector]
        public GameObject m_Weatherbject;

        public Transform m_Camera;
        void Awake()
        {
            m_Main = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            //SetField(m_DataStorage.m_FieldNumber);
            //SetWeather(m_DataStorage.m_WeatherNumber);
        }

        // Update is called once per frame
        void Update()
        {
            m_Camera.localPosition = new Vector3(50 * Mathf.Cos(.6f * Time.time), 20 * Mathf.Sin(.3f * Time.time), -1100);
        }

        public void SetField(int num)
        {
            if (m_FieldObject != null)
            {
                Destroy(m_FieldObject);
                m_FieldObject = null;
            }

            // m_FieldObject = Instantiate(m_Contents.m_Characters[num].m_FieldPrefab);
            m_FieldObject.transform.SetParent(transform);
            m_FieldObject.transform.localPosition = Vector3.zero;
        }

        public void SetWeather(int num)
        {
            if (m_Weatherbject != null)
            {
                Destroy(m_Weatherbject);
                m_Weatherbject = null;
            }

            //m_Weatherbject = Instantiate(m_Contents.m_Characters[num].m_Prefab);
            m_Weatherbject.transform.SetParent(transform);
            m_Weatherbject.transform.localPosition = Vector3.zero;
        }
    }
}