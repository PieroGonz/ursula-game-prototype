using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgeOfHeroes.ScriptableObjects;
namespace AgeOfHeroes
{
    public class DataStorageLoader : MonoBehaviour
    {
        [SerializeField]
        private DataStorage m_DataStorage;
        [SerializeField, Space]
        private PlayerData m_PlayerData;

        // Start is called before the first frame update
        void Start()
        {
            m_DataStorage.LoadData();
            m_PlayerData.LoadData();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}