using AgeOfHeroes.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes
{
    public class CharacterE : Pawn
    {
        public GameObject m_ArrowPrefab;
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

        public override void HandleAnimEvent(string eventName)
        {
            base.HandleAnimEvent(eventName);

            switch (eventName)
            {
                case "LetLoose":
                    ArrowTrigger();
                    break;
                case "LetLoose_2":
                    ArrowTrigger_2();
                    break;
                case "hit2":
                    HitEvent_1();
                    break;
            }
        }


        IEnumerator CoAttack_1()
        {
            m_AttackEnded = false;
            SetAnimAttack(0);
            yield return new WaitForSeconds(2.5f);
            m_AttackEnded = true;
        }


        IEnumerator CoAttack_3()
        {
            m_AttackEnded = false;
            SetAnimAttack(2);
            yield return new WaitForSeconds(4);
            m_AttackEnded = true;
        }

        public void ArrowTrigger()
        {
            GameObject obj = Instantiate(m_ArrowPrefab);
            obj.transform.position = m_LoosePoint.position;
            obj.GetComponent<Arrow>().m_TargetPos = m_TargetEnemy.m_ProjectileHitPoint.transform.position;
            obj.GetComponent<Arrow>().m_Target = m_TargetEnemy;
            obj.GetComponent<Arrow>().m_Owner = this;
        }

        public void ArrowTrigger_2()
        {

            for (int i = 0; i < 3; i++)
            {
                if (m_MyTeam.m_OtherTeam.m_Pawns[i].gameObject.activeSelf)
                {
                    GameObject obj = Instantiate(m_ArrowPrefab);
                    obj.transform.position = m_LoosePoint.position;
                    obj.GetComponent<Arrow>().m_TargetPos = m_MyTeam.m_OtherTeam.m_Pawns[i].m_ProjectileHitPoint.transform.position;
                    obj.GetComponent<Arrow>().m_Target = m_MyTeam.m_OtherTeam.m_Pawns[i];
                    obj.GetComponent<Arrow>().m_Owner = this;
                }
            }


        }
    }
}