using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes.ScriptableObjects
{
    [CreateAssetMenu(fileName = "AvatarPartList", menuName = "CustomObjects/AvatarPartList", order = 1)]
    public class AvatarPartList : ScriptableObject
    {
        public AvatarPart[] m_Parts;
    }
}