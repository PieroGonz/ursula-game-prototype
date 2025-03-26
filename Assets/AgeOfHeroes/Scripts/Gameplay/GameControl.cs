using AgeOfHeroes.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using AgeOfHeroes.ScriptableObjects;
using AgeOfHeroes.Gameplay;
using UnityEngine.SceneManagement;
using System.Security.Principal;


namespace AgeOfHeroes.Gameplay
{
    public class GameControl : MonoBehaviour
    {
        [SerializeField]
        public DataStorage m_DataStorage;
        [SerializeField]
        public GameplayData m_GameplayData;
        [SerializeField]
        public Contents m_Contents;
        [SerializeField]
        private OpponentData m_OpponentData;
        [SerializeField]
        private PlayerData m_PlayerData;
        [SerializeField]
        public GameplayContents m_GameplayContents;

        [HideInInspector]
        public Pawn m_SelectedPawn;
        [HideInInspector]
        public Pawn m_SelectedEnemy;

        [HideInInspector]
        public int m_SelectedPawnNum = 0;
        [HideInInspector]
        public int m_SelectedAttackNum = 0;
        [HideInInspector]
        public int m_SelectedEnemyNum = 0;

        [HideInInspector]
        public bool Pause = false;
        [HideInInspector]
        public bool m_FirstTime;

        public float m_TurnTimer;
        private bool m_StartTimer;

        [HideInInspector]
        public int m_Team0Deaths = 0;
        [HideInInspector]
        public int m_Team1Deaths = 0;

        [HideInInspector]
        public bool[] m_UsedItem;
        [HideInInspector]
        public bool m_SpecialItemUsed;

        public GameObject[] m_Fields;

        public GameObject m_Tutorial;

        public Team[] m_Teams;
        public enum GameStates
        {
            None,
            MatchStart,
            MatchEnd,
            ChooseHero,
            ChooseAction,
            ChooseEnemy,
            Enganging,
            WaitForOther
        }

        [HideInInspector]
        public int m_Turn;

        public static GameControl m_Current;

        [HideInInspector]
        public GameStates m_GameState = GameStates.MatchStart;

