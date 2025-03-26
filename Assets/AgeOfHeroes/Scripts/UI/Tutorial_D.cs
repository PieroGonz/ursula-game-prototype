using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AgeOfHeroes.ScriptableObjects;
using AgeOfHeroes.Gameplay;
namespace AgeOfHeroes
{
    public class Tutorial_D : MonoBehaviour
    {
        [SerializeField, Space]
        private GameplayData m_GameplayData;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Co_TutorialLoop1());

        }

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator Co_TutorialLoop1()
        {
            Image back = UISystem.FindImage(gameObject, "img-background");
            Image img, img1;

            //tut-1 -- corns
            yield return new WaitForSeconds(15);
            img = UISystem.FindImage(gameObject, "img-tut-1");
            img.gameObject.SetActive(true);
            yield return new WaitForSeconds(5);
            img.gameObject.SetActive(false);

            //tut-2 -- corn maker
            // yield return new WaitForSeconds(1);
            // img = UISystem.FindImage(gameObject, "img-tut-2");
            // img.gameObject.SetActive(true);
            // yield return new WaitForSeconds(5);
            // img.gameObject.SetActive(false);



        }

    }
}