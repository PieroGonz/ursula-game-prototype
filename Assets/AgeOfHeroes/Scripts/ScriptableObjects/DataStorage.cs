using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AgeOfHeroes.UI;
using System.Threading.Tasks;

namespace AgeOfHeroes.ScriptableObjects
{
    [CreateAssetMenu(fileName = "DataStorage", menuName = "Soccer2DCustomObjects/DataStorage", order = 1)]
    public class DataStorage : ScriptableObject
    {
        [SerializeField, Space]
        private PlayerData m_PlayerData;

        [SerializeField, Space]
        private Contents m_Contents;

        public int Coin;
        public int Gem;
        public int m_PlayerXP;

        public int[] m_CharactersNumber;

        public int m_LastLevelNum = 0;
        public bool m_ShowWinLevel = false;

        public int m_WinCount;

        public bool m_DailyOfferRecieved = false;
        public bool m_ShowTapsellErrorMessage = false;
        public bool MuteMusic;
        public bool m_UnlockFriendlyOnlineGame;
        public bool m_RemoveAds = false;
        public bool m_CustomizeProfilePic = false;
        public bool[] m_Chests;
        public bool m_PlayerCommented;

        public bool m_GotDailyReward;
        public bool m_NotFirstTimeInGame;


        public int[] PowerCounts;

        public int m_RewardWheelCount = 1;
        public int m_RandomReward;
        public int VideoCountSeen = 0;
        public int m_PowersVideoCountSeen = 0;
        public int m_DayNumInGame = 0;
        public int m_CurrentRewardVideoRequest = 0;
        public string m_SubscribeDate;


        public void SaveData()
        {
            PlayerPrefs.SetString("m_SubscribeDate", m_SubscribeDate);

            PlayerPrefs.SetInt("m_RandomReward", m_RandomReward);
            PlayerPrefs.SetInt("m_DayNumInGame", m_DayNumInGame);

            PlayerPrefs.SetInt("m_PowersVideoCountSeen", m_PowersVideoCountSeen);
            PlayerPrefs.SetInt("VideoCountSeen", VideoCountSeen);

            int tempCoin = PlayerPrefs.GetInt("Coin", 0);
            if (Coin < 0)
                Coin = 0;
            if (Coin - tempCoin <= 30000)
            {
                PlayerPrefs.SetInt("Coin", Coin);
            }
            else
            {
                //cheating
                //  Debug.Log("CHEATING");
                Coin = tempCoin;
                if (Coin < 0)
                    Coin = 0;
                PlayerPrefs.SetInt("Coin", Coin);
            }

            int tempGem = PlayerPrefs.GetInt("Gem", 0);

            if (Gem - tempGem <= 1000)
            {
                PlayerPrefs.SetInt("Gem", Gem);
            }
            else
            {
                //cheating
                //Debug.Log("CHEATING");
                Gem = tempGem;
                PlayerPrefs.SetInt("Gem", tempGem);
            }



            PlayerPrefs.SetInt("m_PlayerXP", m_PlayerXP);
            PlayerPrefs.SetInt("m_RewardWheelCount", m_RewardWheelCount);

            PlayerPrefs.SetInt("m_LastLevelNum", m_LastLevelNum);
            PlayerPrefs.SetInt("m_WinCount", m_WinCount);

            if (m_CharactersNumber != null && m_CharactersNumber.Length == 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    PlayerPrefs.SetInt("CharactersNumber" + i.ToString(), m_CharactersNumber[i]);
                }
            }

            SaveItemUnlockes(m_Contents.m_Characters, "Char");
            SaveItemUnlockes(m_Contents.m_CharacterSkins, "CharSkin");
            SaveItemUnlockes(m_Contents.m_CharacterWeapons, "CharWeapon");
            SaveItemUnlockes(m_Contents.m_Stages, "Stage");

