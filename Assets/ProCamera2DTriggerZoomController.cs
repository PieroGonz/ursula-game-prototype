using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;
using System.Collections.Generic;

public class ProCamera2DTriggerZoomController : MonoBehaviour
{
    [Tooltip("When checked, the zoom will be triggered programmatically")]
    public bool TriggerZoom = false;

    [Tooltip("If true, will reset the zoom when this boolean is toggled off")]
    public bool ResetWhenToggled = true;

    [Tooltip("The GameObjects that this zoom trigger is associated with")]
    public List<GameObject> associatedCharacters = new List<GameObject>();

    [Tooltip("Enable touch detection for the associated characters")]
    public bool enableTouchDetection = true;

    [Header("Trigger Position")]
    [Tooltip("If true, the trigger position will follow the average position of all associated characters")]
    public bool useGroupAveragePosition = true;

    [Tooltip("If true, favor the position of characters that are interacting with objects")]
    public bool favorInteractingCharacters = true;

    [Tooltip("How much weight to give to interacting characters (1=normal, 2=double weight, etc.)")]
    [Range(1f, 5f)]
    public float interactingCharacterWeight = 2f;

    [Header("Dynamic Zoom Adjustment")]
    [Tooltip("If enabled, camera will automatically zoom out to keep all associated characters in view")]
    public bool keepAllCharactersInView = true;

    [Tooltip("The maximum zoom out multiplier (lower values = more zoomed out)")]
    [Range(0.1f, 1f)]
    public float maxZoomOutMultiplier = 1f;

    [Tooltip("How quickly the zoom adjusts to fit characters (higher = faster)")]
    [Range(0.1f, 10f)]
    public float zoomAdjustmentSpeed = 2f;

    [Tooltip("Padding around characters when calculating required zoom (higher = more space around edges)")]
    [Range(0.05f, 0.5f)]
    public float viewportPadding = 0.1f;

    // Add Card Management functionality
    [Header("Card Management")]
    [Tooltip("Whether to draw cards when zooming in")]
    public bool drawCardsOnZoom = true;

    [Tooltip("Number of cards to draw when zooming in")]
    public int cardsToDrawOnZoom = 3;

    [Tooltip("Whether to return cards to deck when zooming out")]
    public bool returnCardsToDeckOnZoomOut = true;

    [Tooltip("Speed multiplier for returning cards to deck (lower = faster)")]
    public float returnToDeckSpeed = 0.3f;

    private ProCamera2DTriggerZoom _triggerZoom;
    private ProCamera2D _proCamera2D;
    private bool _previousTriggerState = false;
    private bool _hasDrawnCards = false;
    private Vector3 _initialPosition;
    private float _originalZoomMultiplier;
    private float _currentDynamicMultiplier = 1f;
    private bool _isDynamicallyAdjustingZoom = false;

    // Dictionary to track characters in interaction state and their interaction status
    private Dictionary<GameObject, InteractionState> _interactingCharacters = new Dictionary<GameObject, InteractionState>();

    // Enum to track the exact state of interaction
    private enum InteractionState
    {
        MovingToObject,
        InteractingWithObject,
        ReturningFromObject
    }

    void Start()
    {
        // Get reference to the ProCamera2DTriggerZoom component on this GameObject
        _triggerZoom = GetComponent<ProCamera2DTriggerZoom>();

        if (_triggerZoom == null)
        {
            Debug.LogError("ProCamera2DTriggerZoomController: No ProCamera2DTriggerZoom component found on this GameObject!");
            enabled = false;
            return;
        }

        // Get reference to ProCamera2D
        _proCamera2D = ProCamera2D.Instance;
        if (_proCamera2D == null)
        {
            Debug.LogError("ProCamera2DTriggerZoomController: No ProCamera2D instance found!");
            enabled = false;
            return;
        }

        // Store initial position
        _initialPosition = transform.position;

        // Store the original zoom multiplier
        _originalZoomMultiplier = _triggerZoom.TargetZoom;

        // Initially disable the automatic trigger
        _triggerZoom.Toggle(false);

        // Set initial state
        _previousTriggerState = TriggerZoom;

        // If initially enabled, trigger the zoom
        if (TriggerZoom)
        {
            TriggerZoomIn();
        }

        _triggerZoom.OnExitedTrigger += HandleTriggerExit;
        _triggerZoom.OnEnteredTrigger += HandleTriggerEnter;
    }

