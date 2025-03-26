using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace AgeOfHeroes
{
    public class ItemListButton : MonoBehaviour
    {
        [HideInInspector]
        public int m_ItemNum;
        [HideInInspector]
        public int m_ItemNum2;

        [HideInInspector]
        public bool m_IsSelected = false;

        public Material m_GrayscaleMat;

        public Image m_MainBody;
        public Image m_Level;
        public Text m_FarsiTitles;
        public Sprite[] m_MainBodySprites;
        public Func<int, int, bool> f_Clicked;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //if (m_IsSelected)
            //{
            //    m_MainBody.sprite = m_MainBodySprites[1];
            //}
            //else
            //{
            //    m_MainBody.sprite = m_MainBodySprites[0];
            //}
        }

        public void BtnClicked()
        {
            f_Clicked(m_ItemNum, m_ItemNum2);
        }
    }
}