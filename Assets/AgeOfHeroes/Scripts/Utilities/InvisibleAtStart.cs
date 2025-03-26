using UnityEngine;
using System.Collections;
namespace AgeOfHeroes
{
    public class InvisibleAtStart : MonoBehaviour
    {


        // Use this for initialization
        void Start()
        {
            GetComponent<Renderer>().enabled = false;

        }
    }
}