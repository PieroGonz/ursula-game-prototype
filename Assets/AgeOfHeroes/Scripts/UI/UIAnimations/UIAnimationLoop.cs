using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace AgeOfHeroes
{
    public enum UIAnimationLoopTypes
    {
        Scale,
        Alpha,
        Shake,
        Rotate,
        Spin
    }
    public class UIAnimationLoop : MonoBehaviour
    {
        float timer = 0;
        Vector3 InitScale;
        Color InitColor;
        Quaternion InitRotation;
        UIAnimation StartAnim;

        public float Speed = 1;
        public float Radius = .1f;

        public UIAnimationLoopTypes AnimType = UIAnimationLoopTypes.Scale;
        [HideInInspector]
        public Image UIImage;
        [HideInInspector]
        public Text UIText;

        [HideInInspector]
        public bool AnimEnable = true;
        // Use this for initialization
        void Start()
        {
            StartAnim = GetComponent<UIAnimation>();
            InitScale = transform.localScale;
            InitRotation = transform.localRotation;
            UIImage = GetComponent<Image>();
            UIText = GetComponent<Text>();

            if (UIImage != null)
            {
                InitColor = UIImage.color;
            }
            else if (UIText != null)
            {
                InitColor = UIText.color;
            }


        }

        // Update is called once per frame
        void Update()
        {
            if (StartAnim == null || StartAnim.Timer == 1)
            {
                if (AnimEnable)
                {
                    timer += 2 * Speed * Time.deltaTime;

                    if (AnimType == UIAnimationLoopTypes.Scale)
                    {
                        transform.localScale = (1 + Radius * Mathf.Sin(timer)) * InitScale;
                    }
                    else if (AnimType == UIAnimationLoopTypes.Alpha)
                    {
                        //print("Color");
                        float a = InitColor.a + Radius * (Mathf.Sin(timer) - 1);
                        Color c = InitColor;
                        c.a = a;
                        UIImage.color = c;
                    }
                    else if (AnimType == UIAnimationLoopTypes.Rotate)
                    {
                        transform.localRotation = InitRotation * Quaternion.Euler(0, 0, Radius * Mathf.Sin(timer));
                    }
                }
            }
        }

        public void ResetUIAnim()
        {
            transform.localScale = InitScale;
            transform.localRotation = InitRotation;

            if (UIImage != null)
            {
                UIImage.color = InitColor;
            }
        }
    }
}