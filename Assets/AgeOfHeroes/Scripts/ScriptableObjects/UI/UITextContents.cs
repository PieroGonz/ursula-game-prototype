using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace AgeOfHeroes
{
    [System.Serializable]
    public struct TextData
    {
        public string name;
        [TextAreaAttribute]
        public string text;

    }
    [CreateAssetMenu(fileName = "UITextContents", menuName = "CustomObjects/UITextContents", order = 1)]
    public class UITextContents : ScriptableObject
    {
        [TextAreaAttribute]
        public string[] m_Messages;


        public TextData[] m_Message;
        //public Dictionary<string, Sprite> d_Graphics;
    }
}