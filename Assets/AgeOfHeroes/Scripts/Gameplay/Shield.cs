using AgeOfHeroes.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes
{
    public class Shield : MonoBehaviour
    {
        public CharacterG m_Owner;
        public GameObject m_HitParticle;
        public Team m_TargetTeam;
        public GameObject m_Sprite;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(LetLoose());
        }

        // Update is called once per frame
        void Update()
        {
            m_Sprite.transform.Rotate(0, 0, 15);
        }


        IEnumerator LetLoose()
        {

            Vector3 end = Vector3.zero;
            for (int i = 0; i < 3; i++)
            {
                Vector3 start = transform.position;
                if (m_TargetTeam.m_Pawns[i].gameObject.activeSelf)
                {
                    end = m_TargetTeam.m_Pawns[i].transform.position;
                    float lerp = 0;
                    while (lerp < 1)
                    {
                        Vector3 nextPos = Vector3.Lerp(start, end, lerp);
                        transform.position = nextPos;
                        lerp += Time.deltaTime * 2;
                        yield return null;
                    }
                    m_Owner.m_TargetEnemy = m_TargetTeam.m_Pawns[i];
                    m_Owner.HitEvent_1();
                    GameObject obj = Instantiate(m_HitParticle);
                    obj.transform.position = end;
                }
            }
            Vector3 start1 = transform.position;
            end = m_Owner.transform.position;
            float lerp1 = 0;
            while (lerp1 < 1)
            {
                Vector3 nextPos = Vector3.Lerp(start1, end, lerp1);
                transform.position = nextPos;
                lerp1 += Time.deltaTime * 2;
                yield return null;
            }
            m_Owner.m_Shield.gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}