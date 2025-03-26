using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgeOfHeroes.ScriptableObjects;

namespace AgeOfHeroes.UI
{
    public class ServerMessagePanel : MonoBehaviour
    {
        // Start is called before the first frame update




        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }


        public void BtnExit()
        {
            gameObject.SetActive(false);
            SoundGallery.PlaySound("button32");
        }
    }

}
