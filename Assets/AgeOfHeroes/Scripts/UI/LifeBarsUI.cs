using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes
{

    public class LifeBarsUI : MonoBehaviour
    {
        public static LifeBarsUI m_Main;
        [SerializeField]
        private GameObject[] LifeBarPrefab;
        [SerializeField, Space]
        public RectTransform MainCanvas;
        private void Awake()
        {
            m_Main = this;
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public LifeBar AddLifeBar(int type)
        {
            GameObject obj = Instantiate(LifeBarPrefab[type]);
            obj.GetComponent<RectTransform>().parent = MainCanvas;
            return obj.GetComponent<LifeBar>();
        }
    }
}