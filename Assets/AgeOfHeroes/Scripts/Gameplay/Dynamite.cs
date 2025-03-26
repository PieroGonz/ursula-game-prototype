using AgeOfHeroes.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes
{
    public class Dynamite : MonoBehaviour
    {
        public AnimationCurve m_Curve;

        public Vector3 m_TargetPos;

        public Team m_TargetTeam;

        public CharacterB m_Owner;

        public GameObject m_Explosion;
        public GameObject m_Sprite;

        [HideInInspector]
        public bool m_Exploded = false;


        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Throw(m_TargetPos));
        }

        // Update is called once per frame
        void Update()
        {
            m_Sprite.transform.Rotate(new Vector3(0, 0, 100 * Time.deltaTime));
        }


        IEnumerator Throw(Vector3 endPos)
        {
            m_Exploded = false;
            float lerp = 0;
            Vector3 start = transform.position;
            Vector3 end = endPos;
            while (lerp < 1)
            {
                transform.position = Vector3.Lerp(start, endPos, lerp) + new Vector3(0, m_Curve.Evaluate(lerp), 0);
                lerp += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(.3f);
            FightCamera.m_Current.SmallShake();
            GameObject obj = Instantiate(m_Explosion);
            obj.transform.position = transform.position + new Vector3(0, 0, -10);
            m_Sprite.SetActive(false);

            Pawn mainTarget = m_Owner.m_TargetEnemy;
            for (int i = 0; i < 3; i++)
            {
                m_Owner.m_TargetEnemy = m_TargetTeam.m_Pawns[i];
                if (m_Owner.m_TargetEnemy.gameObject.activeSelf)
                {
                    if (mainTarget == m_Owner.m_TargetEnemy)
                    {
                        m_Owner.ApplyAttackEffect();
                        m_Owner.m_TargetEnemy.SetAnimHit();
                        m_Owner.CreateHitParticle();
                    }
                    else
                    {
                        m_TargetTeam.m_Pawns[i].m_HealthControl.TakeDamage(.4f * m_Owner.CurrentAttackDamage);
                        m_TargetTeam.m_Pawns[i].SetAnimHit();
                    }
                }
            }
            m_Exploded = true;
            yield return new WaitForSeconds(2);
            Destroy(gameObject);

        }
    }
}