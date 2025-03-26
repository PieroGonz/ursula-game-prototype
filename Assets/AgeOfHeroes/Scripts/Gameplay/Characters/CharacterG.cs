using AgeOfHeroes.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes
{
    public class CharacterG : Pawn
    {

        public GameObject m_ShieldPrefab;
        public GameObject m_Shield;
        public GameObject m_ThrowPoint;
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

            yield return new WaitForSeconds(2);
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
                case "hit2":
                    HitEvent_1();
                    break;
                case "throwshield":
                    Throw();
                    break;
            }
        }



        public void Throw()
        {
            m_Shield.gameObject.SetActive(false);
            GameObject obj = Instantiate(m_ShieldPrefab);
            obj.transform.position = m_ThrowPoint.transform.position;
            obj.GetComponent<Shield>().m_TargetTeam = m_MyTeam.m_OtherTeam;
            obj.GetComponent<Shield>().m_Owner = this;
        }
    }
}