            for (int i = 0; i < m_Contents.m_Characters.Length; i++)
            {
                PlayerPrefs.SetInt("CharSkinNum" + i.ToString(), m_Contents.m_Characters[i].m_SkinNum);
                PlayerPrefs.SetInt("CharWeaponNum" + i.ToString(), m_Contents.m_Characters[i].m_WeaponNum);
                PlayerPrefs.SetInt("CharLevel" + i.ToString(), m_Contents.m_Characters[i].m_ItemLevel);

                if (m_Contents.m_Characters[i].m_Abilities[2].m_Unlocked)
                    PlayerPrefs.SetInt("CharAbility_3_" + i.ToString(), 1);
                else
                    PlayerPrefs.SetInt("CharAbility_3_" + i.ToString(), 0);
            }

            for (int i = 0; i < m_Chests.Length; i++)
            {
                if (m_Chests[i])
                    PlayerPrefs.SetInt("Chest_" + i.ToString(), 1);
                else
                    PlayerPrefs.SetInt("Chest_" + i.ToString(), 0);
            }

            if (m_UnlockFriendlyOnlineGame)
                PlayerPrefs.SetInt("m_UnlockFriendlyOnlineGame", 1);
            else
                PlayerPrefs.SetInt("m_UnlockFriendlyOnlineGame", 0);
            if (MuteMusic)
                PlayerPrefs.SetInt("MuteMusic", 1);
            else
                PlayerPrefs.SetInt("MuteMusic", 0);
            if (m_DailyOfferRecieved)
                PlayerPrefs.SetInt("m_DailyOfferRecieved", 1);
            else
                PlayerPrefs.SetInt("m_DailyOfferRecieved", 0);
            if (m_PlayerCommented)
                PlayerPrefs.SetInt("m_PlayerCommented", 1);
            else
                PlayerPrefs.SetInt("m_PlayerCommented", 0);

            if (m_RemoveAds)
                PlayerPrefs.SetInt("m_RemoveAds", 1);
            else
                PlayerPrefs.SetInt("m_RemoveAds", 0);

            if (m_CustomizeProfilePic)
                PlayerPrefs.SetInt("m_CustomizeProfilePic", 1);
            else
                PlayerPrefs.SetInt("m_CustomizeProfilePic", 0);

            if (m_GotDailyReward)
                PlayerPrefs.SetInt("m_GotDailyReward", 1);
            else
                PlayerPrefs.SetInt("m_GotDailyReward", 0);
            if (m_NotFirstTimeInGame)
                PlayerPrefs.SetInt("m_NotFirstTimeInGame", 1);
            else
                PlayerPrefs.SetInt("m_NotFirstTimeInGame", 0);


            for (int i = 0; i < PowerCounts.Length; i++)
            {
                PlayerPrefs.SetInt("PowerCounts-" + i.ToString(), PowerCounts[i]);
            }

            for (int i = 0; i < m_Contents.m_Achievements.Length; i++)
            {
                PlayerPrefs.SetInt("m_AchievementsAchieved" + i.ToString(), Helper.BoolToInt(m_Contents.m_Achievements[i].m_Achieved));
                PlayerPrefs.SetInt("m_AchievementsGotReward" + i.ToString(), Helper.BoolToInt(m_Contents.m_Achievements[i].m_GotReward));
                PlayerPrefs.SetInt("m_AchievementsCounter" + i.ToString(), m_Contents.m_Achievements[i].Counter);
            }

            for (int i = 0; i < m_Contents.m_ProgressionRewards.Length; i++)
            {
                PlayerPrefs.SetInt("m_ProgressionRewardAchieved" + i.ToString(), Helper.BoolToInt(m_Contents.m_ProgressionRewards[i].m_Aquired));
            }


            PlayerPrefs.Save();
        }

