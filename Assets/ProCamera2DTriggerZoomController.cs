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
    private bool _previousTriggerState = false;
    private bool _hasDrawnCards = false;

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

        // _triggerZoom.OnExi += HandleTriggerExit;
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

        // Handle touch/click detection if enabled
        if (enableTouchDetection && associatedCharacters.Count > 0)
        {
            DetectCharacterTouch();
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

    private void HandleTriggerExit()
    {
        _triggerZoom.Toggle(false);

        Debug.Log("trigger exit");

        // Return cards to deck when exiting the trigger
        if (returnCardsToDeckOnZoomOut && _hasDrawnCards)
        {

            ReturnCardsToDeck();
        }

        // Reset the card drawing flag
        _hasDrawnCards = false;
    }

    /// <summary>
    /// Programmatically activates the zoom effect
    /// </summary>
    public void TriggerZoomIn()
    {
        // Force test to enter the trigger
        TriggerZoom = true;

        _triggerZoom.Toggle(true);

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

        // Return cards to deck when zooming out
        if (returnCardsToDeckOnZoomOut && _hasDrawnCards)
        {
            ReturnCardsToDeck();
            Debug.Log("trying to return cards");
        }

        // Reset flag to allow drawing cards again next time
        _hasDrawnCards = false;
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
}
