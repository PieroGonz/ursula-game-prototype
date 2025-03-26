using AgeOfHeroes.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.UI;


namespace AgeOfHeroes.Gameplay
{
    public class Pawn : MonoBehaviour
    {
        [HideInInspector]
        public Team m_MyTeam;
        [HideInInspector]
        public int m_ID;
        public CharacterHealth m_HealthControl;
        [HideInInspector]
        public int[] m_CoolDown = new int[3];
        public GameObject m_HitPoint;
        [HideInInspector]
        public int m_StunCount = 0;
        [HideInInspector]
        public int m_BleedCount = 0;
        [HideInInspector]
        public int m_BuffCount = 0;

        [SerializeField]
        private SpriteRenderer[] m_WeaponSprites;

        [HideInInspector]
        public Pawn m_TargetEnemy;

        public Character m_CharacterData;

        public Animator m_Animator;

        public GameObject m_BodyBase;

        public GameObject m_HitParticle;
        public GameObject m_GunParticle;
        public GameObject m_DeathParticle;
        public GameObject m_ProjectileHitPoint;

        [HideInInspector]
        public GameObject m_HighlightMark;

        [HideInInspector]
        public GameObject m_StunPartcile;
        [HideInInspector]
        public GameObject m_BuffPartcile;
        [HideInInspector]
        public GameObject m_BleedPartcile;

        public GameObject m_MarkPrefab;

        [HideInInspector]
        public bool m_AttackEnded = false;

        private int[] m_CoolDownDecrease = new int[3];

        [HideInInspector]
        public int m_AttackNum = 0;

        [HideInInspector]
        public int CurrentAttackDamage = 0;

        [HideInInspector]
        public bool m_ReachedTargetPos;

        [HideInInspector]
        public Vector3 m_InitPosition;

        public Transform m_Head;

        [HideInInspector]
        public int m_WeaponNum;

        [HideInInspector]
        public int m_CharacterLevel;

