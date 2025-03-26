using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AgeOfHeroes.ScriptableObjects;
namespace AgeOfHeroes
{
    public class RandomBoxUI : MonoBehaviour
    {
        [HideInInspector]
        public int m_SelectedBoxNum = 0;

        public RewardPumpkin_A[] m_RewardPumpkins;
        [HideInInspector]
        public bool m_IsSelected = false;

        public RewardData[] m_Rewards;


        [SerializeField]
        private DataStorage m_DataStorage;
        [SerializeField, Space]
        private Contents m_Contents;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Co_ActivatePumpkins());

            Shufle();

            for (int i = 0; i < 9; i++)
            {
                m_RewardPumpkins[i].m_RewardImage.sprite = m_Rewards[i].m_Icon;
            }
        }

        IEnumerator Co_ActivatePumpkins()
        {
            yield return new WaitForSeconds(.1f);
            for (int i = 0; i < m_RewardPumpkins.Length; i++)
            {
                m_RewardPumpkins[i].ActivateReward(1);
                yield return new WaitForSeconds(.1f);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void BtnBox(int num)
        {
            if (!m_IsSelected)
            {
                m_SelectedBoxNum = num;
                m_IsSelected = true;
                StartCoroutine(Co_ShowReward());
            }
        }


        IEnumerator Co_ShowReward()
        {
            m_RewardPumpkins[m_SelectedBoxNum].OpenReward();
            yield return new WaitForSeconds(2);
            GetReward();
            yield return new WaitForSeconds(2);
            UISystem.RemoveUI(gameObject);
            SoundGallery.PlaySound("pop1");
        }




        public void GetReward()
        {
            RewardData reward = m_Rewards[m_SelectedBoxNum];

            switch (reward.m_Type)
            {
                case "coin":
                    UISystem.ShowReward("coin", reward.m_Count); UISystem.ShowCoinReward(reward.m_Count);
                    m_DataStorage.Coin += reward.m_Count;
                    //m_DataStorage.UploadDataonDatabase("Coin");
                    break;

                case "gem":
                    UISystem.ShowReward("gem", reward.m_Count);
                    m_DataStorage.Gem += reward.m_Count;
                    // m_DataStorage.UploadDataonDatabase("Gem");
                    break;

            }

            m_DataStorage.m_RandomReward--;
            if (m_DataStorage.m_RandomReward < 0)
                m_DataStorage.m_RandomReward = 0;

            m_DataStorage.SaveData();
        }
        public void BtnClose()
        {
            UISystem.RemoveUI(gameObject);
            SoundGallery.PlaySound("pop1");
        }

        public void Shufle()
        {

            for (int i = 0; i < 20; i++)
            {
                int r1 = Random.Range(0, 9);
                int r2 = Random.Range(0, 9);

                RewardData a = m_Rewards[r1];
                m_Rewards[r1] = m_Rewards[r2];
                m_Rewards[r2] = a;
            }

        }
    }
}