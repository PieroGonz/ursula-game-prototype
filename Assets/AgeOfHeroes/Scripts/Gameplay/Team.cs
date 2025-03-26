using AgeOfHeroes.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace AgeOfHeroes.Gameplay
{
    public class Team : MonoBehaviour
    {
        public int m_ActionType;
        public GameObject[] m_Tiles;
        public Pawn[] m_Pawns;
        public int m_ControlType;
        public int m_Deaths;

        public bool m_IsClient = true;

        public int m_TeamNum = 0;
        public int m_FaceDirection;
        public Team m_OtherTeam;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public bool AllDead()
        {
            for (int i = 0; i < 3; i++)
            {
                if (!m_Pawns[i].m_HealthControl.m_IsDead)
                    return false;
            }

            return true;
        }

        public void CheckStats()
        {
            for (int i = 0; i < 3; i++)
            {
                m_Pawns[i].CheckStats();
            }
        }

        public void CheckDeath()
        {
            for (int i = 0; i < 3; i++)
            {
                if (m_Pawns[i].gameObject.activeSelf && m_Pawns[i].m_HealthControl.m_IsDead)
                {
                    if (m_TeamNum == 1)
                    {
                        if (GameControl.m_Current.m_GameplayData.m_GameMode == GameModes.Singleplayer)
                        {
                            GameControl.m_Current.m_Contents.m_Achievements[5].AddCount();
                            GameControl.m_Current.m_DataStorage.SaveData();
                        }
                        else if (GameControl.m_Current.m_GameplayData.m_GameMode == GameModes.Multiplayer)
                        {
                            GameControl.m_Current.m_Contents.m_Achievements[6].AddCount();
                            GameControl.m_Current.m_DataStorage.SaveData();
                        }

                    }
                    m_Pawns[i].Dead();
                }
            }
        }

        public void HealTeam()
        {
            for (int i = 0; i < 3; i++)
            {
                if (m_Pawns[i].gameObject.activeSelf)
                {
                    m_Pawns[i].m_HealthControl.AddHealth(30);

                    GameObject obj = Instantiate(GameControl.m_Current.m_GameplayContents.m_HealParticle);
                    obj.transform.position = m_Pawns[i].transform.position + new Vector3(0, 0, -4);
                    Destroy(obj, 3);
                }
            }
        }

        public void ApplyDamageToPawn(int num, int damage)
        {
            m_Pawns[num].m_HealthControl.TakeDamage(damage);
        }

        public void AddStats(int num, int count)
        {
            for (int i = 0; i < 3; i++)
            {
                if (m_Pawns[i].gameObject.activeSelf)
                {
                    m_Pawns[i].AddStat(num, count);
                    if (num == 1)
                    {
                        m_Pawns[i].SetAnimReady();
                    }
                }
            }
        }
    }
}
