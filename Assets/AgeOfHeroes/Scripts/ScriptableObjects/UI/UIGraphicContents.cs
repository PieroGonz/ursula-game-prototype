using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace AgeOfHeroes
{
    [CreateAssetMenu(fileName = "UIGraphicContents", menuName = "CustomObjects/UIGraphicContents", order = 1)]
    public class UIGraphicContents : ScriptableObject
    {
        public Sprite[] m_Graphics;
        public Dictionary<string, Sprite> d_Graphics;
    }
}