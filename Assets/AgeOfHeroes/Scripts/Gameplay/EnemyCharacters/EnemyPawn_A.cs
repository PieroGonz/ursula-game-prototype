using AgeOfHeroes.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes.Gameplay
{
    public class EnemyPawn_A : Pawn
    {
        [SerializeField]
        private GameObject m_Projectile;
        [SerializeField]
        private GameObject m_ShootPoint;

        private bool m_StatChance;

        public bool m_SecondAttack;
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
                    StartCoroutine(Co_CloseAttack_1());
                    break;
                case 1:
                    StartCoroutine(CoAttack_2());
                    break;
            }
        }


        public override IEnumerator Co_CloseAttack_1()
        {


            m_AttackEnded = false;
            Vector3 delta = new Vector3((-1 * m_MyTeam.m_FaceDirection) * 50, 0, 0);

            Move(transform.position, m_TargetEnemy.transform.position + delta);

            while (!m_ReachedTargetPos)
                yield return null;
            SetAnimAttack(0);

            yield return new WaitForSeconds(2);

            SetDirection(-m_MyTeam.m_FaceDirection);
            Move(transform.position, m_InitPosition);

            while (!m_ReachedTargetPos)
                yield return null;

            SetDirection(m_MyTeam.m_FaceDirection);
            m_AttackEnded = true;
        }

        IEnumerator CoAttack_2()
        {
            m_AttackEnded = false;

            SetAnimAttack(1);
            yield return new WaitForSeconds(2);

            m_AttackEnded = true;
        }

        public override void HandleAnimEvent(string eventName)
        {
            base.HandleAnimEvent(eventName);

            switch (eventName)
            {
                case "hit1":
                    HitTrigger();
                    break;
                case "Shoot":
                    Projectile();
                    break;
                case "hit2":
                    HitTrigger_3();
                    break;
            }
        }

        public void HitTrigger()
        {
            FightCamera.m_Current.SmallShake();
            CreateHitParticle();

            //test
            //m_TargetEnemy.m_HealthControl.TakeDamage(200);

            if (m_StunCount != 0)
            {
                m_TargetEnemy.m_HealthControl.TakeDamage(Random.Range(15, 20));
            }
            else if (m_BuffCount != 0)
            {
                m_TargetEnemy.m_HealthControl.TakeDamage(Random.Range(25, 30));
            }
            else
            {
                m_TargetEnemy.m_HealthControl.TakeDamage(Random.Range(20, 25));
            }

            float randValue = Random.value;
            if (randValue < .75)
            {
                m_StatChance = false;
            }
            else
            {
                m_StatChance = true;
            }

            if (m_StatChance)
            {
                float random = Random.Range(0, 2);
                if (random == 0)
                {
                    m_TargetEnemy.AddStat(0, 5);
                }
                else
                {
                    m_TargetEnemy.AddStat(2, 4);
                }
            }


            m_TargetEnemy.SetAnimHit();
        }

        public void HitTrigger_3()
        {
            FightCamera.m_Current.SmallShake();
            CreateHitParticle();

            //test
            //m_TargetEnemy.m_HealthControl.TakeDamage(200);

            if (m_StunCount != 0)
            {
                m_TargetEnemy.m_HealthControl.TakeDamage(Random.Range(23, 28));
            }
            else if (m_BuffCount != 0)
            {
                m_TargetEnemy.m_HealthControl.TakeDamage(Random.Range(34, 39));
            }
            else
            {
                m_TargetEnemy.m_HealthControl.TakeDamage(Random.Range(28, 34));
            }

            float randValue = Random.value;
            if (randValue < .65)
            {
                m_StatChance = false;
            }
            else
            {
                m_StatChance = true;
            }

            if (m_StatChance)
            {
                float random = Random.Range(0, 2);
                if (random == 0)
                {
                    m_TargetEnemy.AddStat(0, 5);
                }
                else
                {
                    m_TargetEnemy.AddStat(2, 4);
                }
            }


            m_TargetEnemy.SetAnimHit();
        }


        public void Projectile()
        {
            GameObject obj = Instantiate(m_Projectile);
            obj.transform.position = m_ShootPoint.transform.position;
            obj.GetComponent<EnemyProjectile>().m_TargetPos = m_TargetEnemy.m_ProjectileHitPoint.transform.position;
            obj.GetComponent<EnemyProjectile>().m_Owner = this;
        }

        public void HitTrigger_2()
        {
            FightCamera.m_Current.SmallShake();
            CreateHitParticle();
            if (m_StunCount != 0)
            {
                m_TargetEnemy.m_HealthControl.TakeDamage(Random.Range(20, 25));
            }
            else if (m_BuffCount != 0)
            {
                m_TargetEnemy.m_HealthControl.TakeDamage(Random.Range(30, 35));
            }
            else
            {
                m_TargetEnemy.m_HealthControl.TakeDamage(Random.Range(25, 30));
            }
            m_TargetEnemy.SetAnimHit();
        }


    }
}
