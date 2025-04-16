using System.Collections.Generic;
using UnityEngine;

public class QuickBarManager : MonoBehaviour
{
    [Header("Quickbar References")]
    public string quickbarName = "QuickBarMenu";

    [Header("Item Database")]
    public List<ItemScriptableObject> availableItems = new List<ItemScriptableObject>();

    // Dictionary to track items currently in the player's inventory
    private Dictionary<string, int> playerItems = new Dictionary<string, int>();

    void Start()
    {
        // Make sure the quickbar is empty at start
        UltimateMobileQuickbar.ClearQuickbar(quickbarName);

        // Add some sample items to the player's inventory
        // This is for testing - in a real game, items would be earned through gameplay
        foreach (var item in availableItems)
        {
            AddItemToInventory(item, Random.Range(1, 4));
        }
    }

    // Adds an item to the player's inventory
    public void AddItemToInventory(ItemScriptableObject item, int count = 1)
    {
        if (item == null) return;

        // Add to dictionary or update count
        if (playerItems.ContainsKey(item.itemName))
        {
            playerItems[item.itemName] += count;
        }
        else
        {
            playerItems.Add(item.itemName, count);

            // If this is a new item and it can be used in the quickbar, add it
            if (item.canUseInQuickbar)
            {
                RegisterItemToQuickbar(item);
            }
        }

        // Update the count display on the quickbar
        if (item.canUseInQuickbar && item.quickbarButtonInfo.ExistsOnQuickbar())
        {
            item.quickbarButtonInfo.UpdateCount(playerItems[item.itemName]);
        }

        Debug.Log($"Added {count} {item.itemName}(s) to inventory. Total: {playerItems[item.itemName]}");
    }

    // Consumes an item from inventory (reduces count)
    public bool ConsumeItem(ItemScriptableObject item, int count = 1)
    {
        if (item == null) return false;

        // Check if we have enough of this item
        if (!playerItems.ContainsKey(item.itemName) || playerItems[item.itemName] < count)
        {
            Debug.Log($"Not enough {item.itemName} in inventory!");
            return false;
        }

        // Reduce the item count
        playerItems[item.itemName] -= count;

        // Update the count display on the quickbar
        if (item.canUseInQuickbar && item.quickbarButtonInfo.ExistsOnQuickbar())
        {
            item.quickbarButtonInfo.UpdateCount(playerItems[item.itemName]);
        }

        // If count reaches zero, remove from inventory
        if (playerItems[item.itemName] <= 0)
        {
            playerItems.Remove(item.itemName);

            // Remove from quickbar if it exists there
            if (item.canUseInQuickbar && item.quickbarButtonInfo.ExistsOnQuickbar())
            {
                item.quickbarButtonInfo.RemoveFromQuickbar();
            }
        }

        Debug.Log($"Used {count} {item.itemName}(s). Remaining: {(playerItems.ContainsKey(item.itemName) ? playerItems[item.itemName] : 0)}");
        return true;
    }

    // Registers an item to the quickbar
    private void RegisterItemToQuickbar(ItemScriptableObject item)
    {
        if (item == null || !item.canUseInQuickbar) return;

        // Set up the button info
        item.quickbarButtonInfo.key = item.itemName;
        item.quickbarButtonInfo.icon = item.itemIcon;

        // Register the item to the quickbar with a string callback
        UltimateMobileQuickbar.RegisterToQuickbar(quickbarName, UseItemByName, item.quickbarButtonInfo);

        // Update the count display
        if (playerItems.ContainsKey(item.itemName))
        {
            item.quickbarButtonInfo.UpdateCount(playerItems[item.itemName]);
        }
    }

    // This is the callback function that will be called when a quickbar button is pressed
    private void UseItemByName(string itemName)
    {
        // Find the item in our available items list
        ItemScriptableObject itemToUse = availableItems.Find(item => item.itemName == itemName);

        if (itemToUse != null)
        {
            // First make sure we have the item and can consume it
            if (ConsumeItem(itemToUse))
            {
                // Call the item's use function
                itemToUse.UseItem();
            }
        }
        else
        {
            Debug.LogWarning($"Item named {itemName} was not found in available items!");
        }
    }

    // Helper method to get an item by name
    public ItemScriptableObject GetItemByName(string itemName)
    {
        return availableItems.Find(item => item.itemName == itemName);
    }

    // Helper method to get current item count
    public int GetItemCount(string itemName)
    {
        if (playerItems.ContainsKey(itemName))
            return playerItems[itemName];
        return 0;
    }
}
