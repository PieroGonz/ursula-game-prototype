using AgeOfHeroes.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes
{
    public class CharacterD : Pawn
    {
        public GameObject m_AxePrefab;
        public Transform[] m_LoosePoint;

        public GameObject m_TeleportParticle;
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
                case 2:
                    StartCoroutine(CoAttack_3());
                    break;
                case 3:
                    break;

            }
        }



        IEnumerator CoAttack_2()
        {
            m_AttackEnded = false;
            SetAnimAttack(1);
            yield return new WaitForSeconds(2);
            ApplyStatToEnemy();
            m_AttackEnded = true;
        }

        public void CreateTeleportParticle()
        {
            GameObject obj = Instantiate(m_TeleportParticle);
            obj.transform.position = transform.position + new Vector3(0, 30, -4);
            Destroy(obj, 2);
        }

        IEnumerator CoAttack_3()
        {
            m_AttackEnded = false;
            Vector3 delta = new Vector3(-60 * m_MyTeam.m_FaceDirection, 0, 0);

            yield return new WaitForSeconds(.2f);
            m_BodyBase.SetActive(false);
            CreateTeleportParticle();
            yield return new WaitForSeconds(.5f);

            for (int i = 0; i < 3; i++)
            {
                if (m_MyTeam.m_OtherTeam.m_Pawns[i].gameObject.activeSelf)
                {
                    m_TargetEnemy = m_MyTeam.m_OtherTeam.m_Pawns[i];
                    transform.position = m_TargetEnemy.transform.position + delta;
                    m_BodyBase.SetActive(true);
                    CreateTeleportParticle();
                    SetAnimAttack(2);
                    yield return new WaitForSeconds(.8f);
                    m_BodyBase.SetActive(false);
                    CreateTeleportParticle();
                    yield return new WaitForSeconds(.5f);
                }
            }

            transform.position = m_InitPosition;
            m_BodyBase.SetActive(true);
            m_AttackEnded = true;
        }

        public override void HandleAnimEvent(string eventName)
        {
            base.HandleAnimEvent(eventName);

            switch (eventName)
            {
                case "hit1":
                    HitEvent_1();
                    break;
                case "LetLoose":
                    AxeTrigger();
                    break;
                case "hit2":
                    HitTrigger_2();
                    break;
                case "hit3":
                    HitEvent_1();
                    break;
            }
        }



        public void AxeTrigger()
        {
            GameObject obj = Instantiate(m_AxePrefab);
            obj.transform.position = m_LoosePoint[Random.Range(0, 2)].transform.position;
            obj.GetComponent<Axe>().m_TargetPos = m_TargetEnemy.m_ProjectileHitPoint.transform.position;
            obj.GetComponent<Axe>().m_Owner = this;
        }

        public void HitTrigger_2()
        {
            FightCamera.m_Current.SmallShake();
            CreateProjectileHitParticle();

            m_TargetEnemy.m_HealthControl.TakeDamage(.3f * CurrentAttackDamage);

            m_TargetEnemy.SetAnimHit();
        }

    }
}