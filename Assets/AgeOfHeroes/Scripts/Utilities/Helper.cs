using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes
{
    public class Helper : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public static Vector2 ToVector2(Vector3 v)
        {
            return (new Vector2(v.x, v.y));
        }

        public static Vector3 ToVector3(Vector2 v)
        {
            return (new Vector3(v.x, v.y, 0));
        }

        public static float Distance2D(Vector3 v1, Vector3 v2)
        {
            v1.z = 0;
            v2.z = 0;
            return (Vector3.Distance(v1, v2));

        }

        public static int BoolToInt(bool a)
        {
            if (a)
                return 1;
            else
                return 0;
        }

        public static GameObject FindObject(GameObject parentObj, string objName)
        {
            Transform[] all = parentObj.GetComponentsInChildren<Transform>(true);
            foreach (Transform t in all)
            {
                if (t.gameObject.name == objName)
                {
                    return t.gameObject;
                }
            }

            return null;
        }

        public static Vector2 WorldToUIPosition(Vector3 worldPos, Camera worldCamera, Vector2 baseSize)
        {
            Vector3 pos = worldCamera.WorldToScreenPoint(worldPos);
            pos.x = pos.x / Screen.width;
            pos.y = pos.y / Screen.height;
            Vector2 p2 = Vector2.zero;
            p2.x = pos.x * baseSize.x;
            p2.y = pos.y * baseSize.y;
            return p2;

        }

    }
}