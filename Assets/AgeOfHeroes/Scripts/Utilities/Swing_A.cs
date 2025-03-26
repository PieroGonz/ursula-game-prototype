using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes
{
    public class Swing_A : MonoBehaviour
    {
        // Start is called before the first frame update

        public float Speed = 1;
        [HideInInspector]
        public float Lerp = 0;
        [HideInInspector]
        public int Direction;
        [HideInInspector]
        public Vector3 InitPosition;
        public Vector3 Axis;
        public float Radius = 1;
        void Start()
        {
            InitPosition = transform.position;
            Direction = 1;
        }

        // Update is called once per frame
        void Update()
        {
            if (Direction == 1)
            {
                Lerp += Speed * Time.deltaTime;
                transform.position = Vector3.Lerp(InitPosition - 0.5f * Radius * Axis, InitPosition + 0.5f * Radius * Axis, Lerp);
                if (Lerp >= 1)
                {
                    Lerp = 0;
                    Direction = -1;
                }
            }
            else if (Direction == -1)
            {
                Lerp += Speed * Time.deltaTime;
                transform.position = Vector3.Lerp(InitPosition + 0.5f * Radius * Axis, InitPosition - 0.5f * Radius * Axis, Lerp);
                if (Lerp >= 1)
                {
                    Lerp = 0;
                    Direction = 1;
                }
            }
        }
    }
}