    void Update()
    {
        // Check if the debug toggle has changed
        if (TriggerZoom != _previousTriggerState)
        {
            _previousTriggerState = TriggerZoom;

            if (TriggerZoom)
            {
                TriggerZoomIn();
            }
            else if (ResetWhenToggled)
            {
                Debug.Log("1 trying to return cards");
                TriggerZoomOut();
            }
        }

        // Update position to follow the average of associated characters if enabled
        if (useGroupAveragePosition && associatedCharacters.Count > 0)
        {
            UpdateTriggerPosition();
        }

        // Handle touch/click detection if enabled
        if (enableTouchDetection && associatedCharacters.Count > 0)
        {
            DetectCharacterTouch();
        }

        // Continuously check interaction state for each character
        if (favorInteractingCharacters)
        {
            UpdateInteractionStates();
        }

        // Check if we need to adjust zoom to keep all characters in view
        if (keepAllCharactersInView && TriggerZoom && associatedCharacters.Count > 0)
        {
            AdjustZoomToFitAllCharacters();
        }
    }

    private bool _isInsideTrigger = false;
    // Check if all characters are in view and adjust zoom if needed
    private void AdjustZoomToFitAllCharacters()
    {
        if (associatedCharacters.Count == 0 || _proCamera2D == null)
            return;

        // Get camera component from ProCamera2D
        Camera camera = _proCamera2D.GetComponent<Camera>();
        if (camera == null)
            return;

        // Check if any characters are outside the view
        bool allInView = true;
        Rect viewport = new Rect(0, 0, 1, 1);

        // Expand viewport rect by padding to give some margin
        viewport.xMin += viewportPadding;
        viewport.xMax -= viewportPadding;
        viewport.yMin += viewportPadding;
        viewport.yMax -= viewportPadding;

        // Calculate the bounds needed to contain all characters
        Bounds charactersBounds = new Bounds();
        bool boundsInitialized = false;

        foreach (GameObject character in associatedCharacters)
        {
            if (character == null) continue;

            // Get character position in viewport coordinates using the Camera component
            Vector3 viewportPos = camera.WorldToViewportPoint(character.transform.position);

            // Check if this character is outside the safe viewport area
            if (viewportPos.x < viewport.xMin || viewportPos.x > viewport.xMax ||
                viewportPos.y < viewport.yMin || viewportPos.y > viewport.yMax)
            {
                allInView = false;
            }

            // Update bounds to include this character
            if (!boundsInitialized)
            {
                charactersBounds = new Bounds(character.transform.position, Vector3.one);
                boundsInitialized = true;
            }
            else
            {
                charactersBounds.Encapsulate(character.transform.position);
            }
        }

        // If all characters are in view and we're not already at original zoom, gradually return to it
        if (allInView && _currentDynamicMultiplier < 1f)
        {
            _currentDynamicMultiplier = Mathf.MoveTowards(_currentDynamicMultiplier, 1f, Time.deltaTime * zoomAdjustmentSpeed * 0.5f);
            UpdateZoomMultiplier();
            return;
        }

        // If all characters are in view and we're at original zoom, nothing to do
        if (allInView)
            return;

        // Some characters are out of view, calculate required zoom to fit all
        if (boundsInitialized)
        {
            // Calculate the size needed to fit all characters
            float boundsWidth = charactersBounds.size.x + (charactersBounds.size.x * viewportPadding * 2);
            float boundsHeight = charactersBounds.size.y + (charactersBounds.size.y * viewportPadding * 2);

            // Get current camera size
            float cameraHeight = _proCamera2D.ScreenSizeInWorldCoordinates.y;
            float cameraWidth = _proCamera2D.ScreenSizeInWorldCoordinates.x;

            // Calculate zoom ratio needed on each axis
            float widthRatio = cameraWidth / boundsWidth;
            float heightRatio = cameraHeight / boundsHeight;

            // We need the smallest ratio to ensure everything fits
            float targetZoomRatio = Mathf.Min(widthRatio, heightRatio);

            // Clamp to our maximum allowed zoom out
            targetZoomRatio = Mathf.Clamp(targetZoomRatio, maxZoomOutMultiplier, 1f);

            // Smoothly adjust the current multiplier
            _currentDynamicMultiplier = Mathf.Lerp(_currentDynamicMultiplier, targetZoomRatio, Time.deltaTime * zoomAdjustmentSpeed);

            // Apply the zoom
            UpdateZoomMultiplier();
        }
    }


