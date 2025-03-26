using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AgeOfHeroes.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Soccer2DCustomObjects/PlayerData", order = 1)]
    public class PlayerData : ScriptableObject
    {
        public string m_PlayerName = "Player";
        public string m_PlayerEmail = "";
        public string m_PlayerPassword = "";
        public string m_OnlineUID = "";
        public int m_UserID = 0;
        public string m_AndroidID = "";

        public int m_PlayerLevel = 0;
        public int m_PlayerImageNum = 0;

        public int[] m_PlayerAvatarParts;

        public Texture2D m_DefaultAvatarImage;


        public Sprite m_PlayerAvatarSprite;

        public void SaveData()
        {
            PlayerPrefs.SetString("m_PlayerName", m_PlayerName);
            PlayerPrefs.SetString("m_PlayerEmail", m_PlayerEmail);
            PlayerPrefs.SetString("m_PlayerPassword", m_PlayerPassword);
            PlayerPrefs.SetInt("m_PlayerLevel", m_PlayerLevel);
            PlayerPrefs.SetInt("m_PlayerImageNum", m_PlayerImageNum);
            PlayerPrefs.SetInt("m_UserID", m_UserID);
            PlayerPrefs.SetString("m_AndroidID", m_AndroidID);

            for (int i = 0; i < 10; i++)
            {
                PlayerPrefs.SetInt("m_PlayerAvatarParts" + i.ToString(), m_PlayerAvatarParts[i]);
            }
            PlayerPrefs.Save();
        }

        public void LoadData()
        {
            m_PlayerName = PlayerPrefs.GetString("m_PlayerName", "Player");
            m_PlayerEmail = PlayerPrefs.GetString("m_PlayerEmail", "");
            m_PlayerPassword = PlayerPrefs.GetString("m_PlayerPassword", "");
            m_PlayerLevel = PlayerPrefs.GetInt("m_PlayerLevel", 0);
            m_PlayerImageNum = PlayerPrefs.GetInt("m_PlayerImageNum", 0);
            m_UserID = PlayerPrefs.GetInt("m_UserID", 0);
            m_AndroidID = PlayerPrefs.GetString("m_AndroidID", "");

            m_PlayerAvatarParts = new int[10];
            for (int i = 0; i < 10; i++)
            {
                m_PlayerAvatarParts[i] = PlayerPrefs.GetInt("m_PlayerAvatarParts" + i.ToString(), 0);

            }
        }
    }
}