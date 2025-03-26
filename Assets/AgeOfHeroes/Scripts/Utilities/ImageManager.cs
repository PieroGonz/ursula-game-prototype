using System.IO;
using System;
using UnityEngine;
using AgeOfHeroes.ScriptableObjects;
using UnityEngine.UI;
namespace AgeOfHeroes
{
    public class ImageManager : MonoBehaviour
    {
        [SerializeField, Space]
        private PlayerData m_PlayerData;
        [SerializeField, Space]
        private Contents m_Contents;
        public static ImageManager m_Current;
        private string m_SourceImagePath;

        void Awake()
        {
            m_Current = this;
        }
        private void Start()
        {
            //m_SourceImagePath = Application.dataPath + "/Art/GeneratedAvatars/GeneratedAvatar_1.png";

            //string destinationPath = Path.Combine(Application.persistentDataPath, "Images/GeneratedAvatar_1.png");

            //// ایجاد پوشه مقصد در صورت وجود نداشتن
            //Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));

            //// کپی تصویر از مسیر منبع به مسیر مقصد
            //File.Copy(m_SourceImagePath, destinationPath, true);

            //Debug.Log("Image is in the destination " + destinationPath);

            //byte[] imageData = File.ReadAllBytes(m_SourceImagePath);
            //ByteArray ba = new ByteArray();
            //ba.bytes = imageData;
            //m_PlayerData.m_PlayerAvatarBytes = JsonUtility.ToJson(ba);
            //byte[] bytes = m_PlayerData.m_DefaultAvatarImage.EncodeToPNG();
            //DataBase_PlayerAvatar ba = new DataBase_PlayerAvatar();
            //ba.m_PlayerAvatarBytes = bytes;
            //m_PlayerData.m_DefaultAvatarBytes = JsonUtility.ToJson(ba);



        }


        //public Sprite GetAvatarImageFromSource()
        //{
        //    string sourceImagePath = Application.dataPath + "/Art/GeneratedAvatars/GeneratedAvatar_1.png";

        //    // خواندن تصویر از مسیر منبع
        //    byte[] imageData = File.ReadAllBytes(m_SourceImagePath);

        //    // ایجاد تصویر از داده‌های تصویر
        //    Texture2D texture = new Texture2D(512, 512);
        //    texture.LoadImage(imageData);
        //    //imageData = ImageConversion.EncodeToPNG(texture);

        //    // ساخت sprite جدید با استفاده از تصویر
        //    Sprite avatarSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

        //    Debug.Log("Image is set as the ProfileImage sprite.");


        //    return avatarSprite;
        //}

        public byte[] GetBytesFromTexture(RenderTexture renderTexture)
        {
            Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA64, false);
            RenderTexture.active = renderTexture;
            tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            tex.Apply();
            RenderTexture.active = null;

            // تبدیل Texture2D به داده‌های PNG
            byte[] bytes = tex.EncodeToPNG();
            return bytes;
        }

        public Sprite GetSpriteFromRenderTexture(RenderTexture renderTexture)
        {
            Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA64, false);
            RenderTexture.active = renderTexture;
            tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            tex.Apply();
            RenderTexture.active = null;
            Sprite avatarSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f, 10);

            return avatarSprite;
        }

        public Sprite GetSpriteFromBytes(byte[] imageData)
        {
            Sprite avatarSprite = null;
            try
            {
                Texture2D texture = new Texture2D(512, 512);
                texture.LoadImage(imageData);
                avatarSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f, 10);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {

            }
            return avatarSprite;
        }
    }
}