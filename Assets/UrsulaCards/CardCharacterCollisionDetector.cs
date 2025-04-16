using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Lean.Common;
using Lean.Touch;

public class CardCharacterCollisionDetector : MonoBehaviour
{
    [Tooltip("Tag of the characters to detect collisions with")]
    [SerializeField] private string characterTag = "Pawn";

    [Tooltip("How often to check for collisions (in seconds)")]
    [SerializeField] private float checkInterval = 0.1f;

    [Tooltip("Distance threshold for considering a collision")]
    [SerializeField] private float collisionDistance = 30f;

    // Cached components
    private RectTransform rectTransform;
    private Canvas parentCanvas;
    private Camera mainCamera;
    private LeanSelectableByFinger leanSelectable;

    // State tracking
    private float nextCheckTime;
    private bool isDragging = false;
    private List<GameObject> collidingCharacters = new List<GameObject>();

    private void Awake()
    {
        // Get required components
        rectTransform = GetComponent<RectTransform>();

        // Find parent canvas - more reliable approach
        parentCanvas = GetComponentInParent<Canvas>();

        if (parentCanvas == null)
        {
            Debug.LogError("CardCharacterCollisionDetector: No Canvas found in parents!", this);
        }

        // Find camera - more reliable approach with fallback
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("CardCharacterCollisionDetector: No main camera found!", this);
            // Try to find any camera as fallback
            mainCamera = FindObjectOfType<Camera>();
        }

        // Get the lean selectable component
        leanSelectable = GetComponent<Lean.Touch.LeanSelectableByFinger>();
    }

    void OnEnable()
    {
        if (leanSelectable != null)
        {
            leanSelectable.OnSelected.AddListener(OnCardSelected);
            leanSelectable.OnDeselected.AddListener(OnCardDeselected);
        }
    }

    void OnDisable()
    {
        if (leanSelectable != null)
        {
            leanSelectable.OnSelected.RemoveListener(OnCardSelected);
            leanSelectable.OnDeselected.RemoveListener(OnCardDeselected);
        }
    }

    private void OnCardSelected(LeanSelect select)
    {
        isDragging = true;

        parentCanvas = GetComponentInParent<Canvas>();

        Debug.Log("dragging");
    }

    private void OnCardDeselected(LeanSelect select)
    {
        isDragging = false;

        // Handle any finalization with characters we were colliding with
        if (collidingCharacters.Count > 0)
        {
            foreach (GameObject character in collidingCharacters)
            {
                // You can trigger specific card-placement logic here
                Debug.Log($"Card applied to character: {character.name}");

                if (CardSystem.DeckManager.Instance != null)
                {
                    CardSystem.DeckManager.Instance.PlayCard(GetComponent<CardSystem.Card>());
                    Debug.Log("Returned cards to deck from zoom trigger");
                }

                // Example: Get the character component and call a method
            }

            collidingCharacters.Clear();
        }
    }

    void Update()
    {
        if (!isDragging || Time.time < nextCheckTime)
            return;

        nextCheckTime = Time.time + checkInterval;
        CheckForCharacterCollisions();
    }

    private void CheckForCharacterCollisions()
    {
        // Clear previous collisions
        collidingCharacters.Clear();

        // Convert UI position to world position
        Vector3 cardWorldPos = GetWorldPosition(transform.position);

        // Find all pawns
        GameObject[] pawns = GameObject.FindGameObjectsWithTag(characterTag);

        foreach (GameObject pawn in pawns)
        {
            // Get distance between card and character
            float distance = Vector2.Distance(new Vector2(cardWorldPos.x, cardWorldPos.y),
                                             new Vector2(pawn.transform.position.x, pawn.transform.position.y));

            if (distance <= collisionDistance)
            {
                collidingCharacters.Add(pawn);
                Debug.Log($"Card is colliding with: {pawn.name}");

                // You can provide visual feedback or trigger immediate effects here
            }
        }
    }

    private Vector3 GetWorldPosition(Vector3 screenPosition)
    {
        // Check for null references first
        if (mainCamera == null)
        {
            Debug.LogError("CardCharacterCollisionDetector: mainCamera is null!", this);
            return Vector3.zero;
        }

        if (parentCanvas == null)
        {
            Debug.LogError("CardCharacterCollisionDetector: parentCanvas is null!", this);
            return Vector3.zero;
        }

        if (parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            // For Screen Space - Overlay
            return mainCamera.ScreenToWorldPoint(new Vector3(
                screenPosition.x,
                screenPosition.y,
                mainCamera.nearClipPlane + 10)); // Adjust z-depth as needed
        }
        else
        {
            // For Screen Space - Camera or World Space
            Vector3 worldPosition;

            // Get the canvas rect transform with null check
            RectTransform canvasRectTransform = parentCanvas.GetComponent<RectTransform>();
            if (canvasRectTransform == null)
            {
                Debug.LogError("CardCharacterCollisionDetector: Canvas has no RectTransform!", this);
                return Vector3.zero;
            }

            // Use the canvas worldCamera or fallback to main camera
            Camera worldCamera = parentCanvas.worldCamera != null ? parentCanvas.worldCamera : mainCamera;

            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
                canvasRectTransform,
                screenPosition,
                worldCamera,
                out worldPosition))
            {
                return worldPosition;
            }
            else
            {
                Debug.LogWarning("CardCharacterCollisionDetector: Failed to convert screen position to world position", this);
                return Vector3.zero;
            }
        }
    }

}
