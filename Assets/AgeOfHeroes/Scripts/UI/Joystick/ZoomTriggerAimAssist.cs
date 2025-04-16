using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;
using System.Collections.Generic;
using Lean.Touch;

namespace AgeOfHeroes
{
    // This component should be attached to your ProCamera2DTriggerZoom objects
    [RequireComponent(typeof(ProCamera2DTriggerZoom))]
    public class ZoomTriggerAimAssist : MonoBehaviour
    {
        [Header("Target Detection")]
        [Tooltip("Layer mask for detecting the pawn (optional)")]
        public LayerMask pawnLayerMask = -1; // Default to everything

        [Header("Aim Assist Settings")]
        [Tooltip("The outer radius where magnetism starts working")]
        public float attractionRadius = 300f;
        
        [Tooltip("The inner radius where full attraction happens when idle")]
        public float fullAttractionRadius = 150f;
        
        [Tooltip("How strong the 'nudge' is when moving (0-1)")]
        [Range(0f, 1f)]
        public float movingAttractionStrength = 0.3f;
        
        [Tooltip("How strong the full attraction is when idle (0-1)")]
        [Range(0f, 1f)]
        public float idleAttractionStrength = 1.0f;
        
        [Tooltip("How fast the pawn moves toward the zoom trigger")]
        public float aimAssistSpeed = 200f;
        
        [Tooltip("Time required with no input to consider the pawn idle")]
        public float idleThresholdTime = 0.5f;

        [Header("Destination and Overshoot Settings")]
        [Tooltip("Distance at which the pawn is considered to have reached the destination")]
        public float destinationReachedThreshold = 30f;
        
        [Tooltip("Small dead zone around the center where no forces are applied")]
        public float deadZoneRadius = 15f;
        
        [Tooltip("How long to disable attraction after the player moves away from the center")]
        public float overshootProtectionTime = 0.8f;
        
        [Tooltip("Show debug visualization in Scene view")]
        public bool showDebugVisuals = false;

        // Private variables
        private ProCamera2DTriggerZoom zoomTrigger;
        private ExplorationPawn targetPawn;
        private Rigidbody2D pawnRigidbody;
        private Joystick joystick;
        private float lastJoystickActiveTime;
        private bool isCurrentlyIdle = true;
        
        // Destination tracking
        private bool hasReachedDestination = false;
        private float lastDestinationReachedTime = 0f;
        private float lastOvershootTime = 0f;
        private Vector2 lastMovementDirection = Vector2.zero;

        // Static tracking to prevent multiple triggers from affecting the pawn
        private static ZoomTriggerAimAssist activeTrigger = null;      // Trigger that has the pawn at destination
        private static ZoomTriggerAimAssist influencingTrigger = null; // Trigger currently influencing the pawn
        private static ExplorationPawn capturedPawn = null;
        private static float lastInfluenceTime = 0f;                   // When the last influence occurred
        private static Dictionary<ZoomTriggerAimAssist, float> triggerInfluenceStrengths = new Dictionary<ZoomTriggerAimAssist, float>();
        
        // How long a trigger's influence lasts before others can compete again
        private const float INFLUENCE_LOCK_TIME = 0.5f;

        private void Start()
        {
            // Get required components
            zoomTrigger = GetComponent<ProCamera2DTriggerZoom>();
            
            // Find the joystick in the scene
            joystick = Joystick.GeneralJoystick;
            if (joystick == null)
            {
                Debug.LogWarning("ZoomTriggerAimAssist: Could not find Joystick. Aim assist may not work correctly.");
            }
            
            // Initialize time
            lastJoystickActiveTime = Time.time;
            lastDestinationReachedTime = -overshootProtectionTime; // Allow aim assist immediately
            lastOvershootTime = -overshootProtectionTime;
            
            // Find the ExplorationPawn instance
            FindExplorationPawn();
        }

        private void OnDestroy()
        {
            // Release the pawn if this trigger is destroyed while active
            if (activeTrigger == this)
            {
                ReleaseCapture();
            }
            
            // Also release influence if we're the influencing trigger
            if (influencingTrigger == this)
            {
                influencingTrigger = null;
            }
            
            // Remove from influence strengths dictionary
            if (triggerInfluenceStrengths.ContainsKey(this))
            {
                triggerInfluenceStrengths.Remove(this);
            }
        }

