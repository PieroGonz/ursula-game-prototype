using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes
{
    public class CharacterBody : MonoBehaviour
    {
        public Rect m_Bound;

        public SpriteRenderer[] m_MainWeaponSprites;

        public delegate void BodyAnimEventHandler(string eventName);
        public event BodyAnimEventHandler OnAnimEvent;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetMainWeapon(Sprite wpnSprite)
        {
            foreach (SpriteRenderer sprite in m_MainWeaponSprites)
            {
                sprite.sprite = wpnSprite;
            }
        }

        private void OnDrawGizmos()
        {
            Vector3 pos = transform.position + Helper.ToVector3(m_Bound.position);
            Gizmos.DrawWireCube(pos, m_Bound.size);
        }

        public void AnimEvent(string eventName)
        {
            OnAnimEvent(eventName);
        }


    }
}