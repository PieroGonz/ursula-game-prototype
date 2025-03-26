using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes.ScriptableObjects
{
    public enum GameModes
    {
        None,
        Tutorial,
        Singleplayer,
        Multiplayer,
        MultiplayerAI
    }

    public enum MatchModes
    {
        None,
        Friendly,
        OfflineCup,
        IranCup,
        EnglishCup,
        AsiaCup
    }

    [CreateAssetMenu(fileName = "GameplayData", menuName = "Soccer2DCustomObjects/GameplayData", order = 1)]
    public class GameplayData : ScriptableObject
    {
        public GameModes m_GameMode = GameModes.Singleplayer;
        public MatchModes m_MatchMode = MatchModes.None;

        public int m_MyTeam = 0;
        public int m_OtherTeam = 0;
        public int m_LevelNum = 0;
        public bool m_GameEnded = false;
        public int m_GameEndResult = -1;
        public int m_FieldNum = 0;
        public bool m_CheckCompleteDataTransfer;


        [Space]
        public bool m_CheckReward = false;
        public bool m_DoubleCoinReceive = false;
        // public int m_RewardType = 0; // 0 = coin , 1 = gem , 2 = playerturn , 3 = special pack

        public bool m_PowerIngameUIButton = false;
        public bool m_PowerInStoreUI = false;
        public bool m_PowerUseCapacity = false;

        public bool m_UnlockAdsMessage = false;
        public bool m_PauseState;

        //  public int m_MenuVideoType = 0; // 0 = reward wheel video , 1 = random reward video
        //public int m_GameMode = 0; //0 = offline mode , 1 = minigame (soccer mode)


    }


}