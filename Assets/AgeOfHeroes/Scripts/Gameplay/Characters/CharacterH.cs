using AgeOfHeroes.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes
{
    public class CharacterH : Pawn
    {
        public GameObject m_ProjectilePrefab;
        public Transform m_ShootPoint;
        // Start is called before the first frame update


        // Update is called once per frame
        void Update()
        {


        }

        public override void Attack(int attackType)
        {
            base.Attack(attackType);
            switch (attackType)
            {
                case 0:
                    StartCoroutine(CoAttack_1());
                    break;
                case 1:
                    StartCoroutine(Co_CloseAttack_1());
                    break;
                case 2:
                    StartCoroutine(CoAttack_3());
                    break;
                case 3:
                    break;

            }
        }

        IEnumerator CoAttack_1()
        {
            m_AttackEnded = false;
            SetAnimAttack(0);
            yield return new WaitForSeconds(2);
            m_AttackEnded = true;
        }



        IEnumerator CoAttack_3()
        {
            m_AttackEnded = false;
            SetAnimAttack(2);
            yield return new WaitForSeconds(2);
            m_AttackEnded = true;
        }

        public override void HandleAnimEvent(string eventName)
        {
            base.HandleAnimEvent(eventName);

            switch (eventName)
            {
                case "projectile":
                    StaffProjectile();
                    break;
                case "hit1":
                    HitEvent_1();
                    break;
                case "heal":
                    Heal();
                    break;
            }
        }

        public void Heal()
        {
            m_MyTeam.HealTeam();
        }


        public void StaffProjectile()
        {
            GameObject obj = Instantiate(m_ProjectilePrefab);
            obj.transform.position = m_ShootPoint.transform.position;
            obj.GetComponent<Projectile>().m_TargetPos = m_TargetEnemy.m_ProjectileHitPoint.transform.position;
            obj.GetComponent<Projectile>().m_Owner = this;
        }

    }
}