        private void Awake()
        {
            m_Current = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            m_GameplayData.m_PauseState = false;
            m_GameplayData.m_GameEnded = false;
            m_UsedItem = new bool[4];
            StartCoroutine(Co_HandleGameStart());

            SoundGallery.PlaySound("Intro (1)");

            if (m_GameplayData.m_GameMode != GameModes.Tutorial)
            {
                m_GameState = GameStates.ChooseHero;
            }

            if (m_GameplayData.m_GameMode == GameModes.Multiplayer || m_GameplayData.m_GameMode == GameModes.MultiplayerAI)
            {
                if (m_GameplayData.m_MyTeam == 0)
                {
                    m_Turn = 0;
                    m_TurnTimer = 16;
                    m_StartTimer = true;
                }
                else
                {
                    m_Turn = 1;
                    m_GameState = GameStates.WaitForOther;
                }
            }


            m_Teams[0].m_OtherTeam = m_Teams[1];
            m_Teams[0].m_TeamNum = 0;
            m_Teams[0].m_FaceDirection = 1;
            m_Teams[0].m_Pawns = new Pawn[3];
            m_Teams[0].m_IsClient = true;
            for (int i = 0; i < 3; i++)
            {
                Character C = m_Contents.m_Characters[m_DataStorage.m_CharactersNumber[i]];
                GameObject charObj = Instantiate(C.m_Skins[C.m_SkinNum].m_Prefab);
                charObj.transform.position = m_Teams[0].m_Tiles[i].transform.position + new Vector3(0, 0, -1);
                m_Teams[0].m_Pawns[i] = charObj.GetComponent<Pawn>();
                m_Teams[0].m_Pawns[i].m_MyTeam = m_Teams[0];
                m_Teams[0].m_Pawns[i].m_ID = i;
                m_Teams[0].m_Pawns[i].m_CharacterData = C;
                m_Teams[0].m_Pawns[i].m_WeaponNum = m_Teams[0].m_Pawns[i].m_CharacterData.m_WeaponNum;
                m_Teams[0].m_Pawns[i].m_CharacterLevel = m_Teams[0].m_Pawns[i].m_CharacterData.m_ItemLevel;
            }

            m_Teams[1].m_OtherTeam = m_Teams[0];
            m_Teams[1].m_TeamNum = 1;
            m_Teams[1].m_FaceDirection = -1;
            m_Teams[1].m_Pawns = new Pawn[3];
            m_Teams[1].m_IsClient = false;


            if (m_GameplayData.m_GameMode == GameModes.Multiplayer)
            {
                m_Teams[1].m_ControlType = 1;
                for (int i = 0; i < 3; i++)
                {
                    Character C = m_Contents.m_Characters[m_OpponentData.m_CharactersNumber[i]];
                    GameObject obj = Instantiate(C.m_Skins[m_OpponentData.m_CharactersSkins[i]].m_Prefab);
                    obj.transform.position = m_Teams[1].m_Tiles[i].transform.position + new Vector3(0, 0, -1);

                    m_Teams[1].m_Pawns[i] = obj.GetComponent<Pawn>();
                    m_Teams[1].m_Pawns[i].m_MyTeam = m_Teams[1];
                    m_Teams[1].m_Pawns[i].m_ID = i;
                    m_Teams[1].m_Pawns[i].m_CharacterData = C;
                    m_Teams[1].m_Pawns[i].m_BodyBase.transform.localScale = new Vector3(-1, 1, 1);
                    m_Teams[1].m_Pawns[i].m_IsOpponent = true;
                    m_Teams[1].m_Pawns[i].m_WeaponNum = m_OpponentData.m_CharactersWeapons[i];
                    m_Teams[1].m_Pawns[i].m_CharacterLevel = m_OpponentData.m_CharactersLevels[i];
                }
            }
            else if (m_GameplayData.m_GameMode == GameModes.MultiplayerAI)
            {
                m_Teams[1].m_ControlType = 2;
                for (int i = 0; i < 3; i++)
                {
                    Character C = m_Contents.m_Characters[m_OpponentData.m_CharactersNumber[i]];
                    GameObject obj = Instantiate(C.m_Skins[m_OpponentData.m_CharactersSkins[i]].m_Prefab);
                    obj.transform.position = m_Teams[1].m_Tiles[i].transform.position + new Vector3(0, 0, -1);

                    m_Teams[1].m_Pawns[i] = obj.GetComponent<Pawn>();
                    m_Teams[1].m_Pawns[i].m_MyTeam = m_Teams[1];
                    m_Teams[1].m_Pawns[i].m_ID = i;
                    m_Teams[1].m_Pawns[i].m_CharacterData = C;
                    m_Teams[1].m_Pawns[i].m_BodyBase.transform.localScale = new Vector3(-1, 1, 1);
                    m_Teams[1].m_Pawns[i].m_IsOpponent = true;
                    m_Teams[1].m_Pawns[i].m_WeaponNum = m_OpponentData.m_CharactersWeapons[i];
                    m_Teams[1].m_Pawns[i].m_CharacterLevel = m_OpponentData.m_CharactersLevels[i];
                }
            }
            else if (m_GameplayData.m_GameMode == GameModes.Singleplayer)
            {
                //m_StartTimer = true;
                m_Teams[1].m_ControlType = 2;
                for (int i = 0; i < 3; i++)
                {
                    Character C = m_Contents.m_Levels[m_GameplayData.m_LevelNum].m_EnemyCharacters[i];
                    GameObject charObj = Instantiate(C.m_EmptyCharacter);

                    charObj.transform.position = m_Teams[1].m_Tiles[i].transform.position + new Vector3(0, 0, -1);

                    m_Teams[1].m_Pawns[i] = charObj.GetComponent<Pawn>();
                    m_Teams[1].m_Pawns[i].m_MyTeam = m_Teams[1];
                    m_Teams[1].m_Pawns[i].m_ID = i;
                    m_Teams[1].m_Pawns[i].m_BodyBase.transform.localScale = new Vector3(-1, 1, 1);

                    if (C.m_TitleEnglish == "empty")
                    {
                        charObj.SetActive(false);
                        m_Teams[1].m_Pawns[i].m_HealthControl.m_IsDead = true;
                    }
                }
            }
            else if (m_GameplayData.m_GameMode == GameModes.Tutorial)
            {
                m_Tutorial.gameObject.SetActive(true);
                Tutorial.m_Current.StartTut();
                m_Teams[1].m_ControlType = 2;
                float[] health = new float[3] { 20, 30, 30 };
                for (int i = 0; i < 3; i++)
                {
                    Character C = m_Contents.m_Tutorial.m_EnemyCharacters[i];
                    GameObject charObj = Instantiate(C.m_EmptyCharacter);

                    charObj.transform.position = m_Teams[1].m_Tiles[i].transform.position + new Vector3(0, 0, -1);

                    m_Teams[1].m_Pawns[i] = charObj.GetComponent<Pawn>();
                    m_Teams[1].m_Pawns[i].m_MyTeam = m_Teams[1];
                    m_Teams[1].m_Pawns[i].m_ID = i;
                    m_Teams[1].m_Pawns[i].m_HealthControl.m_MaxHealth = health[i];
                    m_Teams[1].m_Pawns[i].m_HealthControl.m_Health = health[i];
                    m_Teams[1].m_Pawns[i].m_BodyBase.transform.localScale = new Vector3(-1, 1, 1);

                    if (C.m_TitleEnglish == "empty")
                    {
                        charObj.SetActive(false);
                        m_Teams[1].m_Pawns[i].m_HealthControl.m_IsDead = true;
                    }
                }

            }

            for (int i = 0; i < m_Fields.Length; i++)
            {
                m_Fields[i].SetActive(false);
            }

            m_Fields[m_GameplayData.m_FieldNum].SetActive(true);

        }
        public void RestartLevel()
        {
            Invoke("LoadLevelDelayed", 1f);
        }
        public void LoadLevelDelayed()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        IEnumerator Co_HandleGameStart()
        {
            UISystem.ShowUI("GameUI");

            yield return new WaitForSeconds(1);
            if (m_GameplayData.m_GameMode != GameModes.Tutorial)
            {
                GameUI.m_Current.ShowTurn();
            }

        }
        // Update is called once per frame
        void Update()
        {
            if (m_GameplayData.m_GameMode == GameModes.Multiplayer || m_GameplayData.m_GameMode == GameModes.MultiplayerAI)
            {
                if (m_StartTimer)
                {
                    if (m_TurnTimer > 0)
                    {
                        m_TurnTimer -= Time.deltaTime;
                    }

                    else
                    {
                        m_TurnTimer = 0;
                        // Debug.Log("Times up!");
                        PlayerAction();
                        m_StartTimer = false;
                    }
                }
            }

            Pawn tempPawn = null;

            switch (m_GameState)
            {
                case GameStates.WaitForOther:
                    break;

                case GameStates.ChooseHero:
                    if (m_Turn == 0)
                    {
                        tempPawn = CheckPawnSelect(true);
                        if (tempPawn != null)
                        {
                            SelectPawn(tempPawn);
                        }
                    }
                    break;

                case GameStates.ChooseAction:
                    if (m_Turn == 0)
                    {
                        tempPawn = CheckPawnSelect(true);
                        if (tempPawn != null)
                        {
                            SelectPawn(tempPawn);
                        }
                    }
                    break;

                case GameStates.ChooseEnemy:
                    if (m_Turn == 0)
                    {
                        tempPawn = CheckPawnSelect(false);
                        if (tempPawn != null)
                        {
                            m_SelectedEnemyNum = tempPawn.m_ID;
                            m_SelectedEnemy = tempPawn;
                            GameUI.m_Current.m_ButtonsPanel.SetActive(false);
                            m_SelectedPawn.CurrentAttackDamage = (int)m_SelectedPawn.CalculateDamage();


                            StartCoroutine(Attack());
                            m_SelectedEnemy.GetComponent<Pawn>().ShowHighlight();
                        }
                    }
                    break;
            }

        }

