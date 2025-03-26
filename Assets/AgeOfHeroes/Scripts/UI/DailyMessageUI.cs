using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
namespace AgeOfHeroes.UI
{

    public class DailyMessageUI : MonoBehaviour
    {
        public Image m_MainImage;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Co_LoadImage());
        }

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator Co_LoadImage()
        {
            string imgURL = "http://shotorguvpalang.ir/Soccer2DMsgImgs/img.png";
            using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(imgURL))
            {
                yield return uwr.SendWebRequest();

                if (uwr.result != UnityWebRequest.Result.Success)
                {
                    // Debug.Log(uwr.error);
                }
                else
                {
                    // Get downloaded asset bundle
                    var texture = DownloadHandlerTexture.GetContent(uwr);
                    Sprite s = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                    m_MainImage.sprite = s;
                }
            }
        }
    }
}