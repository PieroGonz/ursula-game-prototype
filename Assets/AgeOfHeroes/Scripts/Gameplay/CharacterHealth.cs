using AgeOfHeroes.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace AgeOfHeroes.Gameplay
{
    public class CharacterHealth : MonoBehaviour
    {

        public float m_Health = 100;
        [HideInInspector]
        public float m_TempHealth = 100;
        public float m_MaxHealth = 100;
        public Image m_HealthBar;
        public Text m_HealthText;
        public Pawn m_OwnerPawn;

        public bool m_UseCharacterData = true;

        [HideInInspector]
        public bool m_DoneDamaging = true;

        [HideInInspector]
        public bool m_IsDead = false;

        public Coroutine m_DamageCoroutine;
        // Start is called before the first frame update
        void Start()
        {
            if (m_UseCharacterData)
            {
                m_Health = m_OwnerPawn.m_CharacterData.m_Health + m_OwnerPawn.m_CharacterData.m_Skins[m_OwnerPawn.m_CharacterData.m_SkinNum].m_AddedHealth + m_OwnerPawn.m_CharacterData.m_ItemLevel * 5;
                m_MaxHealth = m_Health;
            }
            else
            {
                m_Health = m_MaxHealth;
            }
            m_TempHealth = m_Health;
            m_DoneDamaging = true;
        }

        // Update is called once per frame
        void Update()
        {
            m_HealthBar.fillAmount = m_TempHealth / m_MaxHealth;
            m_HealthText.text = Mathf.FloorToInt(m_TempHealth).ToString();
        }

        public void TakeDamage(float damage)
        {
            if (!m_IsDead)
            {
                m_DoneDamaging = false;
                m_Health -= damage;
                if (m_Health <= 0)
                {
                    m_Health = 0;
                    m_IsDead = true;
                    //GameControl.m_Current.HandleDeath(m_OwnerPawn);
                }

                if (m_DamageCoroutine != null)
                {
                    StopCoroutine(m_DamageCoroutine);
                }
                m_DamageCoroutine = StartCoroutine(UpdateHealth());
            }
        }

        IEnumerator UpdateHealth()
        {

            float lerp = 0;
            float currentHealth = m_TempHealth;
            float updatedHealth = m_Health;
            while (lerp < 1)
            {
                m_TempHealth = Mathf.Lerp(currentHealth, updatedHealth, lerp);
                lerp += 2 * Time.deltaTime;
                yield return null;
            }

            m_TempHealth = updatedHealth;
            m_DoneDamaging = true;
        }

        public void AddHealth(float count)
        {
            m_Health += count;
            m_Health = Mathf.Clamp(m_Health, 0, m_MaxHealth);
            if (m_DamageCoroutine != null)
            {
                StopCoroutine(m_DamageCoroutine);
            }
            m_DamageCoroutine = StartCoroutine(UpdateHealth());
        }
    }
}
