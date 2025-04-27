using System.Collections;
using UnityEngine;

namespace AgeOfHeroes
{
    public class NPCInteractionMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [Tooltip("How fast the NPC moves when interacting with objects")]
        public float moveSpeed = 25f;

        [Tooltip("How close the NPC needs to be to consider it has reached its destination")]
        public float arrivalDistance = 5f;

        [Header("Animation Settings")]
        [Tooltip("Multiplier for animation speed relative to movement (lower = slower animation)")]
        [Range(0.1f, 5f)]
        public float animationSpeedMultiplier = 0.5f;

        [Tooltip("Debug mode - shows more console logs")]
        public bool debugMode = true;

        // Reference to key components
        private Rigidbody2D m_Body;
        private Animator m_Animator;

        // Reference to the current movement coroutine (if any)
        private Coroutine currentMovementCoroutine;

        // Whether the NPC is currently moving
        public bool IsMoving { get; private set; }

        // Original position to return to after interaction
        private Vector3 returnPosition;
        private bool returnPositionSet = false;

        private void Awake()
        {
            // Store initial position
            if (debugMode) Debug.Log($"Initial return position set to: {returnPosition}");

            // Get the required components
            m_Body = GetComponent<Rigidbody2D>();
            if (m_Body == null)
            {
                Debug.LogWarning($"No Rigidbody2D found on {gameObject.name}. Adding one for movement.");
                m_Body = gameObject.AddComponent<Rigidbody2D>();
                m_Body.gravityScale = 0;
                m_Body.freezeRotation = true;
                m_Body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                m_Body.interpolation = RigidbodyInterpolation2D.Interpolate;
            }

            // Try to find animator in BodyBase child
            Transform bodyBase = transform.Find("BodyBase");
            if (bodyBase != null)
            {
                m_Animator = bodyBase.GetComponent<Animator>();
                if (m_Animator != null)
                {
                    if (debugMode) Debug.Log($"Found animator on BodyBase for {gameObject.name}");
                }
            }

            // If not found in BodyBase, try on this object or any child
            if (m_Animator == null)
            {
                m_Animator = GetComponentInChildren<Animator>();
                if (m_Animator != null)
                {
                    if (debugMode) Debug.Log($"Found animator in children of {gameObject.name}");
                }
            }

            if (m_Animator == null)
            {
                Debug.LogWarning($"No Animator found for {gameObject.name}. Animations won't play.");
            }
        }

        public void StartInteraction(Vector3 targetPosition, System.Action onReachedTarget = null)
        {
            if (!returnPositionSet)
            {
                returnPosition = transform.position;
                returnPositionSet = true;
                if (debugMode) Debug.Log($"Set missing return position: {returnPosition}");
            }

            if (currentMovementCoroutine != null)
            {
                StopCoroutine(currentMovementCoroutine);
                currentMovementCoroutine = null;
            }

            // Always ensure we have a valid return position

            // Start movement to target
            currentMovementCoroutine = StartCoroutine(MoveToTargetCoroutine(targetPosition, onReachedTarget));

            if (debugMode) Debug.Log($"Started moving to target: {targetPosition}, return position: {returnPosition}");
        }

        public void ReturnFromInteraction(System.Action onReturned = null)
        {
            if (debugMode) Debug.Log($"ReturnFromInteraction called. Return position: {returnPosition}");

            if (currentMovementCoroutine != null)
            {
                StopCoroutine(currentMovementCoroutine);
                currentMovementCoroutine = null;
            }

            // Ensure we have a valid return position
            if (!returnPositionSet)
            {
                Debug.LogWarning("No return position was set! Using current position.");
                returnPosition = transform.position;
                returnPositionSet = true;
            }

            // Start movement back to original position
            currentMovementCoroutine = StartCoroutine(MoveToTargetCoroutine(returnPosition, onReturned));
        }

        private IEnumerator MoveToTargetCoroutine(Vector3 targetPosition, System.Action onArrived = null)
        {
            IsMoving = true;
            if (debugMode) Debug.Log($"Starting movement to {targetPosition}");

            // Check if this is a return journey
            bool isReturningHome = Vector3.Distance(targetPosition, returnPosition) < 1f;

            // Store the target position precisely
            Vector2 targetPos2D = new Vector2(targetPosition.x, targetPosition.y);

            // Initial distance check
            float distance = Vector2.Distance(
                new Vector2(transform.position.x, transform.position.y),
                targetPos2D
            );

            if (debugMode) Debug.Log($"Initial distance to target: {distance}, arrival threshold: {arrivalDistance}");

            // Keep moving until we're close enough
            float startTime = Time.time;

            // For return journeys, use a stricter arrival distance
            float effectiveArrivalDistance = isReturningHome ? arrivalDistance * 0.2f : arrivalDistance;

            while (distance > effectiveArrivalDistance)
            {
                // Safety timeout after 15 seconds
                if (Time.time - startTime > 15f)
                {
                    Debug.LogWarning($"Movement timeout - forcing arrival at {targetPosition}");
                    // Force position to ensure completion
                    transform.position = targetPosition;
                    break;
                }

                // Calculate direction and movement
                Vector2 direction = (targetPos2D - (Vector2)transform.position).normalized;

                // Use direct velocity-based movement
                m_Body.linearVelocity = direction * moveSpeed;

                // Handle sprite flipping based on direction
                if (direction.x != 0)
                {
                    transform.localScale = new Vector3(
                        direction.x > 0 ? 1 : -1,
                        transform.localScale.y,
                        transform.localScale.z
                    );
                }

                // Update distance
                distance = Vector2.Distance(
                    new Vector2(transform.position.x, transform.position.y),
                    targetPos2D
                );

                if (debugMode && Time.frameCount % 60 == 0) // Log every 60 frames to avoid spamming
                {
                    Debug.Log($"Distance to target: {distance}, velocity: {m_Body.linearVelocity.magnitude}");
                }

                yield return null;
            }

            // If returning home, snap exactly to the position
            if (isReturningHome)
            {
                // Skip the smooth positioning coroutine and just set the position directly
                //transform.position = returnPosition;

                if (debugMode) Debug.Log("Set exactly to return position");
            }

            // Stop all movement
            m_Body.linearVelocity = Vector2.zero;
            IsMoving = false;

            // Make sure animation stops
            if (m_Animator != null && HasParameter(m_Animator, "run-blend"))
            {
                m_Animator.SetFloat("run-blend", 0f);
            }

            currentMovementCoroutine = null;

            if (debugMode) Debug.Log($"Arrived at destination {targetPosition}");

            // Invoke callback
            if (onArrived != null)
            {
                if (debugMode) Debug.Log("Invoking arrival callback");
                onArrived.Invoke();
            }
            else if (debugMode)
            {
                Debug.Log("No arrival callback to invoke");
            }
        }