    // Update the zoom multiplier based on the current dynamic multiplier
    private void UpdateZoomMultiplier()
    {

        if (!_isDynamicallyAdjustingZoom)
        {
            // First time adjusting, save original
            _originalZoomMultiplier = _triggerZoom.TargetZoom;
            _isDynamicallyAdjustingZoom = true;
        }

        // Apply the dynamic multiplier to the original zoom
        float newZoomMultiplier = _originalZoomMultiplier * _currentDynamicMultiplier;

        // Apply the new zoom multiplier (this will make the camera zoom out)
        _triggerZoom.TargetZoom = newZoomMultiplier;

        // If we've changed zoom, we need to refresh the trigger
        if (_isInsideTrigger)
        {
            // This will apply the new zoom value
            _triggerZoom.TestTrigger();
        }
    }

    // Reset zoom to original value
    private void ResetZoomToOriginal()
    {
        if (_isDynamicallyAdjustingZoom)
        {
            _triggerZoom.TargetZoom = _originalZoomMultiplier;
            _currentDynamicMultiplier = 1f;
            _isDynamicallyAdjustingZoom = false;

            // Apply the change if we're inside the trigger
            //if (_triggerZoom.InsideTrigger)
            //{
                //_triggerZoom.TestTrigger();
           // }
        }
    }

