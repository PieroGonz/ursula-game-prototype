using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgeOfHeroes.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Contents", menuName = "Soccer2DCustomObjects/Contents", order = 1)]
    public class Contents : ScriptableObject
    {
        public CharacterSkins[] m_CharacterSkins;
        public CharacterEquipment[] m_CharacterWeapons;
        public SpecialPower[] m_SpecialPower;
        public AvatarPartList[] m_AvatarPartList;
        public Power[] m_Powers;
        public Character[] m_Characters;
        public Stage[] m_Stages;


        public DailyRewardsData[] m_DailyRewards;
        public Level[] m_Levels;
        public Level m_Tutorial;
        public Sprite[] m_PlayerAvatars;

        public AchievementData[] m_Achievements;
        public RewardData[] m_ProgressionRewards;

        public string[] m_ChatMessages;
        public Sprite[] m_ChatEmojis;

        public RewardData[] m_WheelRewards;

        public string[] m_RandomPlayerNames;

        public AnimationCurve m_CamLerp;

    }
}
