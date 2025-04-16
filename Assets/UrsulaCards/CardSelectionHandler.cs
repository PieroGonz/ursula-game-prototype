using UnityEngine;
using UnityEngine.EventSystems;

public class CardSelectionHandler : MonoBehaviour, IPointerClickHandler
{
    private bool isSelected = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isSelected)
        {
            SelectCard();
        }
        else
        {
            DeselectCard();
        }
    }

    private void SelectCard()
    {
        isSelected = true;
        Debug.Log("Selected Card");

        // Call your card's selection method if it exists
        // For example: GetComponent<Card>().Selected(true);

        // Use visual feedback for selection
        transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

        // Notify the targeting system
        if (CardTargetingSystem.Instance != null)
        {
            CardTargetingSystem.Instance.OnCardSelected(gameObject);
        }
    }

    public void DeselectCard()
    {
        isSelected = false;

        // Call your card's deselection method if it exists
        // For example: GetComponent<Card>().Selected(false);

        // Reset visual feedback
        transform.localScale = Vector3.one;

        // Notify the targeting system
        if (CardTargetingSystem.Instance != null)
        {
            CardTargetingSystem.Instance.OnCardDeselected();
        }
    }
}
