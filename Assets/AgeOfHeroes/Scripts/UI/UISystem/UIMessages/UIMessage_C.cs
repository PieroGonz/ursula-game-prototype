using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace AgeOfHeroes
{
    public enum MessageButtons_C
    {
        none,
        exit,
        ok,
        yes,
        no,
        cancel
    }
    public class UIMessage_C : MonoBehaviour
    {
        public Func<bool> f_Clicked_Yes;
        public Func<bool> f_Clicked_No;
        public Func<bool> f_Clicked_OK;
        public Func<bool> f_Clicked_Cancel;
        public Func<bool> f_Clicked_Exit;
        public Func<bool> f_Clicked_WatchVideoToUnlock;

        public Text m_MessageText;
        public Image m_MessageImage;

        [HideInInspector]
        public int m_MessageType = 0;

        public Image[] m_ButtonPanels;
        // Start is called before the first frame update
        void Start()
        {
            switch (m_MessageType)
            {
                case 0:
                    m_ButtonPanels[0].gameObject.SetActive(true);
                    m_ButtonPanels[1].gameObject.SetActive(false);
                    m_ButtonPanels[2].gameObject.SetActive(false);
                    m_ButtonPanels[3].gameObject.SetActive(false);
                    break;
                case 1:
                    m_ButtonPanels[0].gameObject.SetActive(false);
                    m_ButtonPanels[1].gameObject.SetActive(true);
                    m_ButtonPanels[2].gameObject.SetActive(false);
                    m_ButtonPanels[3].gameObject.SetActive(false);
                    break;
                case 2:
                    m_ButtonPanels[0].gameObject.SetActive(false);
                    m_ButtonPanels[1].gameObject.SetActive(false);
                    m_ButtonPanels[2].gameObject.SetActive(true);
                    m_ButtonPanels[3].gameObject.SetActive(false);
                    break;
                case 3:
                    m_ButtonPanels[0].gameObject.SetActive(false);
                    m_ButtonPanels[1].gameObject.SetActive(false);
                    m_ButtonPanels[2].gameObject.SetActive(false);
                    m_ButtonPanels[3].gameObject.SetActive(true);
                    break;
            }

            if (m_MessageImage.sprite == null)
            {
                m_MessageImage.gameObject.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void BtnClick(string btnType)
        {
            switch (btnType)
            {
                case "":
                    break;
                case "none":
                    break;
                case "exit":
                    if (f_Clicked_Exit != null)
                        f_Clicked_Exit();
                    CloseMessage();
                    //close this message
                    break;
                case "ok":
                    f_Clicked_OK();
                    CloseMessage();
                    break;
                case "yes":
                    f_Clicked_Yes();
                    CloseMessage();
                    //Debug.Log("CloseMessaeee");
                    break;
                case "watchvideo":
                    f_Clicked_WatchVideoToUnlock();
                    CloseMessage();
                    break;
                case "no":
                    f_Clicked_No();
                    CloseMessage();
                    break;
                case "cancel":
                    f_Clicked_Cancel();
                    CloseMessage();
                    break;
            }
        }

        public void CloseMessage()
        {
            UISystem.RemoveUI(gameObject);
        }
    }
}