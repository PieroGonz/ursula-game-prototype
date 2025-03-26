using AgeOfHeroes.ScriptableObjects;
using AgeOfHeroes.Gameplay;
using AgeOfHeroes.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Drawing;
namespace AgeOfHeroes
{
    public class ExplorationPawn : MonoBehaviour
    {
        [HideInInspector]
        public Rigidbody2D m_Body;
        public static ExplorationPawn m_Current;

        [HideInInspector]
        public bool m_CanMove;

        public float m_Speed;

        public int m_PawnNum = 0;

        public LevelPoint m_CurrentLevelPoint;
        public Chest m_CurrentChest;

        public bool m_ControledByPlayer = false;
        [HideInInspector]
        Vector2 m_MovementDir;

        [HideInInspector]
        public Vector2 m_LastMovementDir;

        public Animator m_Animator;

        public ExplorationPawn m_LeaderPawn;

        [SerializeField]
        private DataStorage m_DataStorage;
        private void Awake()
        {
            m_Body = GetComponent<Rigidbody2D>();
            //m_Current = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            m_CanMove = true;

            m_Animator.Play("run-blend-1");
            m_LastMovementDir = Vector2.right;
        }

        // Update is called once per frame
        void Update()
        {
            m_MovementDir = Vector2.zero;
            if (m_ControledByPlayer)
            {
                CheckLevelPoint();
                CheckChest();

                float vertical = Input.GetAxisRaw("Vertical");
                float horizontal = Input.GetAxisRaw("Horizontal");
                m_MovementDir = new Vector2(horizontal, vertical);

                if (Joystick.GeneralJoystick.LeftStick.StickDirection != Vector3.zero)
                {
                    m_MovementDir = Joystick.GeneralJoystick.LeftStick.StickDirection;
                }

            }
            else
            {

                Vector3 pos = m_LeaderPawn.transform.position - (Quaternion.Euler(0, 0, 30) * Helper.ToVector3(70 * m_LeaderPawn.m_LastMovementDir));
                if (m_PawnNum == 2)
                {
                    pos = m_LeaderPawn.transform.position - (Quaternion.Euler(0, 0, -30) * Helper.ToVector3(70 * m_LeaderPawn.m_LastMovementDir));
                }
                Vector3 dir = pos - transform.position;
                dir.z = 0;
                if (dir.magnitude > 30)
                {
                    m_MovementDir = dir;
                }
            }


            m_Animator.SetFloat("run-blend", m_Body.linearVelocity.magnitude / 20f);

            if (m_MovementDir.x != 0)
            {
                if (m_MovementDir.x > 0)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
            }

            if (m_MovementDir.magnitude > 0)
            {
                m_LastMovementDir = m_MovementDir;
            }

        }

        public void CheckChest()
        {
            if (m_CurrentChest == null)
            {
                bool found = false;
                Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 70);
                foreach (Collider2D hit in hits)
                {
                    if (hit.gameObject.tag == "Chest")
                    {
                        m_CurrentChest = hit.gameObject.GetComponent<Chest>();
                        ExplorationControl.m_Current.m_CurrentChest = m_CurrentChest;
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    if (!m_DataStorage.m_Chests[m_CurrentChest.m_ID])
                    {
                        ExplorationUI.m_Current.ShowChestPanel();
                    }
                    else
                    {
                        ExplorationUI.m_Current.ShowChestOpenedPanel();
                    }
                }
            }
            else
            {
                if (Helper.Distance2D(transform.position, m_CurrentChest.transform.position) > 100)
                {
                    ExplorationUI.m_Current.HideChestPanel();
                    m_CurrentChest = null;
                }
            }
        }

        public void CheckLevelPoint()
        {
            if (m_CurrentLevelPoint == null)
            {
                bool found = false;
                foreach (LevelPoint point in ExplorationControl.m_Current.m_LevelPoints)
                {
                    if (Helper.Distance2D(transform.position, point.transform.position) <= 140)
                    {
                        found = true;
                        m_CurrentLevelPoint = point;
                        break;
                    }
                }

                if (found)
                {
                    m_CurrentLevelPoint.m_CloseMark.gameObject.SetActive(true);
                    ExplorationControl.m_Current.m_LevelNum = m_CurrentLevelPoint.m_Level.m_LevelNum;
                    if (m_CurrentLevelPoint.m_Level.m_LevelNum <= m_DataStorage.m_LastLevelNum)
                    {
                        ExplorationUI.m_Current.ShowLevelUI();
                    }
                    else
                    {
                        ExplorationUI.m_Current.ShowLockedLevelUI();
                    }
                }
            }
            else
            {
                if (Helper.Distance2D(transform.position, m_CurrentLevelPoint.transform.position) > 140)
                {
                    m_CurrentLevelPoint.m_CloseMark.gameObject.SetActive(false);
                    ExplorationUI.m_Current.HideLevelUI();
                    m_CurrentLevelPoint = null;
                }
            }
        }
        private void FixedUpdate()
        {
            if (m_CanMove)
            {
                if (m_MovementDir != Vector2.zero)
                {
                    //if (horizontal < 0)
                    //{
                    //    transform.localScale = new Vector3(-6f, 6f, 1f);
                    //    //m_isFacingRight = false;
                    //}
                    //else if (horizontal > 0)
                    //{
                    //    transform.localScale = new Vector3(6f, 6f, 1f);
                    //    //m_isFacingRight = true;

                    //}

                    m_MovementDir.Normalize();
                    m_Body.linearVelocity += new Vector2(m_MovementDir.x, m_MovementDir.y) * m_Speed;

                    //m_Animator.SetBool("isWalking", true);
                    //m_Animator.SetBool("isRunning", false);
                }
            }

            m_Body.linearVelocity -= .1f * m_Body.linearVelocity;

        }

        public void SetAnimStand()
        {
            m_Animator.Play("char-test-stand");
        }
        public void SetAnimAttack()
        {
            m_Animator.Play("char-test-attack-1");
        }

        public void SetAnimRun()
        {
            m_Animator.Play("char-test-run");
        }
    }
}