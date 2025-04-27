using UnityEngine;
using System.Collections.Generic;

public class InteractableObject : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionRadius = 2f;
    public string interactionAnimation = "Open";  // Default animation for chest objects
    public float interactionDuration = 3f;
    public bool canInteractMultipleTimes = false;

    [Header("NPC Interaction Settings")]
    public float interestValue = 1f;  // Higher values make this more interesting to NPCs
    public List<string> interactableByTags = new List<string>() { "NPC", "Player", "Pawn" };
    public float interactionCooldown = 10f;  // Time before NPCs will interact again

    [Header("Interaction Types")]
    public bool isPickable = false;
    public bool isOpenable = true;   // For chest-like objects
    public bool isUsable = false;

    [Header("Visual Feedback")]
    public GameObject highlightEffect;
    public Color highlightColor = Color.yellow;
    public bool showInteractionPrompt = true;

    // Internal state tracking
    private bool isHighlighted = false;
    private bool hasBeenInteractedWith = false;
    private float lastInteractionTime = -999f;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // For detecting if object is within interaction range
    public bool IsInRange(Vector3 position)
    {
        return Vector3.Distance(transform.position, position) <= interactionRadius;
    }

    // Check if this character can interact with this object
    public bool CanCharacterInteract(GameObject character)
    {
        // Don't allow interaction if cooldown hasn't elapsed
        if (Time.time < lastInteractionTime + interactionCooldown)
            return false;

        // Don't allow interaction if it's been used and doesn't allow multiple interactions
        if (hasBeenInteractedWith && !canInteractMultipleTimes)
            return false;

        // Check if the character has an allowed tag
        if (character != null && interactableByTags.Count > 0)
        {
            string characterTag = character.tag;
            return interactableByTags.Contains(characterTag);
        }

        // Default to allowing interaction if no specific tags are set
        return interactableByTags.Count == 0;
    }

    // Visual feedback when object is interactable
    public void SetHighlight(bool active)
    {
        if (isHighlighted == active) return;

        isHighlighted = active;

        if (highlightEffect != null)
        {
            highlightEffect.SetActive(active);
        }
        else
        {
            // Apply highlight to sprite renderer if no specific effect is set
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                if (active)
                {
                    // Store original color in material property
                    renderer.material.SetColor("_OriginalColor", renderer.color);
                    renderer.color = highlightColor;
                }
                else
                {
                    // Restore original color if it was saved
                    if (renderer.material.HasProperty("_OriginalColor"))
                    {
                        renderer.color = renderer.material.GetColor("_OriginalColor");
                    }
                }
            }
        }
    }

    // Called when a character interacts with this object
    public void Interact(GameObject character)
    {
        // Set the interaction time
        lastInteractionTime = Time.time;
        hasBeenInteractedWith = true;

        // Trigger appropriate animation based on interaction type
        if (animator != null)
        {
            if (isOpenable && !string.IsNullOrEmpty(interactionAnimation))
            {
                animator.SetTrigger(interactionAnimation);
            }
        }

        // Custom interaction logic based on type
        if (isPickable)
        {
            // Logic for picking up the object
            // Could notify an inventory system, etc.
            Debug.Log($"{character.name} picked up {gameObject.name}");
        }
        else if (isUsable)
        {
            // Logic for using the object
            Debug.Log($"{character.name} used {gameObject.name}");
        }
        else if (isOpenable)
        {
            // Logic for opening the object
            Debug.Log($"{character.name} opened {gameObject.name}");
        }

        // Broadcast to any listeners that this object was interacted with
        OnObjectInteracted?.Invoke(character);
    }

    // Event system for notifying other components of interaction
    public delegate void ObjectInteractedEvent(GameObject interactor);
    public event ObjectInteractedEvent OnObjectInteracted;

    // Get the visual radius for interaction (useful for visualizing in the editor)
    public float GetInteractionRadius()
    {
        return interactionRadius;
    }

    // Reset interaction state (for example, if a chest closes again)
    public void ResetInteractionState()
    {
        hasBeenInteractedWith = false;
    }

    // Optional: Visual debugging in the editor
    private void OnDrawGizmosSelected()
    {
        // Draw the interaction radius
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }

    // Optional: For objects that want to use trigger colliders for interaction
    private void OnTriggerEnter2D(Collider2D other)
    {
        // This can be used if you want objects to automatically highlight
        // when the player enters their trigger area
        if (other.CompareTag("Player"))
        {
            SetHighlight(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SetHighlight(false);
        }
    }
}