        [HideInInspector]
        public bool m_IsOpponent = false;
        // Start is called before the first frame update
        void Start()
        {



            if (m_Animator != null)
            {
                CharacterBody body = m_Animator.gameObject.GetComponent<CharacterBody>();
                body.OnAnimEvent += HandleAnimEvent;
            }

            m_HighlightMark = Instantiate(m_MarkPrefab);
            m_HighlightMark.transform.SetParent(transform, true);
            m_HighlightMark.transform.localPosition = new Vector3(0, 0, 1);
            m_HighlightMark.gameObject.SetActive(false);

            m_InitPosition = transform.position;

            SetWeapon();
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void SetWeapon()
        {
            for (int i = 0; i < m_WeaponSprites.Length; i++)
            {
                // print(m_CharacterData.name);
                m_WeaponSprites[i].sprite = m_CharacterData.m_Weapons[m_WeaponNum].m_BodySprite;
            }
        }

        public void ShowHighlight()
        {
            m_HighlightMark.gameObject.SetActive(true);
        }

        public void HideHighlight()
        {
            m_HighlightMark.gameObject.SetActive(false);
        }

        public void SetAnimStand()
        {
            m_Animator.Play("stand-1");
        }
        public void SetAnimAttack(int attackNum)
        {
            m_Animator.Play("attack-" + (attackNum + 1).ToString());
        }

        public void SetAnimRun()
        {
            m_Animator.Play("run-1");
        }

        public void SetAnimReady()
        {
            m_Animator.Play("ready-1");
        }

        public void SetAnimHit()
        {
            m_Animator.Play("hit-1");
        }

        public void SetAnimDeath()
        {
            m_Animator.Play("death-1");
        }

        public void CreateHitParticle()
        {
            GameObject obj = Instantiate(GameControl.m_Current.m_GameplayContents.m_HitParticles[0]);
            obj.transform.position = m_TargetEnemy.transform.position + new Vector3(0, 50, -5);
            //obj.transform.position = new Vector3(0, 50, -5);
            Destroy(obj, 2);
        }

        public void CreateHitParticleBlade()
        {
            GameObject obj = Instantiate(GameControl.m_Current.m_GameplayContents.m_HitParticles[1]);
            obj.transform.position = m_TargetEnemy.transform.position + new Vector3(0, 50, -5);
            Destroy(obj, 2);
        }

        public void CreateShootParticle()
        {
            GameObject obj = Instantiate(m_GunParticle);
            obj.transform.position = m_HitPoint.transform.position;
            Destroy(obj, 2);
        }

        public void CreateProjectileHitParticle()
        {
            GameObject obj = Instantiate(m_HitParticle);
            obj.transform.position = m_TargetEnemy.m_ProjectileHitPoint.transform.position;
            Destroy(obj, 2);
        }

        public void CheckStats()
        {
            if (m_StunCount > 0)
            {
                m_StunCount--;
                if (m_StunCount <= 0)
                {
                    HideStatParticle(0);
                }
            }

            if (m_BuffCount > 0)
            {
                m_BuffCount--;
                if (m_BuffCount <= 0)
                {
                    HideStatParticle(1);
                }
            }

            if (m_BleedCount > 0)
            {
                m_HealthControl.TakeDamage(5);
                m_BleedCount--;
                if (m_BleedCount <= 0)
                {
                    HideStatParticle(2);
                }
            }


            for (int i = 0; i < m_CoolDown.Length; i++)
            {

                if (m_CoolDown[i] > 0)
                {
                    if (m_CoolDownDecrease[i] > 0)
                    {
                        m_CoolDownDecrease[i]--;
                    }
                    else
                    {
                        m_CoolDown[i]--;
                        m_CoolDownDecrease[i] = 2;
                    }

                }


            }


        }

        public IEnumerator Co_Move(Vector3 start, Vector3 end)
        {
            SetAnimRun();
            m_ReachedTargetPos = false;
            start.z = 0;
            end.z = 0;
            float speed = 300f / Vector3.Distance(start, end);
            float lerp = 0;
            Vector3 startPos = start;
            Vector3 endPos = end;
            while (lerp <= 1)
            {
                Vector3 pos = Vector3.Lerp(startPos, endPos, lerp);
                pos.z = .2f * pos.y;
                transform.position = pos;
                lerp += speed * Time.deltaTime;
                yield return null;
            }
            transform.position = endPos;
            m_ReachedTargetPos = true;
            SetAnimStand();
        }

        public virtual void Move(Vector3 start, Vector3 end)
        {
            StartCoroutine(Co_Move(start, end));
        }

        public virtual void Attack(int attackType)
        {

        }

        public virtual void HandleAnimEvent(string eventName)
        {

        }

        public void SetDirection(int dir)
        {
            m_BodyBase.transform.localScale = new Vector3(dir, 1, 1);
        }

        public void AddStat(int num, int count)
        {
            switch (num)
            {
                case 0:
                    m_StunCount += count;
                    ShowStatParticle(0);
                    m_BuffCount = 0;
                    HideStatParticle(1);
                    break;

                case 1:
                    m_BuffCount += count;
                    ShowStatParticle(1);
                    m_StunCount = 0;
                    HideStatParticle(0);
                    break;

                case 2:
                    m_BleedCount += count;
                    ShowStatParticle(2);
                    break;
            }
        }

        public void ShowStatParticle(int num)
        {
            switch (num)
            {
                case 0:
                    if (m_StunPartcile == null)
                    {
                        m_StunPartcile = Instantiate(GameControl.m_Current.m_GameplayContents.m_StatParticles[0]);
                        m_StunPartcile.transform.SetParent(transform);
                        m_StunPartcile.transform.position = m_Head.position + new Vector3(0, 5, -3);
                    }
                    break;
                case 1:
                    if (m_BuffPartcile == null)
                    {
                        m_BuffPartcile = Instantiate(GameControl.m_Current.m_GameplayContents.m_StatParticles[1]);
                        m_BuffPartcile.transform.SetParent(transform);
                        m_BuffPartcile.transform.position = m_Head.position + new Vector3(0, 5, -3);
                    }
                    break;

                case 2:
                    if (m_BleedPartcile == null)
                    {
                        m_BleedPartcile = Instantiate(GameControl.m_Current.m_GameplayContents.m_StatParticles[2]);
                        m_BleedPartcile.transform.SetParent(transform);
                        m_BleedPartcile.transform.position = m_Head.position + new Vector3(0, 5, -3);
                    }
                    break;


            }
        }

        public void HideStatParticle(int num)
        {
            switch (num)
            {
                case 0:
                    if (m_StunPartcile != null)
                    {
                        Destroy(m_StunPartcile);
                        //m_StunPartcile.gameObject.SetActive(false);
                        m_StunPartcile = null;
                    }
                    break;

                case 1:
                    if (m_BuffPartcile != null)
                    {
                        Destroy(m_BuffPartcile);
                        //m_BuffPartcile.gameObject.SetActive(false);
                        m_BuffPartcile = null;
                    }
                    break;

                case 2:
                    if (m_BleedPartcile != null)
                    {
                        Destroy(m_BleedPartcile);
                        //m_BleedPartcile.gameObject.SetActive(false);
                        m_BleedPartcile = null;
                    }
                    break;
            }
        }



        public void Dead()
        {
            if (gameObject.activeSelf)
            {
                StartCoroutine(Co_Dead());
            }
        }

        IEnumerator Co_Dead()
        {
            SetAnimDeath();
            yield return new WaitForSeconds(1);

            GameObject obj = Instantiate(m_DeathParticle);
            obj.transform.position = transform.position + new Vector3(0, 20, -5);
            Destroy(obj, 3);

            gameObject.SetActive(false);
        }

        public virtual IEnumerator Co_CloseAttack_1()
        {
            CharAbility ability = m_CharacterData.m_Abilities[m_AttackNum];
            m_AttackEnded = false;
            Vector3 delta = new Vector3((-1 * m_MyTeam.m_FaceDirection) * ability.m_AttackDistance, 0, 0);

            Vector3 camPos = m_TargetEnemy.transform.position + delta + new Vector3(0, 30, 0);
            FightCamera.m_Current.MoveCamera(camPos, .7f);
            FightCamera.m_Current.ZoomCamera(120, .7f);

            Move(transform.position, m_TargetEnemy.transform.position + delta);
            while (!m_ReachedTargetPos)
                yield return null;

            SetAnimAttack(m_AttackNum);

            yield return new WaitForSeconds(2);

            FightCamera.m_Current.MoveCamera(FightCamera.m_Current.m_InitPosition, .5f);
            FightCamera.m_Current.ZoomCamera(150, .5f);

            SetDirection(-m_MyTeam.m_FaceDirection);
            Move(transform.position, m_InitPosition);

            while (!m_ReachedTargetPos)
                yield return null;

            SetDirection(m_MyTeam.m_FaceDirection);
            m_AttackEnded = true;
        }

        public void HitEvent_1()
        {
            CharAbility ability = m_CharacterData.m_Abilities[m_AttackNum];
            FightCamera.m_Current.SmallShake();

            if (ability.m_Blade)
            {
                CreateHitParticleBlade();
            }
            else
            {
                CreateHitParticle();
            }

            ApplyAttackEffect();

            m_TargetEnemy.SetAnimHit();
        }

        public float CalculateDamage()
        {
            CharAbility ability = m_CharacterData.m_Abilities[m_AttackNum];
            float damage = 5;
            if (m_CharacterData != null)
            {
                damage = Random.Range(ability.m_BaseDamageRangeMin, ability.m_BaseDamageRangeMax);
                damage += m_CharacterData.m_Damage + 4 * m_CharacterLevel;
                damage += m_CharacterData.m_Weapons[m_WeaponNum].m_AddedDamage;

                if (m_StunCount != 0)
                {
                    damage -= 5;
                }
                else if (m_BuffCount != 0)
                {
                    damage += 10;
                }
            }

            damage = Mathf.Clamp(damage, 0, 300);

            return damage;
        }

        public void ApplyStatToEnemy()
        {
            CharAbility ability = m_CharacterData.m_Abilities[m_AttackNum];

            if (ability.m_Stun)
                m_TargetEnemy.AddStat(0, 6);

            if (ability.m_Bleed)
                m_TargetEnemy.AddStat(2, 4);

        }

        public void ApplyAttackEffect()
        {
            ApplyStatToEnemy();
            m_TargetEnemy.m_HealthControl.TakeDamage(CurrentAttackDamage);
        }


        public void ApplyAbilityCooldown()
        {

            if (m_AttackNum == 0)
            {
                m_CoolDown[0] = m_CharacterData.m_Abilities[0].m_CoolDown;
                m_CoolDownDecrease[0] = 2;
            }

            if (m_AttackNum == 1)
            {
                m_CoolDown[1] = m_CharacterData.m_Abilities[1].m_CoolDown;
                m_CoolDownDecrease[1] = 2;
            }

            if (m_AttackNum == 2)
            {
                m_CoolDown[2] = m_CharacterData.m_Abilities[2].m_CoolDown;
                m_CoolDownDecrease[2] = 2;
            }

        }

    }
}