        public Pawn CheckPawnSelect(bool myTeam)
        {
            Vector3 screenMousePos = Input.mousePosition;
            bool touched = false;
            if (Input.GetMouseButtonDown(0))
                touched = true;

            if (Input.touchCount > 0)
            {
                touched = true;
                screenMousePos = Input.touches[0].position;
            }

            if (touched)
            {
                Vector3 MousePos = FightCamera.m_Current.GetComponent<Camera>().ScreenToWorldPoint(screenMousePos);
                MousePos.z = 0;

                Collider2D[] collider = Physics2D.OverlapCircleAll(MousePos, 10);

                for (int i = 0; i < collider.Length; i++)
                {
                    if (collider[i].gameObject.tag == "Pawn" && collider[i].gameObject.activeSelf)
                    {
                        Pawn temp = collider[i].gameObject.GetComponent<Pawn>();
                        if (myTeam && temp.m_MyTeam.m_TeamNum == 0)
                        {
                            return temp;
                        }
                        else if (!myTeam && temp.m_MyTeam.m_TeamNum == 1)
                        {
                            return temp;
                        }
                    }
                }
            }

            return null;
        }

        public void SelectPawn(int num)
        {
            if (m_Teams[0].m_Pawns[num].gameObject.activeSelf)
            {
                bool other = false;
                if (m_SelectedPawn != m_Teams[0].m_Pawns[num])
                    other = true;

                m_SelectedPawn = m_Teams[0].m_Pawns[num];
                m_SelectedPawnNum = num;

                if (other)
                {
                    m_SelectedPawn.SetAnimReady();
                    GameUI.m_Current.ShowAbilityPanel();
                    GameUI.m_Current.UpdateAbilities();
                    GameUI.m_Current.UpdateSelectedPawn();
                    SoundGallery.PlaySound("FloorButton");
                    HighlightPawn();
                    GameObject obj = Instantiate(m_GameplayContents.m_SelectPawnParticle);
                    obj.transform.position = m_SelectedPawn.transform.position + new Vector3(0, 50, -4);
                    Destroy(obj, 3);
                }


                if (m_GameplayData.m_GameMode != GameModes.Tutorial)
                {
                    m_GameState = GameStates.ChooseAction;
                }
                else
                {
                    if (!Tutorial.m_Current.m_Passed[0])
                    {
                        m_GameState = GameStates.None;
                        Tutorial.m_Current.StartTut_2();
                    }
                    else
                    {
                        m_GameState = GameStates.ChooseAction;
                    }
                }
            }

        }

