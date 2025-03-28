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
                // Updated positioning logic for followers
                Vector3 pos = GetFollowerPosition();
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

        // New method to calculate follower positions based on pawn number
        // Update the GetFollowerPosition method in ExplorationPawn.cs
        private Vector3 GetFollowerPosition()
        {
            if (m_LeaderPawn == null)
                return transform.position;
                
            // Calculate base offset direction based on pawn number
            float angle = 0;
            float distance = 50f; // Base distance
            bool isFollowingMainLeader = m_LeaderPawn.m_ControledByPlayer;
            
            // Different formation for direct followers of the main leader vs followers of sub-leaders
            if (isFollowingMainLeader)
            {
                // Sub-leaders form a semi-circle behind the main leader
                // Use relative position within the sub-leader group
                int subLeaderIndex = GetSubLeaderIndex();
                int totalSubLeaders = CountTotalSubLeaders();
                
                if (totalSubLeaders <= 1)
                {
                    // Single sub-leader stays directly behind
                    angle = 180f;
                }
                else
                {
                    // Multiple sub-leaders spread out in an arc
                    float arcRange = 180f; // 180 degree arc behind the leader
                    float step = arcRange / (totalSubLeaders - 1);
                    angle = 180f - (arcRange / 2f) + (subLeaderIndex * step);
                }
                
                // Sub-leaders maintain more distance to allow room for their followers
                distance = 80f;
            }
            else
            {
                // Regular followers form a tighter formation behind their sub-leader
                // Find relative position within this sub-leader's followers
                int followerIndex = GetFollowerIndex();
                
                // Followers alternate sides (left/right)
                bool isOnRightSide = (followerIndex % 2 == 0);
                int rowIndex = followerIndex / 2; // Two followers per row
                
                // Set angle based on side
                angle = isOnRightSide ? 150f : 210f; // Right side: 150°, Left side: 210°
                
                // Add small angle variation based on row
                angle += (isOnRightSide ? -1 : 1) * (rowIndex * 15f);
                
                // Followers in back rows stay further back
                distance = 50f + (rowIndex * 15f);
            }
            
            // Get leader's velocity and speed
            Vector2 leaderVelocity = m_LeaderPawn.m_Body.linearVelocity;
            float leaderSpeed = leaderVelocity.magnitude;
            
            // Calculate prediction factor - higher for fast movements
            float predictionFactor = Mathf.Clamp01(leaderSpeed / 40f);
            
            // Calculate offset from leader that creates the formation
            // Use leader's facing direction (based on scale) rather than movement direction for more stable formations
            Vector2 leaderFacing = m_LeaderPawn.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
            Vector2 formationOffset = (Quaternion.Euler(0, 0, angle) * leaderFacing) * distance;
            
            // Calculate prediction offset - followers try to move to where the leader will be
            Vector2 predictionOffset = leaderVelocity.normalized * predictionFactor * distance * 0.5f;
            
            // Final target position combines leader position, formation offset, and prediction
            Vector3 targetPosition = m_LeaderPawn.transform.position + (Vector3)formationOffset + (Vector3)predictionOffset;
            
            return targetPosition;
        }

        // Helper method to get this pawn's sub-leader index
        private int GetSubLeaderIndex()
        {
            // Get reference to the PawnManager
            PawnManager pawnManager = FindObjectOfType<PawnManager>();
            if (pawnManager == null)
                return 0;
                
            // Check if this pawn is in the sub-leaders list
            List<ExplorationPawn> subLeaders = pawnManager.GetSubLeaders();
            for (int i = 0; i < subLeaders.Count; i++)
            {
                if (subLeaders[i] == this)
                    return i;
            }
            return 0;
        }

        // Helper method to get this pawn's follower index within its leader's followers
        private int GetFollowerIndex()
        {
            if (m_LeaderPawn == null)
                return 0;
                
            // Get reference to the PawnManager
            PawnManager pawnManager = FindObjectOfType<PawnManager>();
            if (pawnManager == null)
                return 0;
                
            // Count how many other followers this leader has before this pawn
            int followerIndex = 0;
            foreach (GameObject pawnObj in pawnManager.GetAllPawns())
            {
                if (pawnObj == null)
                    continue;
                    
                ExplorationPawn pawn = pawnObj.GetComponent<ExplorationPawn>();
                if (pawn != null && pawn != this && pawn.m_LeaderPawn == m_LeaderPawn)
                {
                    if (pawn.m_PawnNum < m_PawnNum)
                        followerIndex++;
                }
            }
            return followerIndex;
        }

        // Helper method to count total sub-leaders
        private int CountTotalSubLeaders()
        {
            PawnManager pawnManager = FindObjectOfType<PawnManager>();
            if (pawnManager == null)
                return 0;
                
            return pawnManager.GetSubLeaders().Count;
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
                    if (Helper.Distance2D(transform.position, point.transform.position) <= 50)
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
                if (Helper.Distance2D(transform.position, m_CurrentLevelPoint.transform.position) > 50)
                {
                    m_CurrentLevelPoint.m_CloseMark.gameObject.SetActive(false);
                    ExplorationUI.m_Current.HideLevelUI();
                    m_CurrentLevelPoint = null;
                }
            }
        }
        private void FixedUpdate()
        {
            if (!m_CanMove)
                return;
                
            // Only process movement if we have a direction
            if (m_MovementDir.magnitude > 0.01f)
            {
                // Normalize the movement direction
                Vector2 normalizedDir = m_MovementDir.normalized;
                
                float speedMultiplier = 1f;
                    
                // For followers (not player-controlled)
                if (!m_ControledByPlayer && m_LeaderPawn != null)
                {
                    // Calculate distance to leader
                    float distToLeader = Vector3.Distance(transform.position, m_LeaderPawn.transform.position);
                        
                    // Apply speed boost when far from leader
                    if (distToLeader > 100f)
                        speedMultiplier = 2.5f; // Greater speed boost when very far
                    else if (distToLeader > 50f)
                        speedMultiplier = 1.8f; // Higher boost when moderately far
                        
                    // Special handling for sub-leaders - more direct movement
                    if (m_LeaderPawn.m_ControledByPlayer)
                    {
                        // Sub-leaders move more directly toward their position
                        speedMultiplier *= 1.3f;
                    }
                }
                    
                // Apply movement force
                m_Body.linearVelocity += normalizedDir * m_Speed * speedMultiplier * Time.fixedDeltaTime;
            }
                
            // Apply dampening to slow down naturally - increased for better responsiveness
            m_Body.linearVelocity -= 0.05f * m_Body.linearVelocity;
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