
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

        // Add configurable number of pawns
        [SerializeField]
        private int m_MaxPawns = 2; // Default to 6, but can be changed in inspector

        public GameObject m_ExplorationPawnPrefab;

        public Level[] m_LevelData;
        public LevelPoint[] m_LevelPoints;
        public ProCamera2DTriggerZoomController[] m_AgentZoomPoints;

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
            
            // Clamp max pawns to a reasonable range (2-12)
            m_MaxPawns = Mathf.Clamp(m_MaxPawns, 2, 12);
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
        // Delegate to the unified PawnManager
        PawnManager pawnManager = FindObjectOfType<PawnManager>();
        if (pawnManager == null)
        {
            // If not found, create one
            GameObject managerObj = new GameObject("PawnManager");
            pawnManager = managerObj.AddComponent<PawnManager>();
            
            // Set references
            pawnManager.SetDataStorage(m_DataStorage);
            pawnManager.SetContent(m_Content);
            pawnManager.SetPawnPrefab(m_ExplorationPawnPrefab);
        }
        
        // Initialize pawns through the manager
        pawnManager.InitializePawns();
        
        // Store reference to first pawn
        if (pawnManager.GetAllPawns().Count > 0)
        {
            // Convert to array format for compatibility with existing code
            m_Pawns = new ExplorationPawn[pawnManager.GetAllPawns().Count];
            for (int i = 0; i < pawnManager.GetAllPawns().Count; i++)
            {
                m_Pawns[i] = pawnManager.GetAllPawns()[i].GetComponent<ExplorationPawn>();
            }
        }
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