        private void FindExplorationPawn()
        {
            // Find the ExplorationPawn in the scene
            ExplorationPawn[] pawns = FindObjectsOfType<ExplorationPawn>();
            if (pawns.Length > 0)
            {
                // Use the first one found (or you could find the closest one if there are multiple)
                targetPawn = pawns[0];
                pawnRigidbody = targetPawn.GetComponent<Rigidbody2D>();
                
                if (pawnRigidbody == null)
                {
                    Debug.LogWarning("ZoomTriggerAimAssist: ExplorationPawn doesn't have a Rigidbody2D component.");
                }
            }
            else
            {
                Debug.LogWarning("ZoomTriggerAimAssist: No ExplorationPawn found in the scene.");
            }
        }

        private void Update()
        {
            // Skip if we don't have a target pawn
            if (targetPawn == null || pawnRigidbody == null)
            {
                // Try to find the pawn again in case it was instantiated after this component
                FindExplorationPawn();
                
                if (targetPawn == null || pawnRigidbody == null)
                    return;
            }
            
            // Skip if pawn can't move
            if (!targetPawn.m_CanMove)
                return;
            
            // Check joystick activity
            CheckJoystickActivity();
            
            // Check if we're at the destination
            CheckDestinationReached();
            
            // Process aim assist if pawn is in range, hasn't reached destination
            // AND if we're allowed to influence the pawn
            if (CanInfluencePawn())
            {
                if (!hasReachedDestination)
                {
                    float influenceStrength = CalculateCurrentInfluenceStrength();
                    
                    // Only try to influence if we have a reasonable strength
                    if (influenceStrength > 0.01f)
                    {
                        // Register our influence
                        RegisterInfluence(influenceStrength);
                        
                        // Process the actual aim assist
                        ProcessAimAssist(influenceStrength);
                    }
                }
                else
                {
                    // We've reached destination, maintain our control and apply damping
                    ApplyDampingForceWhenAtDestination();
                }
            }
            else
            {
                // We can't influence, so remove our strength from the dictionary
                if (triggerInfluenceStrengths.ContainsKey(this))
                {
                    triggerInfluenceStrengths[this] = 0f;
                }
            }
        }
        
        private float CalculateCurrentInfluenceStrength()
        {
            // Calculate direction and distance to pawn
            Vector2 pawnPosition = targetPawn.transform.position;
            Vector2 directionToPawn = (pawnPosition - (Vector2)transform.position);
            float distanceToPawn = directionToPawn.magnitude;
            
            // Skip if pawn is too far away
            if (distanceToPawn > attractionRadius)
                return 0f;
                
            // Skip if we're in the dead zone (already very close to target)
            if (distanceToPawn <= deadZoneRadius)
                return 0f;
                
            // Skip if we've recently overshot and are still in the protection period
            if (Time.time - lastOvershootTime < overshootProtectionTime)
                return 0f;
            
            // Direction from pawn to trigger (opposite of directionToPawn)
            Vector2 directionToTrigger = -directionToPawn.normalized;
            
            // Determine attraction strength based on state and distance
            float attractionStrength = 0f;
            
            if (isCurrentlyIdle && distanceToPawn <= fullAttractionRadius)
            {
                // Full attraction when idle and within inner radius
                attractionStrength = idleAttractionStrength;
            }
            else if (distanceToPawn <= attractionRadius)
            {
                // Partial attraction when moving and within outer radius
                Vector2 activeStickDir = Vector2.zero;
                
                if (joystick != null && joystick.LeftStick != null)
                {
                    activeStickDir = joystick.LeftStick.StickDirection;
                }
                
                // Reduce attraction if moving in a different direction
                float directionAlignment = Vector2.Dot(activeStickDir, directionToTrigger);
                
                if (directionAlignment < 0 && activeStickDir.sqrMagnitude > 0.3f)
                {
                    attractionStrength = movingAttractionStrength * 0.5f;
                }
                else
                {
                    attractionStrength = movingAttractionStrength;
                }
                
                // Scale attraction based on distance (stronger when closer)
                float distanceRatio = 1f - (distanceToPawn / attractionRadius);
                attractionStrength *= distanceRatio;
            }
            
            return attractionStrength;
        }
        
