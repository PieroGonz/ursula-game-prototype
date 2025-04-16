using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AgeOfHeroes
{
    public class Joystick_Stick : MonoBehaviour
    {
        public Image Back;
        public Image Stick;

        [HideInInspector]
        public bool Hold;
        [HideInInspector]
        public Vector3 HitPosition;
        [HideInInspector]
        public Vector3 StickDirection;

        public Vector3 m_OriginPosition;
        public bool m_PrevTouch = false;

        public RectTransform m_MainRect;

        // Reference to the quickbar
        private UltimateMobileQuickbar quickbar;

        // Add a BoxCollider2D for the initial interaction area
        private BoxCollider2D interactionArea;

        // Name of the quickbar (can be exposed in the inspector if needed)
        private string quickbarName = "Minimalist Mobile Quickbar";

        // Use this for initialization
        void Start()
        {
            Hold = false;

            // Initialize the main rectangle if not set
            if (m_MainRect == null)
                m_MainRect = transform.root.GetComponent<RectTransform>();

            // Find the quickbar by name
            GameObject quickbarObj = GameObject.Find(quickbarName);
            if (quickbarObj != null)
            {
                quickbar = quickbarObj.GetComponent<UltimateMobileQuickbar>();
            }

            // Add a BoxCollider2D for the initial interaction area
            interactionArea = gameObject.AddComponent<BoxCollider2D>();
            interactionArea.size = new Vector2(320f, 320f); // Size can be adjusted as needed
            interactionArea.isTrigger = true;
        }

        // Check if touch/click is within the interactionArea bounds
        private bool IsWithinInteractionArea(Vector3 position)
        {
            // Convert screen position to local position
            Vector3 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                GetComponent<RectTransform>(),
                position,
                null,
                out Vector2 localPoint);

            localPos = new Vector3(localPoint.x, localPoint.y, 0);

            // Check if the point is within the collider bounds
            return interactionArea.bounds.Contains(transform.TransformPoint(localPos));
        }

        // Update is called once per frame
        void Update()
        {
            Hold = false;
            Vector3[] PointerPos = new Vector3[2];

            if (Application.platform == RuntimePlatform.Android)
            {
                PointerPos = new Vector3[Input.touchCount];
                for (int i = 0; i < Input.touchCount; i++)
                {
                    PointerPos[i] = Input.touches[i].position;
                }
            }
            else
            {
                if (Input.GetMouseButton(0))
                {
                    PointerPos = new Vector3[1];
                    PointerPos[0] = Input.mousePosition;
                }
                else
                {
                    PointerPos = new Vector3[1];
                    PointerPos[0] = new Vector3(3000, -1000, 0);
                }
            }

            HitPosition = Vector3.zero;
            bool found = false;
            Vector3 foundPos = Vector3.zero;
            for (int i = 0; i < PointerPos.Length; i++)
            {
                if (PointerPos[i].x < 1.2f * Screen.width && PointerPos[i].y < 1.2f * Screen.height)
                {
                    // If this is the first touch, check if it's within the interaction area
                    if (!m_PrevTouch)
                    {
                        // Only allow initial interaction if within the defined area
                        if (IsWithinInteractionArea(PointerPos[i]))
                        {
                            // Disable quickbar when joystick is activated
                            if (quickbar != null)
                            {
                                //quickbar.DisableQuickbar();
                            }

                            m_PrevTouch = true;
                            m_OriginPosition = PointerPos[i];
                            Hold = true;
                            foundPos = PointerPos[i];
                            found = true;
                        }
                    }
                    else
                    {
                        // Already touching, continue tracking
                        Hold = true;
                        foundPos = PointerPos[i];
                        found = true;
                    }
                    break;
                }
            }

            if (!found)
            {
                // If we were previously touching, re-enable the quickbar
                if (m_PrevTouch && quickbar != null)
                {
                    //quickbar.EnableQuickbar();
                }

                m_PrevTouch = false;
                StickDirection = Vector3.zero;

                Back.rectTransform.anchoredPosition = new Vector2(420, 235);
                Stick.enabled = false;
            }
            else
            {
                //back
                Vector3 pos = m_OriginPosition;
                pos.z = 0;
                pos.x = pos.x / Screen.width;
                pos.y = pos.y / Screen.height;

                Vector2 p2 = Vector2.zero;
                p2.x = pos.x * m_MainRect.sizeDelta.x;
                p2.y = pos.y * m_MainRect.sizeDelta.y;
                Back.rectTransform.anchoredPosition = p2;

                //stick
                Stick.enabled = true;
                pos = foundPos;
                pos.z = 0;
                pos.x = pos.x / Screen.width;
                pos.y = pos.y / Screen.height;

                p2 = Vector2.zero;
                p2.x = pos.x * m_MainRect.sizeDelta.x;
                p2.y = pos.y * m_MainRect.sizeDelta.y;
                Stick.rectTransform.anchoredPosition = p2;

                StickDirection = foundPos - m_OriginPosition;
                StickDirection = StickDirection / Screen.height;
                StickDirection.Normalize();

                Vector3 dir = foundPos - m_OriginPosition;
                if (dir.magnitude > Screen.height / 5f)
                {
                    m_OriginPosition = Vector3.Lerp(m_OriginPosition, foundPos, 5 * Time.deltaTime);
                }
            }
        }
    }
}
