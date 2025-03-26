using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes.Gameplay
{
    public class Shadow : MonoBehaviour
    {
        public GameObject m_Owner;
        // Start is called before the first frame update
        void Start()
        {
            m_Owner = transform.parent.gameObject;
            transform.SetParent(null);
        }

        // Update is called once per frame
        void LateUpdate()
        {
            Vector3 pos = m_Owner.transform.position;
            pos.y = 0f;
            transform.position = pos;
        }
    }
}