using AgeOfHeroes.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes
{
    public class CharacterC : Pawn
    {

        public GameObject m_CarrotPrefab;
        public Transform m_LoosePoint;
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
                    StartCoroutine(Co_CloseAttack_1());
                    break;
                case 2:
                    StartCoroutine(CoAttack_3());
                    break;
                case 3:
                    break;

            }
        }


        IEnumerator CoAttack_3()
        {
            m_AttackEnded = false;
            SetAnimAttack(2);
            yield return new WaitForSeconds(1);
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
                case "belly":
                    HitEvent_1();
                    break;
                case "throwcarrot":
                    ThrowCarrot();
                    break;
            }
        }



        public void HitEvent_2(Pawn targetEnemy)
        {
            FightCamera.m_Current.SmallShake();
            m_TargetEnemy = targetEnemy;
            ApplyAttackEffect();
            targetEnemy.SetAnimHit();
        }

        public void ThrowCarrot()
        {
            Team targetTeam = m_MyTeam.m_OtherTeam;

            GameObject obj;
            for (int i = 0; i < 3; i++)
            {
                if (targetTeam.m_Pawns[i].gameObject.activeSelf)
                {
                    obj = Instantiate(m_CarrotPrefab);
                    obj.transform.position = m_LoosePoint.position;
                    obj.GetComponent<Carrot>().m_TargetPos = targetTeam.m_Pawns[i].m_ProjectileHitPoint.transform.position;
                    obj.GetComponent<Carrot>().m_Target = targetTeam.m_Pawns[i];
                    obj.GetComponent<Carrot>().m_Owner = this;
                }
            }
        }
    }
}