    // Track the interaction state of all associated characters
    private void UpdateInteractionStates()
    {
        foreach (GameObject character in associatedCharacters)
        {
            if (character == null) continue;

            AgeOfHeroes.NPCInteractionMovement movementController =
                character.GetComponent<AgeOfHeroes.NPCInteractionMovement>();

            AgeOfHeroes.CharacterInteractionController interactionController =
                character.GetComponent<AgeOfHeroes.CharacterInteractionController>();

            if (movementController != null)
            {
                // Check if character is actively moving (to object)
                if (movementController.IsMoving)
                {
                    // Check if we need to add this character to our tracking
                    if (!_interactingCharacters.ContainsKey(character))
                    {
                        _interactingCharacters.Add(character, InteractionState.MovingToObject);
                    }

                    // Check if this is the return journey
                    bool isReturning = IsCharacterReturning(movementController);
                    if (isReturning)
                    {
                        _interactingCharacters[character] = InteractionState.ReturningFromObject;
                    }
                    else
                    {
                        _interactingCharacters[character] = InteractionState.MovingToObject;
                    }
                }
                else
                {
                    // Not moving - check if this character is in our tracking
                    if (_interactingCharacters.ContainsKey(character))
                    {
                        // Check if we can detect that they're in the interaction phase
                        // (between MovingToObject and ReturningFromObject)
                        if (_interactingCharacters[character] == InteractionState.MovingToObject)
                        {
                            // Character was moving to object but now stopped - they must be interacting
                            _interactingCharacters[character] = InteractionState.InteractingWithObject;
                        }
                        else if (_interactingCharacters[character] == InteractionState.ReturningFromObject
                                 && !movementController.IsMoving)
                        {
                            // Character finished return journey - remove from tracking
                            _interactingCharacters.Remove(character);
                        }
                    }
                    // We can also check if this character has an active interaction routine
                    else if (interactionController != null)
                    {
                        // Use reflection to check for active interaction
                        var activeRoutineField = typeof(AgeOfHeroes.CharacterInteractionController).GetField(
                            "activeInteractionRoutine",
                            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                        if (activeRoutineField != null)
                        {
                            var activeRoutine = activeRoutineField.GetValue(interactionController);
                            if (activeRoutine != null)
                            {
                                // Character has an active interaction but isn't moving
                                // This happens during the waiting phase
                                _interactingCharacters[character] = InteractionState.InteractingWithObject;
                            }
                        }
                    }
                }
            }
        }
    }

    // Helper method to determine if a character is in the "return" phase of their interaction
    private bool IsCharacterReturning(AgeOfHeroes.NPCInteractionMovement movementController)
    {
        // Get the private fields we need through reflection
        System.Reflection.FieldInfo isReturningField = typeof(AgeOfHeroes.NPCInteractionMovement).GetField(
            "isReturning", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // If we have direct access to isReturning field, use that
        if (isReturningField != null)
        {
            bool isReturning = (bool)isReturningField.GetValue(movementController);
            return isReturning;
        }

        // Otherwise try to determine it by comparing target to original position
        System.Reflection.FieldInfo originalPosField = typeof(AgeOfHeroes.NPCInteractionMovement).GetField(
            "originalPosition", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        System.Reflection.FieldInfo targetPosField = typeof(AgeOfHeroes.NPCInteractionMovement).GetField(
            "targetPosition", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (originalPosField != null && targetPosField != null)
        {
            Vector3 originalPos = (Vector3)originalPosField.GetValue(movementController);
            Vector3 targetPos = (Vector3)targetPosField.GetValue(movementController);

            // If target is very close to original, they're likely returning
            float distance = Vector3.Distance(targetPos, originalPos);
            return distance < 1f;
        }

        // Fallback strategy - assume they're in the interesting phase
        return false;
    }

    // Calculate and update the trigger position based on the average of all associated characters
    // with special weighting for characters that are interacting with objects
    private void UpdateTriggerPosition()
    {
        Vector3 weightedSum = Vector3.zero;
        float totalWeight = 0f;

        foreach (GameObject character in associatedCharacters)
        {
            if (character != null)
            {
                float characterWeight = 1f;

                // Check if this character is in an interaction sequence
                if (favorInteractingCharacters && _interactingCharacters.ContainsKey(character))
                {
                    InteractionState state = _interactingCharacters[character];

                    // Apply weight for all interaction states, including the static interaction phase
                    switch (state)
                    {
                        case InteractionState.MovingToObject:
                            characterWeight = interactingCharacterWeight;
                            break;

                        case InteractionState.InteractingWithObject:
                            // Critical: During the actual interaction, give full weight
                            // This ensures we don't reset during the waiting period
                            characterWeight = interactingCharacterWeight;
                            break;

                        case InteractionState.ReturningFromObject:
                            characterWeight = interactingCharacterWeight;
                            break;
                    }
                }

                // Apply weighted position
                weightedSum += character.transform.position * characterWeight;
                totalWeight += characterWeight;
            }
        }

        if (totalWeight > 0)
        {
            Vector3 averagePosition = weightedSum / totalWeight;
            // Keep the same Z position as original
            averagePosition.z = _initialPosition.z;
            // Update the trigger position
            transform.position = averagePosition;
        }
    }

    private void DetectCharacterTouch()
    {
        if (Input.GetMouseButtonDown(0)) // 0 is left mouse button/touch
        {
            // Cast a ray from the camera to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            // Check if we hit any of our associated characters
            if (hit.collider != null)
            {
                foreach (GameObject character in associatedCharacters)
                {
                    if (character != null && hit.collider.gameObject == character)
                    {
                        TriggerZoomIn();
                        Debug.Log("Zoom triggered by character touch: " + character.name);
                        break;
                    }
                }
            }
        }
    }
    private void HandleTriggerEnter()
    {
        _isInsideTrigger = true;
    }

    private void HandleTriggerExit()
    {
        _triggerZoom.Toggle(false);
        _isInsideTrigger = false; // Mark that we're no longer inside the trigger

        Debug.Log("trigger exit");

        // Return cards to deck when exiting the trigger
        if (returnCardsToDeckOnZoomOut && _hasDrawnCards)
        {
            ReturnCardsToDeck();
        }

        // Reset the card drawing flag
        _hasDrawnCards = false;

        // Reset any dynamic zoom adjustments
        ResetZoomToOriginal();
    }

    /// <summary>
    /// Programmatically activates the zoom effect
    /// </summary>
    public void TriggerZoomIn()
    {
        // Force test to enter the trigger
        TriggerZoom = true;

        _triggerZoom.Toggle(true);
        _isInsideTrigger = true; // Mark that we're inside the trigger

        // Reset any previous dynamic zoom first
        if (_isDynamicallyAdjustingZoom)
        {
            ResetZoomToOriginal();
        }

        _triggerZoom.TestTrigger();

        // Draw cards if enabled and not already drawn
        if (drawCardsOnZoom && !_hasDrawnCards)
        {
            DrawCards();
        }
    }

    /// <summary>
    /// Programmatically deactivates the zoom effect (if ResetSizeOnExit is true in the trigger)
    /// </summary>
    public void TriggerZoomOut()
    {
        // If ResetSizeOnExit is true, this will automatically reset the zoom
        _triggerZoom.Toggle(false);
        _isInsideTrigger = false; // Mark that we're no longer inside the trigger

        // Reset any dynamic zoom adjustments
        ResetZoomToOriginal();

        // Return cards to deck when zooming out
        if (returnCardsToDeckOnZoomOut && _hasDrawnCards)
        {
            ReturnCardsToDeck();
            Debug.Log("trying to return cards");
        }

        // Reset flag to allow drawing cards again next time
        _hasDrawnCards = false;
    }

    /// <summary>
    /// Set whether the trigger should follow the average position of associated characters
    /// </summary>
    public void SetUseGroupAveragePosition(bool useAverage)
    {
        useGroupAveragePosition = useAverage;

        // Reset to initial position if turning off group average position
        if (!useAverage)
        {
            transform.position = _initialPosition;
        }
    }

    /// <summary>
    /// Set whether to favor characters that are interacting with objects
    /// </summary>
    public void SetFavorInteractingCharacters(bool favorInteracting)
    {
        favorInteractingCharacters = favorInteracting;

        // Clear the tracking dictionary if turning off this feature
        if (!favorInteracting)
        {
            _interactingCharacters.Clear();
        }
    }

    /// <summary>
    /// Set the weight multiplier for interacting characters
    /// </summary>
    public void SetInteractingCharacterWeight(float weight)
    {
        interactingCharacterWeight = Mathf.Clamp(weight, 1f, 5f);
    }

    /// <summary>
    /// Enable or disable the feature to keep all characters in view
    /// </summary>
    public void SetKeepAllCharactersInView(bool keepInView)
    {
        keepAllCharactersInView = keepInView;

        // Reset any dynamic zoom adjustments if turning off
        if (!keepInView && _isDynamicallyAdjustingZoom)
        {
            ResetZoomToOriginal();
        }
    }

    /// <summary>
    /// Set the maximum zoom out multiplier (how far the camera can zoom out)
    /// </summary>
    /// <summary>
    /// Set the maximum zoom out multiplier (how far the camera can zoom out)
    /// </summary>
    public void SetMaxZoomOutMultiplier(float multiplier)
    {
        maxZoomOutMultiplier = Mathf.Clamp(multiplier, 0.1f, 1f);

        // If we're currently zoomed out more than the new maximum, adjust immediately
        if (_currentDynamicMultiplier < maxZoomOutMultiplier)
        {
            _currentDynamicMultiplier = maxZoomOutMultiplier;
            UpdateZoomMultiplier();
        }
    }

    /// <summary>
    /// Set the zoom adjustment speed
    /// </summary>
    public void SetZoomAdjustmentSpeed(float speed)
    {
        zoomAdjustmentSpeed = Mathf.Clamp(speed, 0.1f, 10f);
    }

    /// <summary>
    /// Set the viewport padding amount
    /// </summary>
    public void SetViewportPadding(float padding)
    {
        viewportPadding = Mathf.Clamp(padding, 0.05f, 0.5f);
    }

    /// <summary>
    /// Add a character to the associated characters list
    /// </summary>
    public void AddAssociatedCharacter(GameObject character)
    {
        if (character != null && !associatedCharacters.Contains(character))
        {
            associatedCharacters.Add(character);
        }
    }

    /// <summary>
    /// Remove a character from the associated characters list
    /// </summary>
    public void RemoveAssociatedCharacter(GameObject character)
    {
        if (character != null && associatedCharacters.Contains(character))
        {
            associatedCharacters.Remove(character);

            // Also remove from interacting characters if present
            if (_interactingCharacters.ContainsKey(character))
            {
                _interactingCharacters.Remove(character);
            }
        }
    }

    /// <summary>
    /// Clear all associated characters
    /// </summary>
    public void ClearAssociatedCharacters()
    {
        associatedCharacters.Clear();
        _interactingCharacters.Clear();
    }

    // Helper method to draw cards using DeckManager
    private void DrawCards()
    {
        if (CardSystem.DeckManager.Instance != null)
        {
            CardSystem.DeckManager.Instance.DrawCards(cardsToDrawOnZoom);
            _hasDrawnCards = true;
            Debug.Log($"Drew {cardsToDrawOnZoom} cards from zoom trigger");
        }
        else
        {
            Debug.LogError("DeckManager instance not found!");
        }
    }

    // Helper method to return cards to deck
    private void ReturnCardsToDeck()
    {
        if (CardSystem.DeckManager.Instance != null)
        {
            CardSystem.DeckManager.Instance.ReturnAllCardsToDeck(returnToDeckSpeed);
            Debug.Log("Returned cards to deck from zoom trigger");
        }
        else
        {
            Debug.LogError("DeckManager instance not found!");
        }
    }

    // OnDestroy cleanup
    private void OnDestroy()
    {
        // Unsubscribe from events
        if (_triggerZoom != null)
        {
            _triggerZoom.OnExitedTrigger -= HandleTriggerExit;
            _triggerZoom.OnEnteredTrigger -= HandleTriggerEnter;
        }
    }
}
