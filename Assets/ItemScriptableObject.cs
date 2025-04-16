using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemScriptableObject : ScriptableObject
{
    [Header("Basic Item Information")]
    public string itemName;
    public string itemDescription;
    public Sprite itemIcon;
    public int maxStackSize = 99;

    [Header("Quickbar Settings")]
    public bool canUseInQuickbar = true;
    public UltimateMobileQuickbarButtonInfo quickbarButtonInfo;

    public enum ItemType { Consumable, Weapon, Armor, Material, Quest, Tool }
    public ItemType itemType = ItemType.Consumable;

    [Header("Item Stats (Optional)")]
    public int itemValue = 0;      // Gold value or economic value
    public int powerLevel = 0;     // For weapons/armor rating

    // Initialization when instance is created
    private void OnEnable()
    {
        // Create a UltimateMobileQuickbarButtonInfo if not already assigned
        if (quickbarButtonInfo == null)
        {
            quickbarButtonInfo = new UltimateMobileQuickbarButtonInfo();
        }

        // Set up the basic button info based on this item
        quickbarButtonInfo.key = itemName;
        quickbarButtonInfo.icon = itemIcon;
    }

    // Method to be called when item is used from the quickbar
    public virtual void UseItem()
    {
        Debug.Log($"Using item: {itemName}");
        // Override this in derived classes for specific item functionality
    }
}
