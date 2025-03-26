using AgeOfHeroes.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
namespace AgeOfHeroes
{
    public class CharacterJ : Pawn
    {
        public GameObject m_CrossHair;

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
            ApplyStatToEnemy();
            m_AttackEnded = true;
        }



        IEnumerator CoAttack_3()
        {
            m_AttackEnded = false;
            SetAnimAttack(3);
            yield return new WaitForSeconds(1);
            GameObject obj = Instantiate(m_CrossHair);
            obj.transform.position = new Vector3(0, 0, -100);
            for (int i = 0; i < 3; i++)
            {
                if (m_MyTeam.m_OtherTeam.m_Pawns[i].gameObject.activeSelf)
                {
                    m_TargetEnemy = m_MyTeam.m_OtherTeam.m_Pawns[i];

                    BaseScriptAnim.MoveFromTo(obj.transform, obj.transform.position, m_TargetEnemy.transform.position + new Vector3(0, 30, -100), .5f);
                    yield return new WaitForSeconds(.8f);
                    CreateProjectileHitParticle();
                    FightCamera.m_Current.SmallShake();

                    ApplyAttackEffect();

                    m_TargetEnemy.SetAnimHit();
                    yield return new WaitForSeconds(.5f);
                }
            }
            Destroy(obj);
            yield return new WaitForSeconds(.5f);
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
                case "knife":
                    HitEvent_1();
                    break;
            }
        }

        public void ShootEvent_1()
        {
            FightCamera.m_Current.SmallShake();
            CreateProjectileHitParticle();
            CreateShootParticle();

            ApplyAttackEffect();

            m_TargetEnemy.SetAnimHit();
        }

    }
}