using AgeOfHeroes.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes.ScriptableObjects
{
    public enum AccessoryType
    {
        glove,
        shoe,
        arm,
        wrist
    }

    [CreateAssetMenu(fileName = "Accessory", menuName = "Soccer2DCustomObjects/Accessory", order = 1)]
    public class Accessory : ScriptableObjBase
    {
        public Sprite[] m_Sprites;
        public AccessoryType m_Type;
    }
}
