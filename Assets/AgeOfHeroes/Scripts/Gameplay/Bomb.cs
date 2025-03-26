using UnityEngine;
using System.Collections;
namespace AgeOfHeroes
{
    public class Bomb : MonoBehaviour
    {


        private Vector3 InitScale;

        [SerializeField]
        private Transform m_Bomb;

        private float m_Countdown = 5;

        private bool m_Carried = false;
        private bool m_Activated = false;

        [SerializeField]
        private GameObject m_ExplodeParticle;

        float m_CanCarryDelay = 0;
        // Use this for initialization
        void Start()
        {
            //m_Carried = false;
            m_Countdown = 3;
            m_Activated = true;
            InitScale = transform.localScale;
        }

        // Update is called once per frame
        void Update()
        {
            //if (m_Carried)
            //{
            //    transform.position = Player.CurrentPlayer.transform.position + new Vector3(0, 3, 0);
            //}

            if (m_Activated)
            {
                m_Countdown -= Time.deltaTime;
                transform.localScale = InitScale + 0.2f * Mathf.Sin((6 + (6 - m_Countdown)) * Time.time) * new Vector3(.3f, 1.1f, .3f);
                if (m_Countdown <= 0)
                {
                    Explode();
                }
            }

            //m_CanCarryDelay -= Time.deltaTime;
            //if (m_CanCarryDelay < 0)
            //    m_CanCarryDelay = 0;
        }

        public void StartCarry()
        {
            if (!m_Carried)
            {
                m_Carried = true;

                if (!m_Activated)
                {
                    m_Activated = true;
                    m_Countdown = 6;
                }
            }

        }

        public void Release()
        {
            m_Carried = false;
            m_CanCarryDelay = 2;
        }


        public void Explode()
        {
            GameObject obj = Instantiate(m_ExplodeParticle);
            obj.transform.position = transform.position;
            Destroy(obj, 4);

            Collider[] colliders = Physics.OverlapSphere(transform.position, 4);
            foreach (Collider c in colliders)
            {
                if (c.gameObject.tag == "Barrier")
                {
                    Destroy(c.gameObject);
                }
                else if (c.gameObject.tag == "NPC")
                {
                    //Destroy(c.gameObject);
                }
            }

            Destroy(gameObject);
        }



        //void OnTriggerEnter(Collider coll)
        //{
        //    //if (!m_Carried && m_CanCarryDelay==0)
        //    {
        //        if (coll.gameObject.tag == "Player")
        //        {
        //            if (!PlayerInventory.MainPlayerInventory.havebomb)
        //            {
        //                
        //                Destroy(gameObject);
        //                //StartCarry();
        //            }
        //        }
        //    }
        //}
    }
}