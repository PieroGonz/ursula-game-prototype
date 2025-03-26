using AgeOfHeroes.Gameplay;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
namespace AgeOfHeroes
{
    public class Axe : MonoBehaviour
    {
        [HideInInspector]
        public CharacterD m_Owner;
        public float Speed;
        public Vector3 m_TargetPos;
        [SerializeField]
        private SpriteRenderer m_Axe;
        [SerializeField]
        private Sprite[] m_Axes;
        // Start is called before the first frame update
        void Start()
        {
            if (m_Owner.m_CharacterData.m_WeaponNum == 0)
            {
                m_Axe.sprite = m_Axes[0];
            }
            else
            {
                m_Axe.sprite = m_Axes[1];
            }
        }

        // Update is called once per frame
        void Update()
        {
            //transform.Rotate(0, 0, -5);

            float distance = Vector3.Distance(transform.position, m_TargetPos);
            if (distance >= 5)
            {
                Vector3 movementDirection = m_TargetPos - transform.position;
                movementDirection.Normalize();
                transform.position += movementDirection * Speed * Time.deltaTime;
            }
            else
            {
                m_Owner.HitTrigger_2();
                Destroy(gameObject);
            }
        }
    }
}