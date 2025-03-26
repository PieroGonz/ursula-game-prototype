using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes
{
    public enum AnimationTypes
    {
        SlideInRight,
        SlideInLeft,
        SlideOut,
        Grow,
        Shrink,
        Spin
    }

    public class UIAnimation : MonoBehaviour
    {

        [HideInInspector]
        public float Timer = 0;
        Vector3 InitPosition;
        Vector3 InitScale;

        public float Speed = 1;
        public float Delay = 0;
        public AnimationCurve AnimCurve;

        public AnimationTypes AnimType;

        [HideInInspector]
        public bool Play = false;

        public bool AutoPlay = true;

        void Awake()
        {

        }
        // Use this for initialization
        void Start()
        {
            Timer = 0;
            if (AutoPlay)
            {
                switch (AnimType)
                {
                    case AnimationTypes.SlideInRight:
                        InitPosition = transform.localPosition;
                        transform.localPosition = InitPosition + new Vector3(400, 0, 0);
                        break;

                    case AnimationTypes.SlideInLeft:
                        InitPosition = transform.localPosition;
                        transform.localPosition = InitPosition + new Vector3(-400, 0, 0);
                        break;

                    case AnimationTypes.Grow:
                        InitScale = transform.localScale;
                        transform.localScale = Vector3.zero;
                        break;

                    case AnimationTypes.Spin:
                        transform.localRotation = Quaternion.Euler(0, 90, 0);
                        break;
                }
            }
            //switch (AnimType)
            //{
            //    case AnimationTypes.SlideIn:
            //        InitPosition = transform.localPosition;
            //        transform.localPosition = InitPosition + new Vector3(400, 0, 0);
            //        break;

            //    case AnimationTypes.Grow:
            //        transform.localScale = Vector3.zero;
            //        break;

            //    case AnimationTypes.Spin:
            //        transform.localRotation = Quaternion.Euler(0, 90, 0);
            //        break;
            //}

            if (AutoPlay)
            {
                Invoke("StartAnimation", Delay);
            }
        }

        public void StartAnimation()
        {
            Play = true;
            Timer = 0;

            switch (AnimType)
            {
                case AnimationTypes.SlideInRight:
                    transform.localPosition = InitPosition + new Vector3(400, 0, 0);
                    break;

                case AnimationTypes.SlideInLeft:
                    transform.localPosition = InitPosition + new Vector3(-400, 0, 0);
                    break;

                case AnimationTypes.Grow:
                    transform.localScale = Vector3.zero;
                    break;

                case AnimationTypes.Spin:
                    transform.localRotation = Quaternion.Euler(0, 90, 0);
                    break;
            }
        }

        // Update is called once per frame
        void Update()
        {
            //print("Update Anim");
            if (Play)
            {
                Timer += Speed * 0.04f;
                if (Timer >= 1)
                {
                    Timer = 1;
                    Play = false;
                }

                switch (AnimType)
                {
                    case AnimationTypes.SlideInRight:
                        transform.localPosition = InitPosition + new Vector3(400, 0, 0) - AnimCurve.Evaluate(Timer) * new Vector3(400, 0, 0);
                        break;

                    case AnimationTypes.SlideInLeft:
                        transform.localPosition = InitPosition + new Vector3(-400, 0, 0) - AnimCurve.Evaluate(Timer) * new Vector3(-400, 0, 0);
                        break;

                    case AnimationTypes.Grow:
                        transform.localScale = AnimCurve.Evaluate(Timer) * InitScale;
                        break;

                    case AnimationTypes.Spin:
                        transform.localRotation = Quaternion.Euler(0, 90 - AnimCurve.Evaluate(Timer) * 90, 0);
                        break;
                }
            }
            //transform.localRotation = Quaternion.Euler(0, 0, AnimCurve.Evaluate(1 - Timer) * 20);
        }
    }
}