using AgeOfHeroes.Gameplay;
using AgeOfHeroes.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



namespace AgeOfHeroes.UI
{
    public class GameUI : MonoBehaviour
    {
        public Image m_ExitMessageImage;
        public Image m_AddLifeBtn;

        public Text m_TimerText;
        public Image m_TimerPanel;
        public GameObject[] m_TutorialPanels;

        public GameObject m_ButtonsPanel;

        public Text[] m_CoolDownText;
        public Sprite[] m_StatIcons;

        public Image[] m_PowerButtons;
        public Image[] m_PowerIcons;
        public Material m_GrayscaleUIMat;
        public GameObject m_PowerPanel;

        public Button[] m_AbilityButtons;

        public Image m_PlayerTurn1;
        public Image m_PlayerTurn2;

        public Sprite[] m_AbilityFrameSprites;
        public Image[] m_AbilityFrames;
        public Image[] m_AbilityImages;

        public Image[] m_TotalLifeBars;
        public Text[] m_TotalLifeTexts;

        public Image m_LockImage;

        public Sprite[] m_CharacterFrameSprites;
        public Image[] m_CharacterFrames;
        public Image[] m_CharacterImages;
        public Image[] m_CharDeadImages;
        [SerializeField]
        public Image[] m_TeamPlayerImages;
        public static GameUI m_Current;

        public DataStorage m_DataStorage;
        public Contents m_Content;
        [SerializeField, Space]
        private GameplayData m_GameplayData;
        [SerializeField, Space]
        private UIGraphicContents m_UIGraphicContents;
        [SerializeField, Space]
        private UITextContents m_UITextContentsContents;

        [SerializeField]
        private OpponentData m_OpponentData;
        [SerializeField, Space]
        private PlayerData m_PlayerData;
        private void Awake()
        {
            m_Current = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            m_AddLifeBtn.gameObject.SetActive(false);
            m_PlayerTurn1.gameObject.SetActive(false);
            m_PlayerTurn2.gameObject.SetActive(false);
            m_ExitMessageImage.gameObject.SetActive(false);
            m_TimerPanel.gameObject.SetActive(false);

            if (m_GameplayData.m_GameMode == GameModes.Tutorial)
            {
                m_PowerPanel.gameObject.SetActive(false);
            }

            for (int i = 0; i < 3; i++)
            {
                Character character = m_Content.m_Characters[m_DataStorage.m_CharactersNumber[i]];
                m_CharacterImages[i].sprite = character.m_Icon;
            }

            for (int i = 0; i < 3; i++)
            {
                m_AbilityFrames[i].sprite = m_AbilityFrameSprites[0];
                m_AbilityButtons[i].transform.localScale = Vector3.one;
            }

            if (m_GameplayData.m_GameMode == GameModes.Singleplayer)
            {
                if (m_DataStorage.PowerCounts.All(count => count == 0))
                {
                    m_AddLifeBtn.gameObject.SetActive(true);
                }
            }

        }


