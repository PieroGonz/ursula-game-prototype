using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgeOfHeroes.ScriptableObjects;
using System.Security.Cryptography;

namespace AgeOfHeroes.Gameplay
{
    public class FightCamera : MonoBehaviour
    {
        public static FightCamera m_Current;
        [HideInInspector]
        public Vector3 m_InitPosition;
        [HideInInspector]
        public Vector3 m_TargetPosition;
        [HideInInspector]
        public float m_MoveLerpSpeed = 5;
        [HideInInspector]
        public float m_TargetSize = 400;

        [HideInInspector]
        public float m_MaxSize = 300;

        [HideInInspector]
        public float m_DefaultSize = 300;

        private float m_ShakeTimer;
        private float m_ShakeArc;
        private float m_ShakeRadius = 1;

        //public Vector3 m_SmallShakeDir;
        public AnimationCurve m_ShakeCurve1;


        [HideInInspector]
        public bool m_GameOverCamera = false;


        [HideInInspector]
        public float m_CamHalfW = 0;

        public Vector2 m_CameraBoundSize = new Vector2(100, 100);

        [HideInInspector]
        public bool m_ChaseBall = true;

        [SerializeField, Space]
        private Contents m_Contents;
        // Start is called before the first frame update
        void Awake()
        {
            m_Current = this;
        }

        void Start()
        {
            m_InitPosition = transform.position;
            m_TargetPosition = m_InitPosition;

            Vector3 pos = ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
            m_CamHalfW = pos.x - transform.position.x;


            //float ratio = (float)Screen.height / (float)Screen.width;
            //float deltaX = Field.m_Main.m_MaxCamBound.x - Field.m_Main.m_MinCamBound.x;

            //m_MaxSize = (ratio * deltaX)/2f;

            m_DefaultSize = 150;
            m_MaxSize = 200;
            //GetComponent<Camera>().orthographicSize = m_DefaultSize;
        }

        void Update()
        {
            //right edge
            //m_CameraRightEdge = ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
            //m_CameraRightEdge.y = 0;

            //
            m_ShakeTimer -= Time.deltaTime;
            //ShakeArc += 100 * Time.deltaTime;

            if (m_ShakeTimer <= 0)
                m_ShakeTimer = 0;

            Vector3 ShakeOffset = Vector3.zero;
            float shakeSin = Mathf.Cos(30 * Time.time) * Mathf.Clamp(m_ShakeTimer, 0, 0.5f);
            float shakeCos = Mathf.Sin(50 * Time.time) * Mathf.Clamp(m_ShakeTimer, 0, 0.5f);
            ShakeOffset = new Vector3(m_ShakeRadius * shakeCos, m_ShakeRadius * shakeSin, 0);

            //if (m_ChaseBall)
            //{
            //    Vector3 target = Ball.m_Main.transform.position;
            //    target.z = -600;
            //    //target.x = 0.8f * target.x;
            //    //target.y = 0.9f * target.y;
            //    transform.position = Vector3.Lerp(transform.position, target, 3 * Time.deltaTime);
            //}

            // Vector3 pos = transform.position;
            // pos.x = Mathf.Clamp(pos.x, -160, 160);
            // pos.y = Mathf.Clamp(pos.y, -120, 240);
            // transform.position = pos;
            //transform.position = Vector3.Lerp(transform.position, m_TargetPosition, m_MoveLerpSpeed * Time.deltaTime);
            //transform.position += ShakeOffset;

            //GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, m_TargetSize, 5 * Time.deltaTime);

            //clamp camera
            //float ySize = GetComponent<Camera>().orthographicSize;
            //float ratio = (float)Screen.width / (float)Screen.height;
            //float xSize = ratio * ySize;

            //Vector2 min = Field.m_Main.m_MinCamBound;
            //Vector2 max = Field.m_Main.m_MaxCamBound;

            //Vector3 pos = transform.position;
            //pos.x = Mathf.Clamp(pos.x, min.x + xSize, max.x - xSize);
            //pos.y = Mathf.Clamp(pos.y, min.y + ySize, max.y - ySize);
            //transform.position = pos;
        }

        public void StartShake(float t, float r)
        {
            if (m_ShakeTimer == 0 || m_ShakeRadius < r)
                m_ShakeRadius = r;

            m_ShakeTimer = t;
        }

        public Vector3 WorldToScreenPoint(Vector3 WorldPos)
        {
            Vector3 pos = GetComponent<Camera>().WorldToScreenPoint(WorldPos);
            pos.x = pos.x / Screen.width;
            pos.y = pos.y / Screen.height;

            return pos;
        }

        public Vector3 ScreenToWorldPoint(Vector3 ScreenPos)
        {
            Ray ray = GetComponent<Camera>().ScreenPointToRay(ScreenPos);
            Vector3 point = ray.origin;
            point.z = 0;

            return point;
        }

        public void MoveCamera(Vector3 targetPosition, float time)
        {
            StartCoroutine(Co_MoveCamera(targetPosition, time));
        }

        IEnumerator Co_MoveCamera(Vector3 end, float time)
        {
            Vector3 start = transform.localPosition;
            float lerp = 0;
            float speed = 1f / time;
            end.z = -200;
            AnimationCurve curve = m_Contents.m_CamLerp;
            while (true)
            {
                transform.localPosition = Vector3.Lerp(start, end, curve.Evaluate(lerp));
                lerp += speed * Time.deltaTime;
                if (lerp >= 1)
                    break;
                yield return null;
            }
            transform.localPosition = end;
        }

        public void ZoomCamera(float targetSize, float time)
        {
            StartCoroutine(Co_ZoomCamera(targetSize, time));
        }

        IEnumerator Co_ZoomCamera(float targetSize, float time)
        {
            Camera cam = GetComponent<Camera>();
            float start = cam.orthographicSize;
            float lerp = 0;
            float speed = 1f / time;
            AnimationCurve curve = m_Contents.m_CamLerp;

            targetSize = Mathf.Clamp(targetSize, 100, m_MaxSize);

            while (true)
            {
                cam.orthographicSize = Mathf.Lerp(start, targetSize, curve.Evaluate(lerp));
                lerp += speed * Time.deltaTime;
                if (lerp >= 1)
                    break;
                yield return null;
            }
            cam.orthographicSize = targetSize;
        }

        public void SmallShake()
        {
            StartCoroutine(Co_SmallShake());
        }

        IEnumerator Co_SmallShake()
        {
            float angle = Random.Range(0, 360);
            Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.right;
            float lerp = 0;
            Vector3 startPos = transform.position;

            while (true)
            {
                transform.position = startPos + 20 * (m_ShakeCurve1.Evaluate(lerp)) * dir;
                lerp += 5 * Time.deltaTime;
                if (lerp >= 1)
                    break;
                yield return null;
            }

            transform.position = startPos;
        }

        private void OnDrawGizmos()
        {

            Gizmos.DrawWireCube(Vector3.zero, new Vector3(m_CameraBoundSize.x, m_CameraBoundSize.y, 0));
        }
    }

}