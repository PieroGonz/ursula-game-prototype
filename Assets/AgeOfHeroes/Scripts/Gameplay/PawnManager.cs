using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using AgeOfHeroes.ScriptableObjects;

namespace AgeOfHeroes
{
    public class PawnManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject m_ExplorationPawnPrefab;
        [SerializeField] private DataStorage m_DataStorage;
        [SerializeField] private Contents m_Content;
        
        [Header("UI")]
        [SerializeField] private Button m_SpawnButton;
        [SerializeField] private Button m_DespawnButton;
        
        [Header("Core Settings")]
        [SerializeField] private float m_LeaderSpeed = 1200f;
        [SerializeField] private float m_SubLeaderSpeed = 1050f;
        [SerializeField] private float m_FollowerSpeed = 900f;
        
        [Header("Formation Settings")]
        [SerializeField] private int m_PawnsPerSubLeader = 5;
        
        // Keep track of all pawns
        private List<GameObject> m_AllPawns = new List<GameObject>();
        private ExplorationPawn m_MainLeader;
        private List<ExplorationPawn> m_SubLeaders = new List<ExplorationPawn>();
        
        // Index for next pawn to spawn
        private int m_CurrentSpawnIndex = 1; // Start at 1 since player is 0

        private void Start()
        {
            // Try to find references if not set
            if (m_DataStorage == null)
                m_DataStorage = FindObjectOfType<DataStorage>();
                
            if (m_Content == null)
                m_Content = FindObjectOfType<Contents>();
                
            if (m_ExplorationPawnPrefab == null)
            {
                ExplorationControl control = FindObjectOfType<ExplorationControl>();
                if (control != null)
                    m_ExplorationPawnPrefab = control.m_ExplorationPawnPrefab;
            }
                
            // Setup UI buttons
            if (m_SpawnButton != null)
                m_SpawnButton.onClick.AddListener(SpawnPawn);
                
            if (m_DespawnButton != null)
                m_DespawnButton.onClick.AddListener(DespawnLastPawn);
                
            // Initialize only the player pawn
            InitializePlayerPawn();
        }
        
        // Initialize just the player pawn
        public void InitializePlayerPawn()
        {
            if (m_DataStorage == null || m_Content == null || m_ExplorationPawnPrefab == null)
            {
                Debug.LogError("Required references missing! Cannot initialize pawns.");
                return;
            }
            
            // Clear any existing pawns
            ClearAllPawns();
            
            // Calculate spawn position
            Vector3 spawnPosition = Vector3.zero;
            
            // Use level point if available
            ExplorationControl exploControl = FindObjectOfType<ExplorationControl>();
            if (exploControl != null && exploControl.m_LevelPoints.Length > 0)
            {
                int levelIndex = Mathf.Max(0, m_DataStorage.m_LastLevelNum);
                if (levelIndex < exploControl.m_LevelPoints.Length)
                {
                    spawnPosition = exploControl.m_LevelPoints[levelIndex].transform.position + new Vector3(-100, -60, 0);
                    
                    if (levelIndex == 0)
                        spawnPosition = exploControl.m_LevelPoints[0].transform.position + new Vector3(-200, -100, 0);
                }
            }
            
            // Make sure there are characters in the storage
            if (m_DataStorage.m_CharactersNumber == null || m_DataStorage.m_CharactersNumber.Length == 0)
            {
                Debug.LogError("No characters found in DataStorage!");
                return;
            }
            
            // Spawn only the player character (index 0)
            SpawnPawnAtPosition(0, spawnPosition);
            
            // Setup camera to follow leader
            if (m_AllPawns.Count > 0 && m_MainLeader != null)
            {
                if (ExplorationCamera.m_Main != null)
                {
                    ExplorationCamera.m_Main.m_Target = m_MainLeader.transform;
                    
                    Vector3 cameraPos = m_MainLeader.transform.position;
                    cameraPos.z = -1000;
                    cameraPos.y += 50;
                    ExplorationCamera.m_Main.transform.position = cameraPos;
                }
            }
        }
        
