using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgeOfHeroes.ScriptableObjects
{
    [CreateAssetMenu(fileName = "AchievementData", menuName = "CustomObjects/AchievementData", order = 1)]
    public class AchievementData : ScriptableObject
    {
        public string m_TitleFarsi = "none";
        public string m_TitleEnglish = "none";
        [TextAreaAttribute]
        public string m_Description = "none";
        public Sprite m_Icon;
        public int CounterMax = 10;
        public int Counter = 0;
        public RewardData m_Reward;

        public bool m_Achieved = false;
        public bool m_GotReward = false;

        public void AddCount()
        {
            if (!m_Achieved)
            {
                Counter++;
                if (Counter >= CounterMax)
                {
                    Counter = CounterMax;
                    m_Achieved = true;
                    m_GotReward = false;
                }
            }
        }
    }
}
