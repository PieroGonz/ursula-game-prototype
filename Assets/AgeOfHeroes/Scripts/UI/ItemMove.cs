using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgeOfHeroes.ScriptableObjects;

namespace AgeOfHeroes.UI
{
    public class ItemMove : MonoBehaviour
    {
        [SerializeField, Space]
        private GameplayData m_GameplayData;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Co_Move());
        }

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator Co_Move()
        {
            RectTransform rectT = GetComponent<RectTransform>();
            Vector3 startPosition = transform.position;
            Vector3 endPosition = Vector3.zero;
            //if (m_GameplayData.m_GameMode == GameModes.Multiplayer)
            //    endPosition=InGameUI.Current.Text_Resource.transform.position;
            //else
            //    endPosition = MultiGameUI.Current.Text_Resource.transform.position;

            float lerp = 0;
            while (true)
            {
                transform.position = Vector3.Lerp(startPosition, endPosition, lerp);
                lerp += 3f * Time.deltaTime;
                if (lerp >= 1)
                    break;
                yield return null;
            }
            transform.position = endPosition;

            //if (m_GameplayData.m_GameMode == GameModes.Multiplayer)
            //    InGameUI.Current.m_CornParticle1.Play();
            //else
            //    endPosition = MultiGameUI.Current.Text_Resource.transform.position;

            Destroy(gameObject);
        }
    }
}