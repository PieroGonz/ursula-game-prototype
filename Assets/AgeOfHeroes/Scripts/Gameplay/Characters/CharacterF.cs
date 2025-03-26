using AgeOfHeroes.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes
{
    public class CharacterF : Pawn
    {
        public GameObject m_CannonPrefab;
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
                    m_TargetEnemy = m_MyTeam.m_OtherTeam.m_Pawns[0];
                    StartCoroutine(Co_CloseAttack_1());
                    break;
                case 3:
                    break;

            }
        }

        public override void HandleAnimEvent(string eventName)
        {
            base.HandleAnimEvent(eventName);

            switch (eventName)
            {
                case "firecanon":
                    CannonTrigger();
                    break;
                case "punch":
                    HitEvent_1();
                    break;
                case "earthshatter":
                    HitTrigger_3();
                    break;
            }
        }

        IEnumerator CoAttack_1()
        {
            m_AttackEnded = false;
            SetAnimAttack(0);
            yield return new WaitForSeconds(1);
            m_AttackEnded = true;
        }

        public void HitTrigger_3()
        {
            FightCamera.m_Current.SmallShake();
            CreateHitParticle();
            for (int i = 0; i < m_MyTeam.m_OtherTeam.m_Pawns.Length; i++)
            {
                if (m_MyTeam.m_OtherTeam.m_Pawns[i].gameObject.activeSelf)
                {
                    m_TargetEnemy = m_MyTeam.m_OtherTeam.m_Pawns[i];
                    CreateHitParticle();
                    ApplyAttackEffect();
                }
            }

            m_TargetEnemy.SetAnimHit();
        }



        public void CannonTrigger()
        {
            GameObject obj = Instantiate(m_CannonPrefab);
            CreateShootParticle();
            obj.transform.position = m_ShootPoint.transform.position;
            obj.GetComponent<Cannon>().m_TargetPos = m_TargetEnemy.m_ProjectileHitPoint.transform.position;
            obj.GetComponent<Cannon>().m_Owner = this;
        }
    }
}