        public void SelectPawn(Pawn pawn)
        {
            if (!m_GameplayData.m_PauseState)
            {
                bool other = false;
                if (m_SelectedPawn != pawn)
                    other = true;

                m_SelectedPawn = pawn;
                m_SelectedPawnNum = pawn.m_ID;

                if (other)
                {
                    GameUI.m_Current.ShowAbilityPanel();
                    GameUI.m_Current.UpdateAbilities();
                    GameUI.m_Current.UpdateSelectedPawn();
                    SoundGallery.PlaySound("FloorButton");
                    HighlightPawn();
                    m_SelectedPawn.SetAnimReady();
                    GameObject obj = Instantiate(m_GameplayContents.m_SelectPawnParticle);
                    obj.transform.position = m_SelectedPawn.transform.position + new Vector3(0, 50, -4);
                    Destroy(obj, 3);
                }

                if (m_GameplayData.m_GameMode != GameModes.Tutorial)
                {
                    m_GameState = GameStates.ChooseAction;
                }
                else
                {
                    if (!Tutorial.m_Current.m_Passed[0])
                    {
                        m_GameState = GameStates.None;
                        Tutorial.m_Current.StartTut_2();
                    }
                    else
                    {
                        m_GameState = GameStates.ChooseAction;
                    }
                }
            }
        }

