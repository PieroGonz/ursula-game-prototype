using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CharacterEquipment", menuName = "Scriptable/CharacterEquipment", order = 1)]

    public class CharacterEquipment : ScriptableObjBase
    {
        public Sprite m_BodySprite;
        public float m_AddedDamage;

    }
}
