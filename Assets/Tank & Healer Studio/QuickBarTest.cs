using UnityEngine;
using System.Collections;

public class QuickBarTest : MonoBehaviour
{
    private QuickBarManager quickBarManager;

    void Start()
    {
        quickBarManager = GetComponent<QuickBarManager>();

        // Wait a short time to ensure everything is initialized
        StartCoroutine(AddStartingItems());

        UltimateMobileQuickbar.DisableQuickbar("QuickBarMenu");
    }

    IEnumerator AddStartingItems()
    {
        yield return new WaitForSeconds(0.5f);

        // Add some items for testing (normally these would be acquired through gameplay)
        foreach (var item in quickBarManager.availableItems)
        {
            quickBarManager.AddItemToInventory(item, Random.Range(1, 4));
        }

        Debug.Log("Starting items added to quickbar");
    }

    // You can add test buttons in the UI to call these methods

    public void AddRandomItem()
    {
        if (quickBarManager.availableItems.Count > 0)
        {
            var randomItem = quickBarManager.availableItems[Random.Range(0, quickBarManager.availableItems.Count)];
            quickBarManager.AddItemToInventory(randomItem);
        }
    }

    public void TestUseFirstItem()
    {
        if (quickBarManager.availableItems.Count > 0)
        {
            var firstItem = quickBarManager.availableItems[0];
            if (quickBarManager.GetItemCount(firstItem.itemName) > 0)
            {
                quickBarManager.ConsumeItem(firstItem);
                firstItem.UseItem();
            }
            else
            {
                Debug.Log($"No {firstItem.itemName} available");
            }
        }
    }
}