        private bool CanInfluencePawn()
        {
            // Case 1: Dead zone protection - check if pawn is at a destination
            // If there's an active trigger with the pawn at its destination
            if (activeTrigger != null && activeTrigger != this && capturedPawn == targetPawn)
            {
                float distanceToCapturer = Vector2.Distance(
                    targetPawn.transform.position, 
                    activeTrigger.transform.position
                );
                
                // Can only influence if the pawn has moved outside the dead zone of the active trigger
                if (distanceToCapturer <= activeTrigger.deadZoneRadius)
                {
                    return false;
                }
            }
            
            // Case 2: Influence protection - check if another trigger is already influencing
            if (influencingTrigger != null && influencingTrigger != this && 
                Time.time - lastInfluenceTime < INFLUENCE_LOCK_TIME)
            {
                // Check if we should be able to override the current influencer
                bool canOverride = ShouldOverrideCurrentInfluencer();
                if (!canOverride)
                {
                    return false;
                }
            }
            
            return true;
        }
        
        private bool ShouldOverrideCurrentInfluencer()
        {
            // If we don't have a current influencer, we can always influence
            if (influencingTrigger == null)
                return true;
            
            // Calculate our distance and potential influence
            float ourDistance = Vector2.Distance(transform.position, targetPawn.transform.position);
            float ourStrength = CalculateCurrentInfluenceStrength();
            
            if (triggerInfluenceStrengths.TryGetValue(influencingTrigger, out float theirStrength))
            {
                // If we're significantly closer OR our influence is significantly stronger
                if (ourStrength > theirStrength * 1.5f)
                {
                    return true;
                }
                
                float theirDistance = Vector2.Distance(influencingTrigger.transform.position, targetPawn.transform.position);
                if (ourDistance < theirDistance * 0.7f)
                {
                    return true;
                }
            }
            
            // Default: don't override
            return false;
        }
        
        private void RegisterInfluence(float strength)
        {
            // Add or update our influence strength
            triggerInfluenceStrengths[this] = strength;
            
            // If there's no current influencer or we're the current one, update the time
            if (influencingTrigger == null || influencingTrigger == this)
            {
                influencingTrigger = this;
                lastInfluenceTime = Time.time;
                return;
            }
            
            // Find the trigger with the strongest influence
            ZoomTriggerAimAssist strongestTrigger = null;
            float strongestInfluence = 0f;
            
            foreach (var pair in triggerInfluenceStrengths)
            {
                if (pair.Value > strongestInfluence)
                {
                    strongestTrigger = pair.Key;
                    strongestInfluence = pair.Value;
                }
            }
            
            // Update the influencing trigger if needed
            if (strongestTrigger != null && strongestTrigger != influencingTrigger)
            {
                influencingTrigger = strongestTrigger;
                lastInfluenceTime = Time.time;
            }
        }
        
        private void CaptureControl()
        {
            // Set this trigger as the active one controlling the pawn
            activeTrigger = this;
            capturedPawn = targetPawn;
            
            // Also set as influencing
            influencingTrigger = this;
            lastInfluenceTime = Time.time;
        }
        
        private void ReleaseCapture()
        {
            // Only release if we're the active trigger
            if (activeTrigger == this)
            {
                activeTrigger = null;
                capturedPawn = null;
                
                // Allow for reevaluation of influencing trigger
                if (influencingTrigger == this)
                {
                    influencingTrigger = null;
                }
            }
        }
        
        private void ApplyDampingForceWhenAtDestination()
        {
            // Only apply damping if the pawn is still moving with some velocity
            if (pawnRigidbody.linearVelocity.sqrMagnitude > 1f)
            {
                // Apply a gentle damping to slow down the pawn
                Vector2 dampingForce = -pawnRigidbody.linearVelocity * 0.5f * Time.deltaTime;
                pawnRigidbody.linearVelocity += dampingForce;
            }
        }
        