        // Update is called once per frame
        void Update()
        {
            m_TeamPlayerImages[0].sprite = m_PlayerData.m_PlayerAvatarSprite;

            if (m_GameplayData.m_GameMode == GameModes.Singleplayer)
            {

            }
            else
            {
                m_TeamPlayerImages[1].sprite = m_OpponentData.m_PlayerAvatarSprite;
            }

            m_TimerText.text = ((int)GameControl.m_Current.m_TurnTimer).ToString();

            if (GameControl.m_Current.m_SelectedPawn != null)
            {
                for (int i = 0; i < m_AbilityButtons.Length; i++)
                {
                    if (GameControl.m_Current.m_SelectedPawn.m_CoolDown[i] == 0)
                    {
                        m_AbilityButtons[i].enabled = true;

                    }
                    else
                    {
                        m_AbilityButtons[i].enabled = false;

                    }
                }
            }


            for (int i = 0; i < 4; i++)
            {
                if (m_DataStorage.PowerCounts[i] != 0 && !GameControl.m_Current.m_SpecialItemUsed && GameControl.m_Current.m_Turn == 0)
                {
                    m_PowerIcons[i].material = null;
                }
                else
                {
                    m_PowerIcons[i].material = m_GrayscaleUIMat;
                }
            }


            float life = 0;
            float maxLife = 0;
            for (int i = 0; i < 3; i++)
            {
                life += GameControl.m_Current.m_Teams[0].m_Pawns[i].m_HealthControl.m_TempHealth;
                maxLife += GameControl.m_Current.m_Teams[0].m_Pawns[i].m_HealthControl.m_MaxHealth;
            }
            m_TotalLifeBars[0].rectTransform.sizeDelta = new Vector2((life / maxLife) * 400, 32);
            m_TotalLifeTexts[0].text = Mathf.FloorToInt(life).ToString();

            life = 0;
            maxLife = 0;
            for (int i = 0; i < 3; i++)
            {
                if (GameControl.m_Current.m_Teams[1].m_Pawns[i].m_HealthControl.m_MaxHealth > 0)
                {
                    life += GameControl.m_Current.m_Teams[1].m_Pawns[i].m_HealthControl.m_TempHealth;
                    maxLife += GameControl.m_Current.m_Teams[1].m_Pawns[i].m_HealthControl.m_MaxHealth;
                }
            }
            m_TotalLifeBars[1].rectTransform.sizeDelta = new Vector2((life / maxLife) * 400, 32);
            m_TotalLifeTexts[1].text = Mathf.FloorToInt(life).ToString();

            for (int i = 0; i < 3; i++)
            {
                if (GameControl.m_Current.m_Teams[0].m_Pawns[i].m_HealthControl.m_IsDead)
                {
                    m_CharacterImages[i].material = m_GrayscaleUIMat;
                    m_CharacterImages[i].color = new Color(.4f, .4f, .4f, 1);
                    m_CharDeadImages[i].gameObject.SetActive(true);
                }
                else
                {
                    m_CharacterImages[i].material = null;
                    m_CharacterImages[i].color = Color.white;
                    m_CharDeadImages[i].gameObject.SetActive(false);
                }
            }
        }
        public void BtnAddLifeVideo()
        {
            UIMessage_B msg = UISystem.ShowMessage_B("UIMessage_B", 0, m_UITextContentsContents.m_Messages[66], null);
            Image img = UISystem.FindImage(msg.gameObject, "MessageImageFade_2");
            img.sprite = m_UIGraphicContents.m_Graphics[18];
            img.gameObject.SetActive(true);
            msg.f_Clicked_WatchVideoToUnlock = WatchLifePowerReward;
            // msg.f_Clicked_No = StartOnlineSearch;
            SoundGallery.PlaySound("pop1");
        }
        public bool WatchLifePowerReward()
        {
            if (Application.platform != RuntimePlatform.Android)
            {
                HandleGetLifeReward();
            }

            return true;
        }


        public void UseAbility(int num)
        {
            Character character = GameControl.m_Current.m_SelectedPawn.m_CharacterData;
            UpdateAbilities();

            if (character.m_Abilities[num].m_Unlocked)
            {
                m_AbilityFrames[num].sprite = m_AbilityFrameSprites[1];
                m_AbilityButtons[num].transform.localScale = 1.1f * Vector3.one;
                SoundGallery.PlaySound("FloorButton");
                GameControl.m_Current.SelectAbility(num);
            }

        }

        public void UpdateAbilities()
        {
            Character character = GameControl.m_Current.m_SelectedPawn.m_CharacterData;
            string[] statImageNames = { "img-stat-effect-1", "img-stat-effect-2", "img-stat-effect-3" };
            for (int i = 0; i < 3; i++)
            {
                Image statImage = UISystem.FindImage(m_AbilityButtons[i].gameObject, statImageNames[i]);
                m_AbilityImages[i].sprite = character.m_Abilities[i].m_Icon;
                m_AbilityButtons[i].transform.localScale = Vector3.one;

                if (character.m_Abilities[i].m_Unlocked)
                {
                    m_AbilityFrames[i].sprite = m_AbilityFrameSprites[0];
                    m_AbilityImages[i].material = null;
                    if (GameControl.m_Current.m_SelectedPawn.m_CoolDown[i] > 0)
                    {
                        m_AbilityImages[i].color = new Color(.3f, .3f, .3f, 1);
                        m_CoolDownText[i].gameObject.SetActive(true);
                        m_CoolDownText[i].text = (GameControl.m_Current.m_SelectedPawn.m_CoolDown[i]).ToString();

                    }
                    else
                    {
                        m_CoolDownText[i].gameObject.SetActive(false);
                        m_AbilityImages[i].color = Color.white;
                    }
                    m_LockImage.gameObject.SetActive(false);

                    statImage.gameObject.SetActive(true);
                    statImage.material = null;
                    if (character.m_Abilities[i].m_Heal)
                    {
                        statImage.sprite = m_StatIcons[0];
                    }
                    else if (character.m_Abilities[i].m_Stun)
                    {
                        statImage.sprite = m_StatIcons[1];
                    }
                    else if (character.m_Abilities[i].m_Buff)
                    {
                        statImage.sprite = m_StatIcons[2];
                    }
                    else if (character.m_Abilities[i].m_Bleed)
                    {
                        statImage.sprite = m_StatIcons[3];
                    }
                    else
                    {
                        statImage.gameObject.SetActive(false);
                    }
                }
                else
                {
                    m_AbilityFrames[i].sprite = m_AbilityFrameSprites[2];
                    m_AbilityImages[i].material = m_GrayscaleUIMat;
                    m_AbilityImages[i].color = new Color(.5f, .5f, .5f, 1);
                    m_LockImage.gameObject.SetActive(true);
                    m_CoolDownText[i].gameObject.SetActive(false);
                    statImage.material = m_GrayscaleUIMat;
                }
            }
        }

