using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AgeOfHeroes.ScriptableObjects;

namespace AgeOfHeroes.UI
{
    public class PlayerPowersBar : MonoBehaviour
    {
        [SerializeField]
        private Contents m_Contents;
        [SerializeField]
        private DataStorage m_DataStorage;
        [SerializeField]
        private GameplayData m_GameplayData;

        public Image m_Bar;
        public Image m_SecondBar;
        public Text m_LabelText;
        public Text m_CountText;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