        IEnumerator Attack()
        {
            if (m_Turn == 0)
            {
                m_Contents.m_Achievements[1].AddCount();
                m_DataStorage.SaveData();
            }
            m_GameState = GameStates.Enganging;
            if (m_GameplayData.m_GameMode == GameModes.Tutorial)
            {
                GameUI.m_Current.m_TutorialPanels[5].gameObject.SetActive(false);
            }
            m_StartTimer = false;
            GameUI.m_Current.m_TimerPanel.gameObject.SetActive(false);
            m_SelectedPawn.m_TargetEnemy = m_SelectedEnemy;
            if (m_GameplayData.m_GameMode == GameModes.Singleplayer || m_GameplayData.m_GameMode == GameModes.Tutorial)
            {
                if (m_Turn == 0)
                {
                    m_SelectedPawn.ApplyAbilityCooldown();
                }
            }
            else
            {
                m_SelectedPawn.ApplyAbilityCooldown();
            }
            m_SelectedPawn.Attack(m_Teams[m_Turn].m_ActionType);
            GameUI.m_Current.DeselectPawns();

            while (!m_SelectedPawn.m_AttackEnded)
            {
                yield return null;
            }

            yield return new WaitForSeconds(.3f);

            m_Teams[0].CheckStats();
            m_Teams[1].CheckStats();

            yield return new WaitForSeconds(.5f);

            m_Teams[0].CheckDeath();
            m_Teams[1].CheckDeath();

            yield return new WaitForSeconds(.5f);

            HandleEndTurn();
        }

        public void HandleEndTurn()
        {

            if (m_Teams[0].AllDead())
            {
                if (m_GameplayData.m_GameMode == GameModes.Singleplayer)
                {
                    m_GameplayData.m_GameEndResult = 1; //lose
                }
                else
                {
                    m_GameplayData.m_GameEndResult = m_GameplayData.m_OtherTeam;
                }

                HandleGameEnd();
                SoundGallery.PlaySound("You Lose (6)");
                //end game
            }
            else if (m_Teams[1].AllDead())
            {
                if (m_GameplayData.m_GameMode == GameModes.Singleplayer)
                {
                    m_GameplayData.m_GameEndResult = 0; //win
                }
                else
                {
                    m_GameplayData.m_GameEndResult = m_GameplayData.m_MyTeam;
                }

                HandleGameEnd();
                SoundGallery.PlaySound("You Win (5)");
                //end game
            }
            else
            {

                if (m_Turn == 0)
                {
                    m_Turn = 1;
                    if (m_Teams[1].m_ControlType == 2)
                    {
                        if (m_GameplayData.m_GameMode != GameModes.Tutorial)
                        {
                            StartCoroutine(AIAction());
                        }
                        else
                        {
                            if (!Tutorial.m_Current.m_Passed[2])
                            {
                                m_GameState = GameStates.None;
                                Tutorial.m_Current.StartTut_4();
                            }
                            else
                            {
                                StartCoroutine(AIAction());
                            }

                        }

                    }
                    else
                    {
                        m_GameState = GameStates.WaitForOther;
                    }
                }
                else
                {
                    m_Turn = 0;
                    m_TurnTimer = 16;
                    m_StartTimer = true;
                    m_GameState = GameStates.ChooseHero;
                }

                if (m_SelectedEnemy != null)
                {
                    m_SelectedEnemy.HideHighlight();
                }
                if (m_SelectedPawn != null)
                {
                    m_SelectedPawn.HideHighlight();
                }

                GameUI.m_Current.ShowTurn();

                SoundGallery.PlaySound("Queue");
            }
        }

        public void HandleGameEnd()
        {
            StartCoroutine(Co_HandleGameEnd());
        }

