using AgeOfHeroes.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes
{
    public class Carrot : MonoBehaviour
    {
        public CharacterC m_Owner;
        public Vector3 m_TargetPos;
        public Pawn m_Target;
        public GameObject m_HitParticle;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(LetLoose(m_TargetPos));
        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(0, 0, 1000 * Time.deltaTime);
        }


        IEnumerator LetLoose(Vector3 endPos)
        {
            float lerp = 0;
            Vector3 start = transform.position;
            Vector3 end = endPos;
            while (lerp < 1)
            {
                Vector3 nextPos = Vector3.Lerp(start, endPos, lerp);
                Vector3 direction = nextPos - transform.position;
                transform.position = nextPos;
                lerp += Time.deltaTime * 2;
                yield return null;
            }
            m_Owner.HitEvent_2(m_Target);
            GameObject obj = Instantiate(m_HitParticle);
            obj.transform.position = endPos;
            Destroy(gameObject);

        }
    }
}