        private void CheckJoystickActivity()
        {
            if (joystick == null)
                return;
                
            // Check if either joystick is active
            bool leftStickActive = joystick.LeftStick != null && 
                                  joystick.LeftStick.StickDirection.sqrMagnitude > 0.1f;
                                  
            bool rightStickActive = joystick.RightStick != null && 
                                   joystick.RightStick.StickDirection.sqrMagnitude > 0.1f;
            
            if (leftStickActive || rightStickActive)
            {
                lastJoystickActiveTime = Time.time;
                isCurrentlyIdle = false;
                
                // Store the movement direction for overshoot detection
                if (joystick.LeftStick != null && joystick.LeftStick.StickDirection.sqrMagnitude > 0.1f)
                {
                    lastMovementDirection = joystick.LeftStick.StickDirection;
                }
            }
            else if (Time.time - lastJoystickActiveTime > idleThresholdTime)
            {
                isCurrentlyIdle = true;
            }

            if(LeanTouch.Fingers.Count > 0 && LeanTouch.Fingers[0].IsActive)
            {
                isCurrentlyIdle = false;
            }
            else
            {
                isCurrentlyIdle = true;
            }
        }
        
        private void CheckDestinationReached()
        {
            Vector2 pawnPosition = targetPawn.transform.position;
            float distanceToPawn = Vector2.Distance(pawnPosition, transform.position);
            
            // Check if we've reached the destination
            if (distanceToPawn <= destinationReachedThreshold)
            {
                if (!hasReachedDestination)
                {
                    hasReachedDestination = true;
                    lastDestinationReachedTime = Time.time;
                    
                    // Capture control of the pawn
                    CaptureControl();
                }
            }
            else
            {
                // We're outside the destination threshold
                if (hasReachedDestination)
                {
                    hasReachedDestination = false;
                    
                    // If we're the active trigger and we're outside the threshold, release the pawn
                    if (activeTrigger == this && distanceToPawn > destinationReachedThreshold)
                    {
                        ReleaseCapture();
                    }
                    
                    // Check if we're moving away from the center (potential overshoot)
                    Vector2 directionToTrigger = ((Vector2)transform.position - pawnPosition).normalized;
                    float directionAlignment = Vector2.Dot(lastMovementDirection, directionToTrigger);
                    
                    // Negative dot product means we're moving away from the trigger
                    if (directionAlignment < -0.2f && lastMovementDirection.sqrMagnitude > 0.3f)
                    {
                        lastOvershootTime = Time.time;
                    }
                }
            }
        }
        
        private void ProcessAimAssist(float calculatedStrength)
        {
            // We've already calculated the influence strength, so we can just use it
            
            // Calculate direction to trigger
            Vector2 pawnPosition = targetPawn.transform.position;
            Vector2 directionToPawn = (pawnPosition - (Vector2)transform.position);
            Vector2 directionToTrigger = -directionToPawn.normalized;
            
            // Apply the attraction force if strength is sufficient
            if (calculatedStrength > 0.01f)
            {
                ApplyAttractionForce(directionToTrigger, calculatedStrength);
            }
        }
        
        private void ApplyAttractionForce(Vector2 direction, float strength)
        {
            Vector2 attractionForce = direction * aimAssistSpeed * strength * Time.deltaTime;
            
            // Apply as velocity change for more direct control
            pawnRigidbody.linearVelocity += attractionForce;
            
            // Update animation direction if attraction is strong enough
            if (strength > 0.5f && targetPawn.m_Animator != null)
            {
                // Access the animation direction field via reflection if it exists
                var field = targetPawn.GetType().GetField("m_LastMovementDir");
                if (field != null)
                {
                    field.SetValue(targetPawn, direction);
                }
            }
        }
        
        private void OnDrawGizmos()
        {
            if (!showDebugVisuals)
                return;
                
            // Check our status
            bool isActive = (activeTrigger == this);
            bool isInfluencer = (influencingTrigger == this);
            
            // Get our current influence strength
            float currentStrength = 0f;
            triggerInfluenceStrengths.TryGetValue(this, out currentStrength);
                
            // Draw outer attraction radius
            Gizmos.color = new Color(0.2f, 0.8f, 1f, 0.2f);
            Gizmos.DrawWireSphere(transform.position, attractionRadius);
            
            // Draw inner full attraction radius
            Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f);
            Gizmos.DrawWireSphere(transform.position, fullAttractionRadius);
            
