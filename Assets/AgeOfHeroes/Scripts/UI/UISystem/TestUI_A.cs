using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes
{
    public class TestUI_A : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void BtnShowMSG1()
        {
            UIMessage_A msg = UISystem.ShowMessage("UIMessage_B", 0, "exit?", null);
            msg.f_Clicked_Yes = DoSomthing;
            msg.f_Clicked_No = DoSomthing;
        }

        public void Btn_No()
        {
            UISystem.RemoveUI(gameObject);
        }

        public bool DoSomthing()
        {
            //---desired code
            //print("Test Func");
            //---
            return true;
        }
    }
}