        public void UpdateSelectedPawn()
        {
            for (int i = 0; i < 3; i++)
            {
                if (i == GameControl.m_Current.m_SelectedPawnNum)
                {
                    m_CharacterFrames[i].sprite = m_CharacterFrameSprites[1];
                }
                else
                {
                    m_CharacterFrames[i].sprite = m_CharacterFrameSprites[0];
                }
            }
        }

        public void DeselectPawns()
        {
            for (int i = 0; i < 3; i++)
            {
                m_CharacterFrames[i].sprite = m_CharacterFrameSprites[0];
            }
        }

        public void BtnSelectCharacter(int num)
        {
            if (GameControl.m_Current.m_GameState == GameControl.GameStates.ChooseAction || GameControl.m_Current.m_GameState == GameControl.GameStates.ChooseHero)
            {
                GameControl.m_Current.SelectPawn(num);
            }

        }

        public void ShowTurn()
        {
            StartCoroutine(Co_ShowTurn());
        }

        IEnumerator Co_ShowTurn()
        {
            if (GameControl.m_Current.m_Turn == 0)
            {
                if (m_GameplayData.m_GameMode == GameModes.Multiplayer || m_GameplayData.m_GameMode == GameModes.MultiplayerAI)
                {
                    m_TimerPanel.gameObject.SetActive(true);
                }
                m_PlayerTurn1.gameObject.SetActive(true);

                BaseScriptAnim.MoveFromTo(m_PlayerTurn1.transform, new Vector3(0, 500, 0), new Vector3(0, 100, 0), .4f);
                yield return new WaitForSeconds(2);
                BaseScriptAnim.MoveFromTo(m_PlayerTurn1.transform, new Vector3(0, 100, 0), new Vector3(0, 500, 0), .4f);
                yield return new WaitForSeconds(.4f);
                m_PlayerTurn1.gameObject.SetActive(false);
            }
            else
            {
                m_TimerPanel.gameObject.SetActive(false);
                m_PlayerTurn2.gameObject.SetActive(true);
                BaseScriptAnim.MoveFromTo(m_PlayerTurn2.transform, new Vector3(0, 500, 0), new Vector3(0, 100, 0), .4f);
                yield return new WaitForSeconds(2);
                BaseScriptAnim.MoveFromTo(m_PlayerTurn2.transform, new Vector3(0, 100, 0), new Vector3(0, 500, 0), .4f);
                yield return new WaitForSeconds(.4f);
                m_PlayerTurn2.gameObject.SetActive(false);
            }
        }


        public void UsePower(int num)
        {
            if (!GameControl.m_Current.m_SpecialItemUsed && GameControl.m_Current.m_Turn == 0)
            {
                if (m_DataStorage.PowerCounts[num] > 0 && !GameControl.m_Current.m_UsedItem[num])
                {
                    GameControl.m_Current.UseItem(num);
                    GameControl.m_Current.m_UsedItem[num] = true;
                    GameControl.m_Current.m_SpecialItemUsed = true;
                }
            }

        }

        public void BtnPause()
        {
            m_GameplayData.m_PauseState = true;
            if (m_GameplayData.m_GameMode == GameModes.Singleplayer)
            {
                GameControl.m_Current.PauseGame();
            }
            else
            {
                m_ExitMessageImage.gameObject.SetActive(true);
            }

            SoundGallery.PlaySound("button32");
        }

        public void ShowAbilityPanel()
        {
            m_ButtonsPanel.SetActive(true);
            m_ButtonsPanel.GetComponent<UIAnimation_MoveIn>().StartMove();
        }
        public void HandleGetLifeReward()
        {
            m_DataStorage.PowerCounts[0]++;
            m_DataStorage.SaveData();
            m_AddLifeBtn.gameObject.SetActive(false);
        }
        public void Btn_ExitGameNo()
        {
            m_GameplayData.m_PauseState = false;
            m_ExitMessageImage.gameObject.SetActive(false);
            SoundGallery.PlaySound("pop4");

        }
        public void Btn_ExitGameYes()
        {
            if (!m_DataStorage.m_NotFirstTimeInGame)
            {
                m_DataStorage.m_NotFirstTimeInGame = true;
                m_DataStorage.SaveData();
            }
            m_GameplayData.m_PauseState = false;
            int otherTeam = 0;
            if (m_GameplayData.m_MyTeam == 0)
                otherTeam = 1;
            m_GameplayData.m_GameEndResult = otherTeam;
            m_GameplayData.m_GameEnded = true;


            {
                LoadMainMenuScene();
            }
            // m_DataStorage.Coin -= 50;
            m_DataStorage.SaveData();
            SoundGallery.PlaySound("pop4");
        }
        void LoadMainMenuScene()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
