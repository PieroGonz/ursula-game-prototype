
using AgeOfHeroes.Gameplay;
using AgeOfHeroes.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;
namespace AgeOfHeroes
{
    public class ExplorationControl : MonoBehaviour
    {

        public static ExplorationControl m_Current;

        [HideInInspector]
        public ExplorationPawn[] m_Pawns;

        public GameObject m_ExplorationPawnPrefab;

        public Level[] m_LevelData;
        public LevelPoint[] m_LevelPoints;

        public int m_LevelNum = 0;

        [HideInInspector]
        public Chest m_CurrentChest = null;

        public GameplayData m_GameplayData;
        public Contents m_Content;
        [SerializeField]
        private DataStorage m_DataStorage;
        [SerializeField, Space]
        private UIGraphicContents m_UIGraphicContents;
        [SerializeField, Space]
        private UITextContents m_UITextContentsContents;
        private void Awake()
        {
            m_Current = this;
            for (int i = 0; i < m_LevelData.Length; i++)
            {
                m_LevelPoints[i].m_Level = m_LevelData[i];

            }
        }
        // Start is called before the first frame update
        void Start()
        {

            UpdateCharactersPosition();

        }


        // Update is called once per frame
        void Update()
        {

        }
        public void UpdateCharactersPosition()
        {
            Vector3 levelPos = m_LevelPoints[m_DataStorage.m_LastLevelNum].transform.position + new Vector3(-100, -60, 0);
            // if (m_DataStorage.m_ShowWinLevel && m_DataStorage.m_LastLevelNum % 10 != 0)
            // {
            //     levelPos = m_LevelPoints[m_DataStorage.m_LastLevelNum - 1].transform.position + new Vector3(-100, -60, 0);
            //     m_DataStorage.m_ShowWinLevel = false;
            // }

            if (m_DataStorage.m_LastLevelNum == 0)
            {
                levelPos = m_LevelPoints[0].transform.position + new Vector3(-200, -100, 0);
            }

            m_Pawns = new ExplorationPawn[3];
            for (int i = 0; i < 3; i++)
            {
                Character C = m_Content.m_Characters[m_DataStorage.m_CharactersNumber[i]];
                GameObject body = Instantiate(C.m_Skins[C.m_SkinNum].m_BaseBodyPrefab);
                body.GetComponent<CharacterBody>().SetMainWeapon(C.m_Weapons[C.m_WeaponNum].m_BodySprite);
                GameObject charObj = Instantiate(m_ExplorationPawnPrefab);
                body.transform.SetParent(charObj.transform, false);
                charObj.transform.position = levelPos;
                charObj.GetComponent<ExplorationPawn>().m_Animator = body.GetComponent<Animator>();
                m_Pawns[i] = charObj.GetComponent<ExplorationPawn>();
                m_Pawns[i].m_PawnNum = i;
            }
            m_Pawns[0].m_ControledByPlayer = true;
            m_Pawns[0].m_LeaderPawn = null;
            m_Pawns[1].m_LeaderPawn = m_Pawns[0];
            m_Pawns[2].m_LeaderPawn = m_Pawns[0];
            m_Pawns[1].m_Speed = 60;
            m_Pawns[2].m_Speed = 60;
            ExplorationCamera.m_Main.m_Target = m_Pawns[0].transform;

            Vector3 pos = m_Pawns[0].transform.position;
            pos.z = -1000;
            pos.y += 50;
            ExplorationCamera.m_Main.transform.position = pos;

        }
        public void OpenChest()
        {
            if (m_CurrentChest != null)
            {
                ChestsControl.m_Main.m_CurrentChest = m_CurrentChest;
                if (m_CurrentChest.m_ChestTypes == Chest.ChestTypes.m_NoVideo)
                {
                    m_CurrentChest.Open();
                }
                else if (m_CurrentChest.m_ChestTypes == Chest.ChestTypes.m_OpenByVideo)
                {
                    ChestsControl.m_Main.ShowVideoMessage();
                }
            }
        }

        public void LoadLevel()
        {
            ExplorationCamera.m_Main.m_Target = m_LevelPoints[m_LevelNum].transform;
            ExplorationCamera.m_Main.m_TargetSize = 130;
            m_GameplayData.m_LevelNum = m_LevelNum;
            m_GameplayData.m_FieldNum = m_Content.m_Levels[m_LevelNum].m_LevelTheme;
            m_GameplayData.m_GameMode = GameModes.Singleplayer;
            m_GameplayData.m_MatchMode = MatchModes.Friendly;
            ExplorationUI.m_Current.HideLevelUI();
            StartCoroutine(Co_LoadLevel());

        }

        IEnumerator Co_LoadLevel()
        {
            FadeControl.m_Current.StartFadeOut();
            // SoundGallery.PlaySound("Oriental");
            yield return new WaitForSeconds(1.2f);
            SceneManager.LoadScene("FightScene-1");
        }
    }
}