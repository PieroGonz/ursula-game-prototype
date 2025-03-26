using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes
{
    public class ZDepthUpdate : MonoBehaviour
    {
        public float ZOffset = 0;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Vector3 pos = transform.position;
            pos.z = .4f * pos.y + ZOffset;
            transform.position = pos;
        }
    }
}
