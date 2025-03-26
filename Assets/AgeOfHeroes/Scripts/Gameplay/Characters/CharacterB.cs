using AgeOfHeroes.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes
{
    public class CharacterB : Pawn
    {
        public GameObject m_DynamitePrefab;
        public Transform m_ThrowPoint;


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
            yield return new WaitForSeconds(1);
            ApplyStatToEnemy();
            m_AttackEnded = true;
        }



        IEnumerator CoAttack_3()
        {
            m_AttackEnded = false;
            SetAnimAttack(2);
            yield return new WaitForSeconds(3);
            m_AttackEnded = true;
        }

        public override void HandleAnimEvent(string eventName)
        {
            base.HandleAnimEvent(eventName);

            switch (eventName)
            {
                case "shoot1":
                    ShootEvent_1();
                    break;
                case "shoot2":
                    ShootEvent_1();
                    break;
                case "kick":
                    HitEvent_1();
                    break;
                case "throw":
                    ThrowDynamiteTrigger();
                    break;
            }
        }

        public void ShootEvent_1()
        {
            FightCamera.m_Current.SmallShake();
            CreateProjectileHitParticle();
            CreateShootParticle();


            m_TargetEnemy.m_HealthControl.TakeDamage(.5f * CurrentAttackDamage);

            m_TargetEnemy.SetAnimHit();
        }


        public void ThrowDynamiteTrigger()
        {
            GameObject obj = Instantiate(m_DynamitePrefab);
            obj.transform.position = m_ThrowPoint.position;
            obj.GetComponent<Dynamite>().m_TargetTeam = m_MyTeam.m_OtherTeam;
            obj.GetComponent<Dynamite>().m_TargetPos = m_TargetEnemy.transform.position;
            obj.GetComponent<Dynamite>().m_Owner = this;
        }
    }
}