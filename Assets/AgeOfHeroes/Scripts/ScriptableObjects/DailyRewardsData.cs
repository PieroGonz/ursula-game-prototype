using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgeOfHeroes.ScriptableObjects
{
    [CreateAssetMenu(fileName = "DailyRewardsData", menuName = "CustomObjects/DailyRewardsData", order = 1)]
    public class DailyRewardsData : ScriptableObject
    {

        public string m_TitleEnglish = "none";

        public int m_DayNum = 0;
        public RewardData m_Reward;

        public bool m_ReachedDay = false;
        public bool m_GotReward = false;


    }
}
