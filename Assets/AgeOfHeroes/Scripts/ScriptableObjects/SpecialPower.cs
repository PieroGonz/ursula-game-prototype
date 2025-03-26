using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgeOfHeroes.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SpecialPower", menuName = "Scriptable/SpecialPower", order = 1)]

    public class SpecialPower : ScriptableObjBase
    {
        public GameObject m_FieldPrefab;
    }
}