            // Draw destination reached threshold (highlight if we're the active trigger)
            if (isActive)
            {
                Gizmos.color = new Color(0f, 1f, 0f, 0.4f);
                Gizmos.DrawSphere(transform.position, destinationReachedThreshold * 0.1f); // Draw a small sphere at center
                Gizmos.DrawWireSphere(transform.position, destinationReachedThreshold);
            }
            else
            {
                Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
                Gizmos.DrawWireSphere(transform.position, destinationReachedThreshold);
            }
            
            // Draw dead zone radius - solid if this is the active trigger
            if (isActive)
            {
                // Draw solid sphere to show active dead zone
                Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
                Gizmos.DrawSphere(transform.position, deadZoneRadius);
            }
            else
            {
                // Draw wire sphere for inactive trigger
                Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
                Gizmos.DrawWireSphere(transform.position, deadZoneRadius);
            }
            
            // Draw a halo around influencing trigger
            if (isInfluencer && targetPawn != null)
            {
                Gizmos.color = new Color(1f, 1f, 0f, 0.4f); // Yellow halo
                Gizmos.DrawWireSphere(transform.position, fullAttractionRadius * 1.1f);
                
                // Draw a pulsing line to show active influence
                float pulseAmount = Mathf.PingPong(Time.time * 2f, 1f) * 0.5f + 0.5f; // 0.5 to 1.0 range
                Gizmos.color = new Color(1f, 1f, 0f, pulseAmount); // Pulsing yellow
                Gizmos.DrawLine(transform.position, targetPawn.transform.position);
            }
            
            // Draw line to pawn if within range and pawn exists
            if (targetPawn != null)
            {
                float distance = Vector2.Distance(transform.position, targetPawn.transform.position);
                if (distance <= attractionRadius)
                {
                    // Choose color based on state
                    if (isActive && hasReachedDestination)
                    {
                        // Bright green for active trigger that has captured the pawn
                        Gizmos.color = new Color(0f, 1f, 0f, 1f);
                    }
                    else if (isInfluencer)
                    {
                        // Yellow for the current influencing trigger
                        float pulseAmount = Mathf.PingPong(Time.time * 2f, 1f) * 0.5f + 0.5f;
                        Gizmos.color = new Color(1f, 1f, 0f, pulseAmount);
                    }
                    else if (!CanInfluencePawn())
                    {
                        // Gray for triggers that can't influence the pawn
                        Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                    }
                    else if (Time.time - lastOvershootTime < overshootProtectionTime)
                    {
                        // Red for triggers in overshoot protection
                        Gizmos.color = Color.red;
                    }
                    else
                    {
                        // Blue for normal potential attraction
                        Gizmos.color = new Color(0.2f, 0.6f, 1f, 0.8f);
                    }
                    
                    // Draw line with thickness based on influence
                    Gizmos.DrawLine(transform.position, targetPawn.transform.position);
                    
                    // Draw label to show status when selected
                    if (UnityEditor.Selection.activeGameObject == gameObject)
                    {
                        string statusText = "";
                        
                        if (isActive && hasReachedDestination)
                            statusText = "ACTIVE - DESTINATION REACHED";
                        else if (isInfluencer)
                            statusText = "INFLUENCING - Strength: " + currentStrength.ToString("F2");
                        else if (!CanInfluencePawn())
                        {
                            if (activeTrigger != null)
                                statusText = "BLOCKED - PAWN AT DESTINATION";
                            else
                                statusText = "BLOCKED - STRONGER TRIGGER INFLUENCING";
                        }
                        else if (Time.time - lastOvershootTime < overshootProtectionTime)
                            statusText = "OVERSHOOT PROTECTION ACTIVE";
                        else
                            statusText = "READY - Potential Strength: " + currentStrength.ToString("F2");
                            
                        UnityEditor.Handles.Label(transform.position + Vector3.up * 50, statusText);
                    }
                        }
                    }
                }
    }
}