        // New method to smoothly set the final position without flickering
        private IEnumerator SmoothFinalPositioning(Vector3 finalPosition)
        {
            // Get current position
            Vector3 startPosition = transform.position;

            // Smoothly interpolate over a very short time (3 frames)
            float elapsedTime = 0;
            float duration = 0.05f; // 50ms, short enough to be almost imperceptible

            // Set IsMoving to true to maintain animation during final positioning
            IsMoving = true;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);

                // Use smooth step for more natural easing
                float smoothT = t * t * (3f - 2f * t);

                // Set position with smooth interpolation
                transform.position = Vector3.Lerp(startPosition, finalPosition, smoothT);

                yield return null;
            }

            // Ensure we're exactly at the final position
            transform.position = finalPosition;

            // Stop all movement
            m_Body.linearVelocity = Vector2.zero;

            // Important: Set IsMoving to false AFTER stopping the movement
            IsMoving = false;

            if (debugMode) Debug.Log("Smoothly positioned at return position");

            // Explicitly set animation to idle state
            if (m_Animator != null && HasParameter(m_Animator, "run-blend"))
            {
                m_Animator.SetFloat("run-blend", 0f);
            }
        }

        private void FixedUpdate()
        {
            // Initialize m_Animator on first frame if not found in Awake
            if (m_Animator == null && Time.frameCount < 10)
            {
                Transform bodyBase = transform.Find("BodyBase");
                if (bodyBase != null)
                {
                    m_Animator = bodyBase.GetComponent<Animator>();
                }

                if (m_Animator == null)
                {
                    m_Animator = GetComponentInChildren<Animator>();
                }
            }

            // Update animation based on velocity
            if (m_Animator != null && m_Body != null)
            {
                // More sensitive velocity check - use a very small threshold (0.01f instead of 0.1f)
                bool shouldBeIdle = !IsMoving || m_Body.linearVelocity.sqrMagnitude < 0.0001f; // Square magnitude is faster

                // Using the animation speed multiplier to control animation speed
                float speed = shouldBeIdle ?
                    0f : // Force to exactly 0 when not moving or barely moving
                    m_Body.linearVelocity.magnitude / 20f * animationSpeedMultiplier;

                // Check if animator has the run-blend parameter
                if (HasParameter(m_Animator, "run-blend"))
                {
                    // Set speed to precisely 0 when idle
                    m_Animator.SetFloat("run-blend", speed);

                    // Only play idle animation if we're not moving
                    if (shouldBeIdle && !m_Animator.GetCurrentAnimatorStateInfo(0).IsName("run-blend-1"))
                    {
                        m_Animator.Play("run-blend-1");
                    }
                }
            }

            // Apply dampening to slow down naturally but only when not being directly moved by a coroutine
            if (m_Body != null && !IsMoving)
            {
                m_Body.linearVelocity = Vector2.zero; // Immediately stop movement instead of lerping
            }
        }

        // Add this method to NPCInteractionMovement.cs
        public void ForceStopAnimation()
        {
            if (m_Body != null)
            {
                // Ensure velocity is exactly zero
                m_Body.linearVelocity = Vector2.zero;
            }

            // Force animation to stop with immediate effect
            if (m_Animator != null && HasParameter(m_Animator, "run-blend"))
            {
                m_Animator.SetFloat("run-blend", 0f);

                // Force the animator to update immediately
                m_Animator.Update(0f);

                // Explicitly play idle animation
                if (!m_Animator.GetCurrentAnimatorStateInfo(0).IsName("run-blend-1"))
                {
                    m_Animator.Play("run-blend-1");
                }
            }

            // Update state flag
            IsMoving = false;
        }




        // Helper function to check if an animator has a specific parameter
        private bool HasParameter(Animator animator, string paramName)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.name == paramName)
                {
                    return true;
                }
            }
            return false;
        }

        // For debugging
        private void OnDrawGizmosSelected()
        {
            // Draw the arrival distance threshold
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, arrivalDistance);

            // Draw the return position
            if (Application.isPlaying && returnPositionSet)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(returnPosition, 2f);
                Gizmos.DrawLine(transform.position, returnPosition);
            }
        }
    }
}
