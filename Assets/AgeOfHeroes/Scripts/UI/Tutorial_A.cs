using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AgeOfHeroes.ScriptableObjects;
using AgeOfHeroes.Gameplay;
namespace AgeOfHeroes
{
    public class Tutorial_A : MonoBehaviour
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
            yield return new WaitForSeconds(7);
            //Image back = UISystem.FindImage(gameObject, "img-background");
            //Image img, img1;

            ////tut-1 -- enemies
            //yield return new WaitForSeconds(7);
            //img = UISystem.FindImage(gameObject, "img-tut-1");
            //img.gameObject.SetActive(true);
            //yield return new WaitForSeconds(5);
            //img.gameObject.SetActive(false);

            ////tut-2 -- pick corn
            //yield return new WaitForSeconds(.5f);
            //img = UISystem.FindImage(gameObject, "img-tut-2");
            //img.gameObject.SetActive(true);
            //img1 = UISystem.FindImage(img.gameObject, "img-hand-1");
            //img1.rectTransform.anchoredPosition = Helper.WorldToUIPosition(new Vector3(0, -30, 0),
            // CameraControl.Current.GetComponent<Camera>(),
            //  UISystem.m_Main.m_GeneralCanvasSize);
            //yield return new WaitForSeconds(2);
            //while (!GameControl.Current.m_CollectedFirstCorn)
            //{
            //    yield return null;
            //}
            //img.gameObject.SetActive(false);

            ////tut-3 -- select chick
            //yield return new WaitForSeconds(.5f);
            //img = UISystem.FindImage(gameObject, "img-tut-3");
            //img.gameObject.SetActive(true);
            //yield return new WaitForSeconds(1);
            //while (DefenceControl.Current.m_DefenseState != DefenseStates.create)
            //{
            //    yield return null;
            //}
            //img.gameObject.SetActive(false);

            ////tut-4 -- place chick
            //yield return new WaitForSeconds(.5f);
            //img = UISystem.FindImage(gameObject, "img-tut-4");
            //img.gameObject.SetActive(true);
            //img1 = UISystem.FindImage(img.gameObject, "img-hand-1");
            //Vector3 handPos = GridControl.Current.m_GridRows[2].m_GridPoints[0].transform.position;
            //handPos += new Vector3(40, -40, 0);
            //img1.rectTransform.anchoredPosition = Helper.WorldToUIPosition(handPos,
            // CameraControl.Current.GetComponent<Camera>(),
            //  UISystem.m_Main.m_GeneralCanvasSize);
            //yield return new WaitForSeconds(1);
            //while (DefenceControl.Current.m_DefenseState == DefenseStates.create)
            //{
            //    yield return null;
            //}
            //img.gameObject.SetActive(false);


        }


    }
}