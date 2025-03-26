using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes.ScriptableObjects
{
    [CreateAssetMenu(fileName = "OpponentData", menuName = "CustomObjects/OpponentData", order = 1)]
    public class OpponentData : ScriptableObject
    {
        // Start is called before the first frame update
        public int m_IconNum;
        public string m_PlayerName;
        public string m_PlayerEmail = "";
        public int m_PlayerLevel;

        public int[] m_CharactersNumber;
        public int[] m_CharactersLevels;
        public int[] m_CharactersSkins;
        public int[] m_CharactersWeapons;

        public string m_ClanName = "";
        public string[] m_MembersIds;
        public bool[] m_MembersOnline;



        //public DataBase_PlayerItemsData m_PlayerEqData;
        public int[] m_AvatarParts;
        public Sprite m_PlayerAvatarSprite;
    }
}
