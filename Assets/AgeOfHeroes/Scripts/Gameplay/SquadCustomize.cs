using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgeOfHeroes.ScriptableObjects;
namespace AgeOfHeroes
{
    public class SquadCustomize : MonoBehaviour
    {
        [HideInInspector]
        public GameObject[] m_Characters;
        public Transform[] m_CharacterPoints;

        public Vector3[] m_Positions;
        public Vector3[] m_DefaultPositions;

        public Camera[] m_Cameras;
        public Transform[] m_Shadows;

        public int m_CharacterSelectNum = 0;

        public int[] m_CharacterNums = new int[3];
        public int[] m_CharSkinNums = new int[3];
        public int[] m_CharWeaponNums = new int[3];

        [SerializeField]
        private Contents m_Content;
        [SerializeField]
        private DataStorage m_DataStorage;

        public static SquadCustomize m_Main;

        private void Awake()
        {
            m_Main = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            m_DefaultPositions = new Vector3[3];
            for (int i = 0; i < 3; i++)
            {
                m_DefaultPositions[i] = m_CharacterPoints[i].localPosition;
            }

            m_CharSkinNums = new int[3];
            m_CharWeaponNums = new int[3];
            m_Characters = new GameObject[3];
            for (int i = 0; i < m_Characters.Length; i++)
            {
                m_CharacterNums[i] = m_DataStorage.m_CharactersNumber[i];
                m_CharSkinNums[i] = m_Content.m_Characters[m_CharacterNums[i]].m_SkinNum;
                m_CharWeaponNums[i] = m_Content.m_Characters[m_CharacterNums[i]].m_WeaponNum;
            }


            CreateTeam();
        }

        public void CreatePlayer(int num)
        {
            if (m_Characters[num] != null)
            {
                Destroy(m_Characters[num]);
            }

            Character characterData = m_Content.m_Characters[m_CharacterNums[num]];
            CharacterSkins skin = characterData.m_Skins[m_CharSkinNums[num]];
            GameObject bodyObj;
            bodyObj = Instantiate(skin.m_BaseBodyPrefab);
            CharacterBody body = bodyObj.GetComponent<CharacterBody>();
            //m_Cameras[num].orthographicSize = body.m_Bound.size.y / 2f;

            //bodyObj.transform.position = m_Cameras[num].transform.position + new Vector3(0, -body.m_Bound.y, 10);
            bodyObj.transform.SetParent(m_CharacterPoints[num], false);
            bodyObj.transform.localPosition = Vector3.zero;
            bodyObj.transform.localScale = Vector3.one;

            m_Characters[num] = bodyObj;
            m_Shadows[num].transform.position = bodyObj.transform.position + new Vector3(0, 0, 2);


            bodyObj.GetComponentInChildren<Animator>().Play("ready-1");

            body.SetMainWeapon(characterData.m_Weapons[m_CharWeaponNums[num]].m_BodySprite);
        }

        public void CreateTeam()
        {
            for (int i = 0; i < 3; i++)
            {
                CreatePlayer(i);
            }
        }

        public void ResetToSavedCharacters()
        {
            float[] scales = new float[3] { 1, .8f, .8f };
            for (int i = 0; i < 3; i++)
            {
                m_CharacterPoints[i].localPosition = m_DefaultPositions[i];
                m_CharacterPoints[i].localScale = scales[i] * Vector3.one;
            }

            for (int i = 0; i < m_Characters.Length; i++)
            {
                m_CharacterNums[i] = m_DataStorage.m_CharactersNumber[i];
                m_CharSkinNums[i] = m_Content.m_Characters[m_CharacterNums[i]].m_SkinNum;
                m_CharWeaponNums[i] = m_Content.m_Characters[m_CharacterNums[i]].m_WeaponNum;
            }
            CreateTeam();
        }
        // Update is called once per frame
        void Update()
        {

        }




    }
}