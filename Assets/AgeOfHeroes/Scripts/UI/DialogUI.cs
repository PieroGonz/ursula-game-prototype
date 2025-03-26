using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes
{
    public class DialogUI : MonoBehaviour
    {
        public static DialogUI m_Main;

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

        public void Show()
        {
            GetComponent<Animator>().SetTrigger("Show");
        }
    }
}