        public void TutorialAIAction()
        {
            StartCoroutine(AIAction());
        }

        IEnumerator Co_HandleGameEnd()
        {
            if (m_GameState != GameStates.MatchEnd)
            {
                m_GameplayData.m_GameEnded = true;
                m_GameState = GameStates.MatchEnd;
                GameUI.m_Current.gameObject.SetActive(false);
                yield return new WaitForSeconds(1f);

                if (m_GameplayData.m_GameMode == GameModes.MultiplayerAI)
                {
                    UISystem.ShowUI("OnlineGameEndUI");
                }
                else if (m_GameplayData.m_GameMode == GameModes.Singleplayer)
                {
                    if (m_GameplayData.m_GameEndResult == 0) //win
                    {
                        if (m_GameplayData.m_LevelNum == m_DataStorage.m_LastLevelNum)
                        {
                            m_FirstTime = true;
                        }
                        int next = m_GameplayData.m_LevelNum + 1;
                        if (next < 30)
                        {
                            if (m_DataStorage.m_LastLevelNum < next)
                            {
                                m_DataStorage.m_LastLevelNum = next;
                                m_DataStorage.m_ShowWinLevel = true;
                            }

                            m_Contents.m_Levels[next].m_Unlocked = true;
                        }
                        m_Contents.m_Levels[m_GameplayData.m_LevelNum].m_Passed = true;

                        UISystem.ShowUI("LevelEndUI");
                    }
                    else //lose
                    {
                        UISystem.ShowUI("LevelLoseUI");
                    }
                }

                else if (m_GameplayData.m_GameMode == GameModes.Tutorial)
                {
                    if (m_GameplayData.m_GameEndResult == 0) //win
                    {
                        UISystem.ShowUI("LevelEndUI");
                    }
                    else //lose
                    {
                        UISystem.ShowUI("LevelLoseUI");
                    }
                }
            }
            yield return null;
        }

        IEnumerator AIAction()
        {
            yield return new WaitForSeconds(1.5f);
            while (true)
            {
                m_SelectedPawn = m_Teams[1].m_Pawns[Random.Range(0, 3)];
                if (m_SelectedPawn.gameObject.activeSelf)
                {
                    break;
                }
            }

            while (true)
            {
                int num = Random.Range(0, 3);
                m_SelectedEnemyNum = num;
                m_SelectedEnemy = m_Teams[0].m_Pawns[num];
                if (m_SelectedEnemy.gameObject.activeSelf)
                {
                    break;
                }
            }

            if (m_GameplayData.m_GameMode == GameModes.Singleplayer || m_GameplayData.m_GameMode == GameModes.Tutorial)
            {
                if (((EnemyPawn_A)m_SelectedPawn).m_SecondAttack)
                {
                    m_Teams[1].m_ActionType = Random.Range(0, 2);
                }
                else
                {
                    m_Teams[1].m_ActionType = 0;
                }
            }
            else
            {
                while (true)
                {
                    int randomInt = Random.Range(0, 5);
                    if (randomInt < 2)
                        m_Teams[1].m_ActionType = 0;
                    else if (randomInt < 4)
                        m_Teams[1].m_ActionType = 1;
                    else
                        m_Teams[1].m_ActionType = 2;

                    if (m_SelectedPawn.m_CoolDown[m_Teams[1].m_ActionType] == 0)
                    {
                        break;
                    }
                }

                m_SelectedPawn.m_AttackNum = m_Teams[1].m_ActionType;
                m_SelectedPawn.ApplyAbilityCooldown();
                m_SelectedPawn.CurrentAttackDamage = (int)(.7f * m_SelectedPawn.CalculateDamage());

            }


            m_SelectedEnemy.ShowHighlight();
            m_SelectedPawn.ShowHighlight();
            StartCoroutine(Attack());

        }

