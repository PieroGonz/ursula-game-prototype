using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgeOfHeroes.ScriptableObjects;
namespace AgeOfHeroes
{
    public class BarnCustomizeSystem : MonoBehaviour
    {
        public static BarnCustomizeSystem m_Main;

        [SerializeField, Space]
        private Contents m_Contents;
        [SerializeField, Space]
        private DataStorage m_DataStorage;
        [SerializeField, Space]
        private GameplayData m_GameplayData;
        [SerializeField, Space]
        private PlayerData m_PlayerData;

        public Transform m_FlagPoint;
        public Transform m_DecorPoint;

        public GameObject m_BarnObject;
        public GameObject m_FlagObject;
        public GameObject m_DecorObject;

        [HideInInspector]
        public int m_BarnNum = 0;
        [HideInInspector]
        public int m_DecorNum = 0;
        [HideInInspector]
        public int m_FlagNum = 0;

        public bool m_UseAnimation = false;

        public int m_PlayerNumber = 0;
        void Awake()
        {
            m_Main = this;
        }
        // Start is called before the first frame update
        void Start()
        {

            //Updatelooks();
        }

        public void Updatelooks()
        {
            m_UseAnimation = false;
            UpdateBarn();
            UpdateDecor();
            UpdateFlag();
            m_UseAnimation = true;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdateBarn()
        {
            if (m_BarnObject != null)
            {
                Destroy(m_BarnObject);
            }


            //BarnSkin barn = m_Contents.m_BarnSkin[m_BarnNum];

            //m_BarnObject = Instantiate(barn.m_Prefab);
            //m_BarnObject.transform.SetParent(transform);
            //m_BarnObject.transform.localPosition = Vector3.zero;
            //m_BarnObject.transform.localScale = Vector3.one;
            //if (m_UseAnimation)
            //    StartCoroutine(Co_GrowObj(m_BarnObject));
        }
        public void UpdateFlag()
        {
            if (m_FlagObject != null)
            {
                Destroy(m_FlagObject);
            }

            //Decoratives flag = m_Contents.m_Flags[m_FlagNum];

            //m_FlagObject = Instantiate(flag.m_Prefab);
            //m_FlagObject.transform.SetParent(m_FlagPoint);
            //m_FlagObject.transform.localPosition = Vector3.zero;
            //m_FlagObject.transform.localScale = Vector3.one;
            //if (m_UseAnimation)
            //    StartCoroutine(Co_GrowObj(m_FlagObject));
        }

        public void UpdateDecor()
        {
            if (m_DecorObject != null)
            {
                Destroy(m_DecorObject);
            }

            //Decoratives decor = m_Contents.m_Decoratives[m_DecorNum];

            //m_DecorObject = Instantiate(decor.m_Prefab);
            //m_DecorObject.transform.SetParent(m_DecorPoint);
            //m_DecorObject.transform.localPosition = Vector3.zero;
            //m_DecorObject.transform.localScale = Vector3.one;
            //if (m_UseAnimation)
            //    StartCoroutine(Co_GrowObj(m_DecorObject));
        }

        IEnumerator Co_GrowObj(GameObject obj)
        {
            obj.transform.localScale = Vector3.zero;
            float lerp = 0;
            while (true)
            {
                obj.transform.localScale = lerp * Vector3.one;
                lerp += 4 * Time.deltaTime;
                yield return null;
                if (lerp >= 1)
                {
                    break;
                }
                obj.transform.localScale = Vector3.one;
            }
        }

    }
}