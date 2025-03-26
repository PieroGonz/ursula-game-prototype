using AgeOfHeroes.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes
{
    public class Arrow : MonoBehaviour
    {

        public AnimationCurve m_Curve;
        public CharacterE m_Owner;
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

        }


        IEnumerator LetLoose(Vector3 endPos)
        {
            float lerp = 0;
            Vector3 start = transform.position;
            Vector3 end = endPos;
            while (lerp < 1)
            {
                Vector3 nextPos = Vector3.Lerp(start, endPos, lerp) + new Vector3(0, m_Curve.Evaluate(lerp), 0);
                Vector3 direction = nextPos - transform.position;
                transform.right = direction;
                transform.position = nextPos;
                lerp += Time.deltaTime;
                yield return null;
            }
            m_Owner.m_TargetEnemy = m_Target;
            m_Owner.HitEvent_1();
            GameObject obj = Instantiate(m_HitParticle);
            obj.transform.position = endPos;
            yield return new WaitForSeconds(.1f);
            Destroy(gameObject);

        }
    }
}