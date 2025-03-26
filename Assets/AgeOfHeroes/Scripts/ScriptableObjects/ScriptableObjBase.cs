using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgeOfHeroes.ScriptableObjects
{
    public class ScriptableObjBase : ScriptableObject
    {
        //        public string m_TitleFarsi = "none";
        public string m_TitleEnglish = "none";
        public int m_PriceInCoin = 1000;
        public int m_PriceInGem = 1000;
        public Sprite m_Icon;
        public Sprite m_IconLocked;
        public bool m_Unlocked = false;
        public bool m_UnlockedAtStart = false;
        public bool m_TempUnlocked = false;
        public bool m_SpecialPackage = false;
        public string m_ItemID = "Item1";



        public int m_ItemLevel = 0;
    }
}

