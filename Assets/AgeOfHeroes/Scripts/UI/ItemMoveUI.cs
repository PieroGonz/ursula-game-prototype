using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AgeOfHeroes
{
    public class ItemMoveUI : MonoBehaviour
    {
        public static ItemMoveUI m_Main;
        public GameObject m_CornItemPrefab;

        [SerializeField, Space]
        public RectTransform m_MainCanvas;

        public Transform m_UICornPoint;

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

        public void AddItem(Vector3 worldPos)
        {
            //GameObject obj = Instantiate(m_CornItemPrefab);
            //obj.GetComponent<RectTransform>().parent = m_MainCanvas;

            //Vector3 pos = CameraControl.Current.WorldToScreenPoint(worldPos);
            //Vector2 p2 = Vector2.zero;
            //p2.x = pos.x * m_MainCanvas.sizeDelta.x;
            //p2.y = pos.y * m_MainCanvas.sizeDelta.y;
            //obj.GetComponent<RectTransform>().anchoredPosition = p2;

        }
    }
}
