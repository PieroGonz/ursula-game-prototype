using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes
{
    public class Spin : MonoBehaviour
    {
        public float Speed;
        public Vector3 Axis;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            transform.rotation = Quaternion.Euler(Time.deltaTime * Speed * Axis) * transform.rotation;
        }
    }
}