        // Spawn a pawn with a specific character index at a position
        private GameObject SpawnPawnAtPosition(int charIndex, Vector3 position)
        {
            if (m_DataStorage.m_CharactersNumber == null || charIndex >= m_DataStorage.m_CharactersNumber.Length)
            {
                Debug.LogError($"Invalid character index {charIndex}. Max index: {(m_DataStorage.m_CharactersNumber != null ? m_DataStorage.m_CharactersNumber.Length - 1 : -1)}");
                return null;
            }
                
            // Get character from content
            int characterIndex = m_DataStorage.m_CharactersNumber[charIndex];
            
            if (characterIndex >= m_Content.m_Characters.Length)
            {
                Debug.LogError($"Invalid character content index {characterIndex}. Max: {m_Content.m_Characters.Length - 1}");
                return null;
            }
            
            Character characterData = m_Content.m_Characters[characterIndex];
            
            if (characterData == null)
            {
                Debug.LogError($"Character data is null for index {characterIndex}");
                return null;
            }
            
            // Create pawn object
            GameObject pawnObj = Instantiate(m_ExplorationPawnPrefab, position, Quaternion.identity);
            ExplorationPawn pawn = pawnObj.GetComponent<ExplorationPawn>();
            
            // Set tag
            pawnObj.tag = "Pawn";
            
            // Create and attach character body
            if (characterData.m_Skins.Length <= characterData.m_SkinNum || characterData.m_Skins[characterData.m_SkinNum] == null)
            {
                Debug.LogError($"Invalid skin index {characterData.m_SkinNum} for character {characterIndex}");
                Destroy(pawnObj);
                return null;
            }
            
            GameObject body = Instantiate(characterData.m_Skins[characterData.m_SkinNum].m_BaseBodyPrefab);
            
            if (characterData.m_Weapons.Length <= characterData.m_WeaponNum || characterData.m_Weapons[characterData.m_WeaponNum] == null)
            {
                Debug.LogError($"Invalid weapon index {characterData.m_WeaponNum} for character {characterIndex}");
                Destroy(body);
                Destroy(pawnObj);
                return null;
            }
            
            body.GetComponent<CharacterBody>().SetMainWeapon(characterData.m_Weapons[characterData.m_WeaponNum].m_BodySprite);
            body.transform.SetParent(pawnObj.transform, false);
            
            // Configure pawn
            pawn.m_PawnNum = m_AllPawns.Count;
            pawn.m_Animator = body.GetComponent<Animator>();
            
            // Set pawn role
            if (m_AllPawns.Count == 0)
            {
                // First pawn is the main leader
                pawn.m_ControledByPlayer = true;
                pawn.m_LeaderPawn = null;
                pawn.m_Speed = m_LeaderSpeed;
                m_MainLeader = pawn;
            }
            else
            {
                // Determine if this is a sub-leader
                pawn.m_ControledByPlayer = false;
                bool isSubLeader = ShouldBeSubLeader(m_AllPawns.Count);
                
                if (isSubLeader)
                {
                    // Sub-leader follows main leader
                    pawn.m_LeaderPawn = m_MainLeader;
                    pawn.m_Speed = m_SubLeaderSpeed;
                    m_SubLeaders.Add(pawn);
                    Debug.Log($"Created sub-leader #{m_AllPawns.Count}");
                }
                else
                {
                    // Regular follower follows appropriate sub-leader
                    ExplorationPawn leader = GetAppropriateLeader(m_AllPawns.Count);
                    pawn.m_LeaderPawn = leader;
                    pawn.m_Speed = m_FollowerSpeed;
                }
            }
            
            // Add to tracking lists
            m_AllPawns.Add(pawnObj);
            
            return pawnObj;
        }
        
        // Public method for UI button to spawn a new pawn
        public void SpawnPawn()
        {
            // Check if we've used all available characters
            if (m_CurrentSpawnIndex >= m_DataStorage.m_CharactersNumber.Length)
            {
                Debug.Log("All available characters have been spawned!");
                return;
            }
                
            // For dynamic spawning, use player position
            Vector3 spawnPosition = GetSpawnPosition(m_AllPawns.Count);
            SpawnPawnAtPosition(m_CurrentSpawnIndex, spawnPosition);
            
            // Increment the spawn index for next time
            m_CurrentSpawnIndex++;
        }
        
        // Despawn the last pawn
        public void DespawnLastPawn()
        {
            if (m_AllPawns.Count <= 1)
            {
                Debug.Log("Cannot despawn the leader pawn!");
                return;
            }
            
            // Get last pawn
            GameObject pawnToRemove = m_AllPawns[m_AllPawns.Count - 1];
            ExplorationPawn pawn = pawnToRemove.GetComponent<ExplorationPawn>();
            
            // Handle sub-leader removal
            if (m_SubLeaders.Contains(pawn))
            {
                m_SubLeaders.Remove(pawn);
                ReassignFollowers(pawn);
            }
            
            // Remove from list
            m_AllPawns.RemoveAt(m_AllPawns.Count - 1);
            
            // Adjust spawn index
            m_CurrentSpawnIndex = Mathf.Max(1, m_AllPawns.Count);
            
            // Destroy game object
            Destroy(pawnToRemove);
        }
        
        // Clear all pawns (except leader if keepLeader is true)
        public void ClearAllPawns(bool keepLeader = false)
        {
            int startIndex = keepLeader ? 1 : 0;
            
            for (int i = m_AllPawns.Count - 1; i >= startIndex; i--)
            {
                if (m_AllPawns[i] != null)
                    Destroy(m_AllPawns[i]);
            }
            
            if (keepLeader && m_AllPawns.Count > 0)
            {
                // Keep only the leader
                GameObject leaderObj = m_AllPawns[0];
                m_AllPawns.Clear();
                m_AllPawns.Add(leaderObj);
                m_CurrentSpawnIndex = 1;
                m_SubLeaders.Clear();
            }
            else
            {
                m_AllPawns.Clear();
                m_CurrentSpawnIndex = 1; // Reset to 1 since 0 is player
                m_MainLeader = null;
                m_SubLeaders.Clear();
            }
        }
        