        public void SelectAbility(int num)
        {
            if (m_SelectedPawn.GetComponent<CharacterH>() != null && num == 2)
            {
                m_SelectedAttackNum = num;
                m_Teams[m_Turn].m_ActionType = num;
                m_SelectedPawn.m_AttackNum = num;
                GameUI.m_Current.m_ButtonsPanel.SetActive(false);


                StartCoroutine(Attack());
            }
            else
            {
                m_SelectedAttackNum = num;
                m_Teams[m_Turn].m_ActionType = num;
                m_SelectedPawn.m_AttackNum = num;
                if (m_GameplayData.m_GameMode != GameModes.Tutorial)
                {
                    m_GameState = GameStates.ChooseEnemy;
                }
                else
                {
                    if (!Tutorial.m_Current.m_Passed[1])
                    {
                        m_GameState = GameStates.None;
                        Tutorial.m_Current.StartTut_3();
                    }
                    else
                    {
                        m_GameState = GameStates.ChooseEnemy;
                    }

                }

            }

        }

        public void UseItem(int itemNum)
        {
            m_Contents.m_Achievements[2].AddCount();
            m_DataStorage.SaveData();
            m_DataStorage.PowerCounts[itemNum]--;
            SoundGallery.PlaySound("Special & Powerup (18)");
            switch (itemNum)
            {
                case 0:
                    m_Teams[0].HealTeam();
                    break;
                case 1:
                    m_Teams[1].AddStats(0, 6);
                    break;
                case 2:
                    m_Teams[0].AddStats(1, 5);
                    break;
                case 3:
                    m_Teams[1].AddStats(2, 3);
                    break;
            }
        }

        public void HandleOpponentTurn(int charNum, int actionNum, int enmeyNum, int damage)
        {
            m_SelectedPawn = m_Teams[1].m_Pawns[charNum];
            m_SelectedPawn.m_AttackNum = actionNum;
            m_SelectedPawn.CurrentAttackDamage = damage;
            m_SelectedAttackNum = m_Teams[1].m_ActionType = actionNum;
            m_SelectedEnemy = m_Teams[0].m_Pawns[enmeyNum];

            StartCoroutine(Attack());
            m_SelectedEnemy.GetComponent<Pawn>().ShowHighlight();
        }
        public void HighlightPawn()
        {
            for (int i = 0; i < m_Teams[m_Turn].m_Pawns.Length; i++)
            {
                if (i == m_SelectedPawn.m_ID)
                {
                    m_Teams[m_Turn].m_Pawns[i].ShowHighlight();
                }
                else
                {
                    m_Teams[m_Turn].m_Pawns[i].HideHighlight();
                }
            }
        }

        public void PauseGame()
        {
            Pause = true;
            Time.timeScale = 0;
            GameUI.m_Current.gameObject.SetActive(false);
            //UISystem.RemoveUI("SoccerInGameUI");
            UISystem.ShowUI("PauseUI");
        }
        public void ResumeGame()
        {
            GameUI.m_Current.gameObject.SetActive(true);
            //UISystem.ShowUI("SoccerInGameUI");
            Pause = false;
            Time.timeScale = 1;
        }

        public void PlayerAction()
        {
            while (true)
            {
                m_SelectedPawn = m_Teams[0].m_Pawns[Random.Range(0, 3)];
                if (m_SelectedPawn.gameObject.activeSelf)
                {
                    break;
                }
            }

            while (true)
            {
                int num = Random.Range(0, 2);
                if (m_SelectedPawn.m_CoolDown[num] == 0)
                {
                    m_SelectedAttackNum = num;
                    m_Teams[m_Turn].m_ActionType = num;
                    m_SelectedPawn.m_AttackNum = num;
                    break;
                }
            }

            while (true)
            {
                int num = Random.Range(0, 3);
                m_SelectedEnemyNum = num;
                m_SelectedEnemy = m_Teams[1].m_Pawns[num];
                if (m_SelectedEnemy.gameObject.activeSelf)
                {
                    break;
                }
            }
            m_SelectedPawn.CurrentAttackDamage = (int)m_SelectedPawn.CalculateDamage();
            StartCoroutine(Attack());

        }
    }
}