        public void LoadData()
        {
            m_SubscribeDate = PlayerPrefs.GetString("m_SubscribeDate", "");

            m_DayNumInGame = PlayerPrefs.GetInt("m_DayNumInGame", 0);
            VideoCountSeen = PlayerPrefs.GetInt("VideoCountSeen", 0);
            m_PowersVideoCountSeen = PlayerPrefs.GetInt("m_PowersVideoCountSeen", 0);
            Coin = PlayerPrefs.GetInt("Coin", 0);
            Gem = PlayerPrefs.GetInt("Gem", 0);
            m_WinCount = PlayerPrefs.GetInt("m_WinCount", 0);
            m_LastLevelNum = PlayerPrefs.GetInt("m_LastLevelNum", 0);


            int[] defCharacters = new int[3] { 0, 2, 3 };
            m_CharactersNumber = new int[3];
            for (int i = 0; i < 3; i++)
            {
                m_CharactersNumber[i] = PlayerPrefs.GetInt("CharactersNumber" + i.ToString(), defCharacters[i]);
            }

            m_RandomReward = PlayerPrefs.GetInt("m_RandomReward", 0);

            m_RewardWheelCount = PlayerPrefs.GetInt("m_RewardWheelCount", 0);
            m_PlayerXP = PlayerPrefs.GetInt("m_PlayerXP", 0);
            MuteMusic = (PlayerPrefs.GetInt("MuteMusic", 0) == 1);
            m_DailyOfferRecieved = (PlayerPrefs.GetInt("m_DailyOfferRecieved", 0) == 1);
            m_PlayerCommented = (PlayerPrefs.GetInt("m_PlayerCommented", 0) == 1);
            m_UnlockFriendlyOnlineGame = (PlayerPrefs.GetInt("m_UnlockFriendlyOnlineGame", 0) == 1);
            m_RemoveAds = (PlayerPrefs.GetInt("m_RemoveAds", 0) == 1);

            m_GotDailyReward = (PlayerPrefs.GetInt("m_GotDailyReward", 0) == 1);
            m_NotFirstTimeInGame = (PlayerPrefs.GetInt("m_NotFirstTimeInGame", 0) == 1);

            m_CustomizeProfilePic = (PlayerPrefs.GetInt("m_CustomizeProfilePic", 0) == 1);

            PowerCounts = new int[5];
            for (int i = 0; i < 5; i++)
            {
                PowerCounts[i] = PlayerPrefs.GetInt("PowerCounts-" + i.ToString(), 0);
            }

            LoadItemUnlockes(m_Contents.m_Characters, "Char");
            LoadItemUnlockes(m_Contents.m_CharacterSkins, "CharSkin");
            LoadItemUnlockes(m_Contents.m_CharacterWeapons, "CharWeapon");
            LoadItemUnlockes(m_Contents.m_Stages, "Stage");


            for (int i = 0; i < m_Contents.m_Characters.Length; i++)
            {
                m_Contents.m_Characters[i].m_SkinNum = PlayerPrefs.GetInt("CharSkinNum" + i.ToString(), 0);
                m_Contents.m_Characters[i].m_WeaponNum = PlayerPrefs.GetInt("CharWeaponNum" + i.ToString(), 0);
                m_Contents.m_Characters[i].m_ItemLevel = PlayerPrefs.GetInt("CharLevel" + i.ToString(), 0);

                bool abilityUnlock = (PlayerPrefs.GetInt("CharAbility_3_" + i.ToString(), 0) == 1);
                m_Contents.m_Characters[i].m_Abilities[2].m_Unlocked = abilityUnlock;
            }

            for (int i = 0; i < m_Chests.Length; i++)
            {
                m_Chests[i] = (PlayerPrefs.GetInt("Chest_" + i.ToString(), 0) == 1);
            }
            for (int i = 0; i < m_Contents.m_Achievements.Length; i++)
            {
                m_Contents.m_Achievements[i].m_Achieved = (PlayerPrefs.GetInt("m_AchievementsAchieved" + i.ToString(), 0) == 1);
                m_Contents.m_Achievements[i].m_GotReward = (PlayerPrefs.GetInt("m_AchievementsGotReward" + i.ToString(), 0) == 1);
                m_Contents.m_Achievements[i].Counter = PlayerPrefs.GetInt("m_AchievementsCounter" + i.ToString(), 0);
            }

            for (int i = 0; i < m_Contents.m_ProgressionRewards.Length; i++)
            {
                m_Contents.m_ProgressionRewards[i].m_Aquired = (PlayerPrefs.GetInt("m_ProgressionRewardAchieved" + i.ToString(), 0) == 1);
            }


            // for (int i = 0; i < m_Contents.m_LevelupRewards.Length; i++)
            // {
            //     m_Contents.m_LevelupRewards[i].m_Aquired = (PlayerPrefs.GetInt("m_LevelupRewardsAquired" + i.ToString(), 0) == 1);
            // }
            // for (int i = 0; i < m_Contents.m_AllEquipment.Length; i++)
            // {
            //     if (m_Contents.m_AllEquipment[i].m_SpecialPackage)
            //     {
            //         m_Contents.m_AllEquipment[i].m_SpecialPackage = true;
            //     }
            //     else
            //     {
            //         m_Contents.m_AllEquipment[i].m_SpecialPackage = (PlayerPrefs.GetInt("m_SpecialPackUnlocked" + i.ToString(), 0) == 1);
            //     }
            // }
        }