        // Calculate spawn position for new pawns
        private Vector3 GetSpawnPosition(int pawnNumber)
        {
            if (m_MainLeader == null)
                return Vector3.zero;
                
            ExplorationPawn leaderToSpawnAround;
            bool isSubLeader = ShouldBeSubLeader(pawnNumber);
            
            if (isSubLeader)
                leaderToSpawnAround = m_MainLeader;
            else
                leaderToSpawnAround = GetAppropriateLeader(pawnNumber);
                
            // Get random position behind leader
            float angle = Random.Range(120f, 240f); // Random angle behind the leader
            float distance = Random.Range(20f, 40f);
            
            Vector3 offset = Quaternion.Euler(0, 0, angle) * Vector3.right * distance;
            return leaderToSpawnAround.transform.position + offset;
        }
        
        // Determine if a pawn should be a sub-leader
        private bool ShouldBeSubLeader(int pawnNumber)
        {
            // First pawn is main leader, not a sub-leader
            if (pawnNumber == 0)
                return false;
                
            // Every Nth pawn is a sub-leader (1st sub-leader is pawn #1)
            return pawnNumber % m_PawnsPerSubLeader == 1;
        }
        
        // Get the appropriate leader for a follower
        private ExplorationPawn GetAppropriateLeader(int pawnNumber)
        {
            // If no sub-leaders, use main leader
            if (m_SubLeaders.Count == 0)
                return m_MainLeader;
                
            // Find sub-leader for this pawn
            int subLeaderIndex = (pawnNumber - 1) / m_PawnsPerSubLeader;
            if (subLeaderIndex >= m_SubLeaders.Count)
                subLeaderIndex = m_SubLeaders.Count - 1;
                
            return m_SubLeaders[subLeaderIndex];
        }
        
        // Reassign followers after a sub-leader is removed
        private void ReassignFollowers(ExplorationPawn removedLeader)
        {
            // If no sub-leaders left, assign all to main leader
            if (m_SubLeaders.Count == 0)
            {
                foreach (GameObject pawnObj in m_AllPawns)
                {
                    if (pawnObj == null) continue;
                    
                    ExplorationPawn pawn = pawnObj.GetComponent<ExplorationPawn>();
                    if (pawn != null && pawn != m_MainLeader && pawn.m_LeaderPawn == removedLeader)
                    {
                        pawn.m_LeaderPawn = m_MainLeader;
                    }
                }
                return;
            }
            
            // Find nearest remaining sub-leader
            ExplorationPawn nearestSubLeader = null;
            float nearestDistance = float.MaxValue;
            
            foreach (ExplorationPawn subLeader in m_SubLeaders)
            {
                if (subLeader == null || subLeader == removedLeader)
                    continue;
                    
                float distance = Vector3.Distance(subLeader.transform.position, removedLeader.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestSubLeader = subLeader;
                }
            }
            
            // Reassign followers
            if (nearestSubLeader != null)
            {
                foreach (GameObject pawnObj in m_AllPawns)
                {
                    if (pawnObj == null) continue;
                    
                    ExplorationPawn pawn = pawnObj.GetComponent<ExplorationPawn>();
                    if (pawn != null && pawn != m_MainLeader && pawn.m_LeaderPawn == removedLeader)
                    {
                        pawn.m_LeaderPawn = nearestSubLeader;
                    }
                }
            }
            else
            {
                // Fall back to main leader
                foreach (GameObject pawnObj in m_AllPawns)
                {
                    if (pawnObj == null) continue;
                    
                    ExplorationPawn pawn = pawnObj.GetComponent<ExplorationPawn>();
                    if (pawn != null && pawn != m_MainLeader && pawn.m_LeaderPawn == removedLeader)
                    {
                        pawn.m_LeaderPawn = m_MainLeader;
                    }
                }
            }
        }

        public void InitializePawns()
        {
            // Call the renamed method to maintain backward compatibility
            InitializePlayerPawn();
        }
        
        // Utility methods to access private data
        public List<GameObject> GetAllPawns() { return m_AllPawns; }
        public ExplorationPawn GetMainLeader() { return m_MainLeader; }
        public List<ExplorationPawn> GetSubLeaders() { return m_SubLeaders; }
        
        // Utility methods to set dependencies
        public void SetDataStorage(DataStorage storage) { m_DataStorage = storage; }
        public void SetContent(Contents content) { m_Content = content; }
        public void SetPawnPrefab(GameObject prefab) { m_ExplorationPawnPrefab = prefab; }
    }
}
