using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
namespace AgeOfHeroes
{
    //[ExecuteInEditMode()]
    public class Joystick : MonoBehaviour
    {



        [HideInInspector]
        public Vector2 StickOffset;

        public Joystick_Stick LeftStick;
        public Joystick_Stick RightStick;
        public JoystickButton ButtonA;
        public JoystickButton ButtonB;
        public JoystickButton ButtonC;
        public JoystickButton ButtonD;
        public JoystickButton ButtonE;

        public GameObject ConversationButton;


        public Image m_TechniqueFill;
        public Image m_TechniqueLight;
        //public JoystickButton LeftSteer;
        //public JoystickButton RightSteer;
        public GraphicRaycaster Canvas;
        public static Joystick GeneralJoystick;

        [HideInInspector]
        public List<RaycastResult> RayCastResults;
        [HideInInspector]
        public List<GameObject> RayCastObjects;

        public Text[] JoyButtonTexts;
        public string[] JoyButtonStrings;
        void Awake()
        {
            GeneralJoystick = this;
            ConversationButton.SetActive(false);
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnGUI()
        {

        }
    }
}