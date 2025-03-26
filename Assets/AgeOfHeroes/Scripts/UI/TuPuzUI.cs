using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgeOfHeroes.ScriptableObjects;

namespace AgeOfHeroes.UI
{
    public class TuPuzUI : MonoBehaviour
    {
        [TextAreaAttribute]
        public string[] str_MessageUI_TuPuz;
        public Sprite[] spr_MessageUI_TuPuz;

        [SerializeField, Space]
        private Contents m_Contents;
        [SerializeField, Space]
        private DataStorage m_DataStorage;
        public static TuPuzUI m_Current;
        void Awake()
        {
            m_Current = this;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void ShowTuPuzUI()
        {
            int selectmessage;

            do
            {
                selectmessage = Random.Range(0, 24);
                //Debug.Log(selectmessage + " :FirstRandom number");

                switch (selectmessage)
                {
                    case 0:
                        if (CheckKitUnlocked("Esteghlal"))
                            break;
                        else
                            continue;
                    case 1:
                        if (CheckKitUnlocked("Perspolis"))
                            break;
                        else
                            continue;
                    case 2:
                        if (!m_DataStorage.m_CustomizeProfilePic)
                            break;
                        else
                            continue;
                    //case 3:
                    //    if (!m_Contents.m_Balls[5].m_Unlocked)
                    //        break;
                    //    else
                    //        continue;
                    case 4:
                    // if (!m_Contents.m_Stadiums[3].m_Unlocked)
                    //     break;
                    // else
                    //     continue;
                    case 5:
                        if (!m_DataStorage.m_RemoveAds)
                            break;
                        else
                            continue;
                    case 6:
                    // if (!m_Contents.m_Stadiums[4].m_Unlocked)
                    //     break;
                    // else
                    //     continue;
                    case 7:
                        if (CheckKitUnlocked("Tractor"))
                            break;
                        else
                            continue;
                    case 8:
                        if (CheckKitUnlocked("Sepahan"))
                            break;
                        else
                            continue;
                    case 9:
                        if (CheckKitUnlocked("France"))
                            break;
                        else
                            continue;
                    case 10:
                        if (CheckKitUnlocked("Liverpool"))
                            break;
                        else
                            continue;
                    case 11:
                    // if (!m_Contents.m_Weathers[3].m_Unlocked)
                    //     break;
                    // else
                    //     continue;
                    case 16:
                        if (CheckKitUnlocked("Malavan"))
                            break;
                        else
                            continue;
                    case 17:
                        if (CheckKitUnlocked("Fulad"))
                            break;
                        else
                            continue;
                    case 18:
                        if (CheckKitUnlocked("ManUtd"))
                            break;
                        else
                            continue;
                    case 19:
                        if (CheckKitUnlocked("Arsenal"))
                            break;
                        else
                            continue;
                    case 20:
                        if (CheckKitUnlocked("Chelsea"))
                            break;
                        else
                            continue;
                    case 21:
                        if (CheckKitUnlocked("Tottenham"))
                            break;
                        else
                            continue;
                    case 22:
                    // if (!m_Contents.m_Stadiums[5].m_Unlocked)
                    //     break;
                    // else
                    //     continue;
                    case 23:
                    // if (!m_Contents.m_Stadiums[6].m_Unlocked)
                    //     break;
                    // else
                    //     continue;
                    default:
                        break;
                }

                // If selectmessage is not handled by any case, it means it's unlocked
                //Debug.Log(selectmessage + " :Random number");
                ShowMessage(selectmessage);
                return;
            } while (true);
        }

        private bool CheckKitUnlocked(string teamName)
        {
            //foreach (var kit in m_Contents.m_Kits)
            //{
            //    if (kit.m_TeamName == teamName && !kit.m_Unlocked) // Show message if kit is unlocked (m_Unlocked = false)
            //        return true;
            //}
            return false; // Don't show message if kit is locked (m_Unlocked = true)
        }


        public void ShowMessage(int selectmessage)
        {
            //Debug.Log(selectmessage + " :Random number");
            UIMessage_A msg = UISystem.ShowMessage("UIMessage_A", 0, str_MessageUI_TuPuz[selectmessage], spr_MessageUI_TuPuz[selectmessage]);
            msg.f_Clicked_Yes = () => Btn_TuPuz_Yes(selectmessage);
            msg.f_Clicked_No = Btn_TuPuz_No;
        }

        public bool Btn_TuPuz_Yes(int num)
        {
            if (num == 12)
            {
                int randommenu = Random.Range(0, 2);
                GameObject obj = UISystem.ShowUI("CoinShopUI");
                obj.GetComponentInChildren<UITabControl>().SelectTab(randommenu);
                UISystem.RemoveUI("MainMenuUI");
            }
            else if (num >= 13 && num <= 15)
            {
                GameObject obj1 = UISystem.ShowUI("CoinShopUI");
                obj1.GetComponentInChildren<UITabControl>().SelectTab(3);
                UISystem.RemoveUI("MainMenuUI");
            }
            else
            {
                //num == 16 || num == 17 ||
                GameObject obj1 = UISystem.ShowUI("CoinShopUI");
                obj1.GetComponentInChildren<UITabControl>().SelectTab(2);
                UISystem.RemoveUI("MainMenuUI");
            }

            return true;
        }
        public bool Btn_TuPuz_No()
        {
            return true;
        }
    }
}
