using System.Collections;
using System.Collections.Generic;
using AgeOfHeroes.ScriptableObjects;
using UnityEngine;

namespace AgeOfHeroes.ScriptableObjects
{
    [CreateAssetMenu(fileName = "AvatarPart", menuName = "CustomObjects/AvatarPart", order = 1)]
    public class AvatarPart : ScriptableObjBase
    {
        [Space]
        public int m_PartType = 0;
        public Sprite m_PartSprite;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
