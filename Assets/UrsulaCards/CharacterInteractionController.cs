using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgeOfHeroes
{
    public class CharacterInteractionController : MonoBehaviour
    {
        [Header("Interaction Settings")]
        [Tooltip("How often this character checks for interactable objects (seconds)")]
        public float interactionCheckInterval = 5f;

        [Tooltip("Chance (0-1) that character will attempt interaction when object is found")]
        [Range(0, 1)]
        public float interactionProbability = 0.3f;

        [Tooltip("Range to detect interactable objects")]
        public float detectionRange = 150f;

        // Static dictionary to track who's interacting with what
        private static Dictionary<InteractableObject, List<GameObject>> currentInteractors =
            new Dictionary<InteractableObject, List<GameObject>>();

        private float nextInteractionCheckTime;
        private Coroutine activeInteractionRoutine;

        // Movement component reference
        private NPCInteractionMovement npcMovement;

        // Add this method to clear the static dictionary when the scene loads
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void ResetInteractorsOnLoad()
        {
            currentInteractors.Clear();
            Debug.Log("Cleared all interactors on scene load");
        }

        private void Awake()
        {
            // Get or add the movement component
            npcMovement = GetComponent<NPCInteractionMovement>();
            if (npcMovement == null)
            {
                npcMovement = gameObject.AddComponent<NPCInteractionMovement>();
                Debug.Log($"Added NPCInteractionMovement component to {gameObject.name}");
            }
        }

        private void Start()
        {
            // Randomize first check time so not all characters check at once
            nextInteractionCheckTime = Time.time + Random.Range(1f, interactionCheckInterval);
        }

        private void Update()
        {
            // Skip if already interacting
            if (activeInteractionRoutine != null)
                return;

            // Check for objects periodically
            if (Time.time >= nextInteractionCheckTime)
            {
                nextInteractionCheckTime = Time.time + interactionCheckInterval;

                // Random chance to skip check (for variety)
                if (Random.value <= interactionProbability)
                {
                    CheckForInteractableObjects();
                }
            }
        }

        private void CheckForInteractableObjects()
        {
            Debug.Log($"{gameObject.name} checking for interactable objects. My position: {transform.position}");

            // Find nearby interactable objects
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRange);
            Debug.Log($"Found {colliders.Length} colliders in range");

            // Shuffle the colliders for randomness
            for (int i = 0; i < colliders.Length; i++)
            {
                int randomIndex = Random.Range(i, colliders.Length);
                var temp = colliders[i];
                colliders[i] = colliders[randomIndex];
                colliders[randomIndex] = temp;
            }

            foreach (Collider2D collider in colliders)
            {
                Debug.Log($"Checking collider: {collider.gameObject.name}");

                InteractableObject interactable = collider.GetComponent<InteractableObject>();
                if (interactable == null)
                {
                    Debug.Log($"No InteractableObject on {collider.gameObject.name}");
                    continue;
                }

                // Check if this character can interact
                if (!interactable.CanCharacterInteract(gameObject))
                {
                    Debug.Log($"Character cannot interact with {interactable.name}");
                    continue;
                }

                // Check if too many characters are already interacting with this object
                if (GetInteractorCount(interactable) >= 1)
                {
                    Debug.Log($"Too many interactors on {interactable.name}. Current count: {GetInteractorCount(interactable)}");

                    // Debug which objects are interacting
                    if (currentInteractors.ContainsKey(interactable))
                    {
                        foreach (var interactor in currentInteractors[interactable])
                        {
                            Debug.Log($"  - Interactor: {(interactor ? interactor.name : "NULL")}");
                        }
                    }
                    continue;
                }

                // Start interaction
                Debug.Log($"Starting interaction with {interactable.name}");
                activeInteractionRoutine = StartCoroutine(PerformInteraction(interactable));
                break; // Only interact with one object at a time
            }
        }

        private int GetInteractorCount(InteractableObject interactable)
        {
            if (!currentInteractors.ContainsKey(interactable))
                return 0;

            // Clean up null references
            currentInteractors[interactable].RemoveAll(i => i == null);

            // If the list is empty after cleanup, remove the key entirely
            if (currentInteractors[interactable].Count == 0)
            {
                currentInteractors.Remove(interactable);
                return 0;
            }

            return currentInteractors[interactable].Count;
        }

        private IEnumerator PerformInteraction(InteractableObject interactable)
        {
            // Register as interacting with this object
            if (!currentInteractors.ContainsKey(interactable))
            {
                currentInteractors[interactable] = new List<GameObject>();
            }
            currentInteractors[interactable].Add(gameObject);

            Debug.Log($"{gameObject.name} is going to interact with {interactable.name}");

            // Move to interactable position using our NPCInteractionMovement
            bool hasArrived = false;
            npcMovement.StartInteraction(interactable.transform.position, () => { hasArrived = true; });

            // Wait until we arrive or timeout
            float startTime = Time.time;
            while (!hasArrived && !interactable.IsInRange(transform.position))
            {
                // Safety check
                if (Time.time - startTime > 10f)
                {
                    Debug.LogWarning($"{gameObject.name} took too long to reach {interactable.name}");
                    break;
                }

                yield return null;
            }

            // Perform interaction
            Debug.Log($"{gameObject.name} interacting with {interactable.name}");
            npcMovement.ForceStopAnimation();
            interactable.Interact(gameObject);

            // *** DEBUGGING - Add these lines to see if the wait is working ***
            float duration = interactable.interactionDuration;
            Debug.Log($"Waiting for {duration} seconds before returning...");

            // Wait for interaction duration
            yield return new WaitForSeconds(duration);

            Debug.Log("Wait complete, now starting return journey...");
            // *** END DEBUGGING ***

            // Return to original position
            bool hasReturned = false;
            Debug.Log("Starting return journey...");
            npcMovement.ReturnFromInteraction(() => {
                hasReturned = true;
                Debug.Log("Return journey completed!");
            });

            // Wait until we arrive back or timeout
            startTime = Time.time;
            float maxReturnTime = 25f; // Increased timeout for return journey
            while (!hasReturned)
            {
                // Log progress during return journey
                if (Time.frameCount % 60 == 0)
                {
                    Debug.Log($"Still waiting for return... Time elapsed: {Time.time - startTime}");
                }

                // Safety check
                if (Time.time - startTime > maxReturnTime)
                {
                    Debug.LogWarning($"{gameObject.name} took too long to return from {interactable.name} - forcing completion");
                    hasReturned = true;
                    break;
                }

                yield return null;
            }

            // Clean up - IMPORTANT: Make sure this runs even if we break from the loop
            if (currentInteractors.ContainsKey(interactable))
            {
                currentInteractors[interactable].Remove(gameObject);

                // If the list is empty after removal, remove the key entirely
                if (currentInteractors[interactable].Count == 0)
                {
                    currentInteractors.Remove(interactable);
                }
            }

            Debug.Log($"{gameObject.name} finished interacting with {interactable.name}");
            activeInteractionRoutine = null;
        }




        // Add this to ensure cleanup when the object is destroyed
        private void OnDestroy()
        {
            // Find any interactables this object is registered with and remove it
            List<InteractableObject> toCleanup = new List<InteractableObject>();

            foreach (var kvp in currentInteractors)
            {
                if (kvp.Value.Contains(gameObject))
                {
                    kvp.Value.Remove(gameObject);

                    // If the list is now empty, mark for removal
                    if (kvp.Value.Count == 0)
                    {
                        toCleanup.Add(kvp.Key);
                    }
                }
            }

            // Remove any empty entries
            foreach (var key in toCleanup)
            {
                currentInteractors.Remove(key);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
        }
    }
}