        public void ResetSaveData()
        {
            SaveData();
        }

        public void EarnXP(int xpAmount)
        {
            int m_MaxXP = 20;
            m_PlayerXP = m_PlayerXP + xpAmount;
            if (m_PlayerXP >= m_MaxXP)
            {
                //m_PlayerData.m_PlayerLevel++;
                //m_MaxXP = +m_PlayerData.m_PlayerLevel;
            }
            SaveData();
        }

        public void EarnCoin(int coinAmount)
        {
            Coin = +coinAmount;
            SaveData();
        }

        public void SpendCoin(int coinAmount)
        {
            Coin = -coinAmount;
            Coin = Mathf.Max(Coin, 0);
            SaveData();
        }

        public void EarnGem(int gemAmount)
        {
            Gem = +gemAmount;
            SaveData();
        }

        public void SpendGem(int gemAmount)
        {
            Gem = -gemAmount;
            Gem = Mathf.Max(Gem, 0);
            SaveData();
        }
        // public void UploadDataonDatabase(string itemName)
        // {
        //     if (CheckInternet() && !string.IsNullOrEmpty(m_PlayerData.m_PlayerEmail))
        //     {
        //         Async_UploadData(itemName);
        //     }

        // }
        // async void Async_UploadData(string itemName)
        // {

        //     var saveDataTask = DbControl.m_Current.SaveSpecifiDataToDatabaseBASEAsync(m_PlayerData.m_PlayerEmail, itemName);

        //     await saveDataTask;

        //     if (saveDataTask.Status == TaskStatus.RanToCompletion)
        //     {
        //         Debug.Log("DataSentSuccessfully");
        //     }
        // }

        public bool CheckInternet()
        {
            if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
                return true;

            return false;
        }

        public void SaveItemUnlockes(ScriptableObjBase[] items, string prefix)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].m_Unlocked || items[i].m_UnlockedAtStart)
                    PlayerPrefs.SetInt(prefix + "_Unlock_" + i.ToString(), 1);
                else
                    PlayerPrefs.SetInt(prefix + "_Unlock_" + i.ToString(), 0);
            }
        }

        public void LoadItemUnlockes(ScriptableObjBase[] items, string prefix)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].m_UnlockedAtStart)
                    items[i].m_Unlocked = true;
                else
                    items[i].m_Unlocked = (PlayerPrefs.GetInt(prefix + "_Unlock_" + i.ToString(), 0) == 1);
            }
        }



    }
}
