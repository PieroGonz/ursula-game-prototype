using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgeOfHeroes.ScriptableObjects;

namespace AgeOfHeroes.UI
{
    public class RewardMessageUI : MonoBehaviour
    {
        // Start is called before the first frame update




        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }




        public void BtnOk()
        {
            SoundGallery.PlaySound("button32");
            gameObject.SetActive(false);
        }
    }

}
