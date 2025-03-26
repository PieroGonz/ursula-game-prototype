using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfHeroes
{
    public class HitGlowControl : MonoBehaviour
    {
        [HideInInspector]
        public SpriteRenderer m_MainRenderer;
        [HideInInspector]
        public Material m_OriginalMat;
        public Material m_GlowMaterial;
        // Start is called before the first frame update
        void Start()
        {
            m_MainRenderer = GetComponent<SpriteRenderer>();
            m_OriginalMat = m_MainRenderer.material;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetOriginal()
        {
            if (m_MainRenderer != null)
                m_MainRenderer.material = m_OriginalMat;
        }

        public void SetGlow()
        {
            if (m_MainRenderer != null)
                m_MainRenderer.material = m_GlowMaterial;
        }
    }
}