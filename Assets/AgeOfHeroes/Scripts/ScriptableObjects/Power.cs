using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgeOfHeroes.ScriptableObjects
{

    public enum PowerTypes
    {
        ExtraTime,
        DoubleScore,
        Freeze,
        PowerShot,
        Slow
    }
    [CreateAssetMenu(fileName = "Power", menuName = "Soccer2DCustomObjects/Power", order = 1)]

    public class Power : ScriptableObjBase
    {
        public int m_PowerNum = 0;
    }
}

