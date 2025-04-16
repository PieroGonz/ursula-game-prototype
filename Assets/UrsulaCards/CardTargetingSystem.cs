using System.Collections.Generic;
using UnityEngine;

public class CardTargetingSystem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private float maxTargetingDistance = 20f;
    [SerializeField] private Material targetHighlightMaterial;

    [Header("Visual Feedback")]
    [SerializeField] private Color targetMarkerColor = Color.red;
    [SerializeField] private float highlightIntensity = 1.25f;

    // Internal references
    private GameObject selectedCard;
    private List<GameObject> validTargets = new List<GameObject>();
    private Dictionary<GameObject, Material[]> originalMaterials = new Dictionary<GameObject, Material[]>();

    // Static instance for easy access
    public static CardTargetingSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Call this when a card is selected
    public void OnCardSelected(GameObject card)
    {
        Debug.Log($"Card selected: {card.name}");

        // Clear previous targets
        ClearTargets();

        selectedCard = card;

        // Find targets for any card (customize this logic based on your needs)
        FindValidTargets();
        HighlightValidTargets();
    }

    // Call this when a card is deselected
    public void OnCardDeselected()
    {
        ClearTargets();
        selectedCard = null;
    }

    // Find all valid enemy targets in the scene
    private void FindValidTargets()
    {
        validTargets.Clear();

        // Find all game objects with the Enemy tag
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        foreach (GameObject enemy in enemies)
        {
            // Check if enemy is within range
            if (Vector3.Distance(transform.position, enemy.transform.position) <= maxTargetingDistance)
            {
                validTargets.Add(enemy);
                Debug.Log($"Found valid target: {enemy.name}");
            }
        }
    }

    // Highlight all valid targets
    private void HighlightValidTargets()
    {
        foreach (GameObject target in validTargets)
        {
            // Store original materials for later restoration
            Renderer[] renderers = target.GetComponentsInChildren<Renderer>();
            List<Material> originalMats = new List<Material>();

            foreach (Renderer renderer in renderers)
            {
                originalMats.AddRange(renderer.materials);

                // Apply highlight material or outline
                Material[] highlightMaterials = new Material[renderer.materials.Length];
                for (int i = 0; i < highlightMaterials.Length; i++)
                {
                    if (targetHighlightMaterial != null)
                    {
                        highlightMaterials[i] = targetHighlightMaterial;
                    }
                    else
                    {
                        highlightMaterials[i] = renderer.materials[i];
                        // Change color to indicate target
                        highlightMaterials[i].color = targetMarkerColor * highlightIntensity;
                    }
                }
                renderer.materials = highlightMaterials;
            }

            originalMaterials[target] = originalMats.ToArray();
        }
    }

    // Clear all target highlights
    private void ClearTargets()
    {
        foreach (GameObject target in validTargets)
        {
            // Restore original materials
            if (originalMaterials.TryGetValue(target, out Material[] materials))
            {
                Renderer[] renderers = target.GetComponentsInChildren<Renderer>();
                int materialIndex = 0;

                foreach (Renderer renderer in renderers)
                {
                    Material[] targetMats = new Material[renderer.materials.Length];
                    for (int i = 0; i < targetMats.Length; i++)
                    {
                        targetMats[i] = materials[materialIndex++];
                    }
                    renderer.materials = targetMats;
                }

                originalMaterials.Remove(target);
            }
        }

        validTargets.Clear();
    }

    // Apply card effect to a target
    public void ApplyCardToTarget(GameObject target)
    {
        if (!validTargets.Contains(target) || selectedCard == null)
            return;

        Debug.Log($"Applying card {selectedCard.name} effect to enemy {target.name}");

        // Get the enemy's health component - use your actual health component type here
        // This is a generic approach, adapt to your actual health component
        var health = target.GetComponent<MonoBehaviour>();

        if (health != null)
        {
            // Simple damage application (visual only for now)
            int damage = 10;

            // Visual feedback
            ShowDamageEffect(target, damage);

            // Here you would call your actual damage method
            // Example: health.TakeDamage(damage);
        }

        // Clear targeting
        ClearTargets();
        selectedCard = null;
    }

    // Show damage effect on the target
    private void ShowDamageEffect(GameObject target, int amount)
    {
        // Create a floating text effect
        GameObject textEffect = new GameObject("DamageText");
        textEffect.transform.position = target.transform.position + Vector3.up;

        // Add a TextMesh component
        TextMesh textMesh = textEffect.AddComponent<TextMesh>();
        textMesh.text = amount.ToString();
        textMesh.color = Color.red;
        textMesh.characterSize = 0.1f;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;
        textMesh.fontSize = 42;

        // Destroy after a short duration
        Destroy(textEffect, 1.5f);
    }
}
