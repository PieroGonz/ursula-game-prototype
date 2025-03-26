using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgeOfHeroes.ScriptableObjects;
namespace AgeOfHeroes
{
    public class AvatarSystem : MonoBehaviour
    {
        public SpriteRenderer[] m_PartsSprites;

        public static AvatarSystem m_Main;
        [SerializeField, Space]
        private Contents m_Contents;
        [SerializeField, Space]
        private PlayerData m_PlayerData;

        public RenderTexture renderTexture;

        [HideInInspector]
        public Sprite m_GeneratedAvatar;
        void Awake()
        {
            m_Main = this;
        }
        // Start is called before the first frame update
        void Start()
        {

            for (int i = 0; i < 10; i++)
            {
                int part = m_PlayerData.m_PlayerAvatarParts[i];
                m_PartsSprites[i].sprite = m_Contents.m_AvatarPartList[i].m_Parts[part].m_PartSprite;
            }

            StartCoroutine(Co_InitAvatarUpdate());
        }
        public void UpdateAvatar()
        {
            StartCoroutine(Co_InitAvatarUpdate());
        }

        IEnumerator Co_InitAvatarUpdate()
        {
            if (m_PlayerData.m_PlayerImageNum == -1)
            {
                yield return new WaitForSeconds(.2f);
                m_GeneratedAvatar = GenerateAvatar();
                yield return new WaitForSeconds(.2f);
                m_PlayerData.m_PlayerAvatarSprite = m_GeneratedAvatar;
            }
            else
            {
                m_PlayerData.m_PlayerAvatarSprite = m_Contents.m_PlayerAvatars[m_PlayerData.m_PlayerImageNum];
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetSprite(int part, int num)
        {
            m_PartsSprites[part].sprite = m_Contents.m_AvatarPartList[part].m_Parts[num].m_PartSprite;
            m_PlayerData.m_PlayerAvatarParts[part] = num;
        }

        public byte[] GenerateBytes()
        {
            byte[] bytes = ImageManager.m_Current.GetBytesFromTexture(renderTexture);
            return bytes;
        }
        public Sprite GenerateAvatar()
        {
            return ImageManager.m_Current.GetSpriteFromRenderTexture(renderTexture);
        }
        public void GenerateAvatar(int[] parts)
        {
            StartCoroutine(Co_GenerateAvatar(parts));
        }

        IEnumerator Co_GenerateAvatar(int[] parts)
        {
            //Debug.Log(parts.ToString());
            if (parts == null || parts.Length != m_Contents.m_AvatarPartList.Length)
            {
                //Debug.Log("opponent avatar");
                parts = new int[10];
            }

            for (int i = 0; i < parts.Length; i++)
            {
                m_PartsSprites[i].sprite = m_Contents.m_AvatarPartList[i].m_Parts[parts[i]].m_PartSprite;
            }
            yield return new WaitForSeconds(.1f);
            //yield return null;
            m_GeneratedAvatar = ImageManager.m_Current.GetSpriteFromRenderTexture(renderTexture);
        }
    }
}