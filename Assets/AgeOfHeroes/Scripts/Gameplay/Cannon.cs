using AgeOfHeroes.Gameplay;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
namespace AgeOfHeroes
{
    public class Cannon : MonoBehaviour
    {
        [HideInInspector]
        public CharacterF m_Owner;
        public float Speed;
        public Vector3 m_TargetPos;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            float distance = Vector3.Distance(transform.position, m_TargetPos);
            if (distance >= 5)
            {
                Vector3 movementDirection = m_TargetPos - transform.position;
                movementDirection.Normalize();
                transform.position += movementDirection * Speed * Time.deltaTime;
            }
            else
            {
                m_Owner.HitEvent_1();
                Destroy(gameObject);
            }
        }
    }
}