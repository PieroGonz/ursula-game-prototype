// CardDrawerBridge.cs - Place this in your project's scripts folder, NOT in the ProCamera2D folder
using UnityEngine;
using CardSystem;
using Com.LuisPedroFonseca.ProCamera2D; // You can reference ProCamera2D from your scripts

public class CardDrawerBridge : MonoBehaviour
{
    [Tooltip("The ProCamera2DTriggerZoom component to listen to")]
    public ProCamera2DTriggerZoom triggerZoom;

    [Tooltip("Number of cards to draw when zooming in")]
    public int cardsToDrawOnZoom = 3;

    private bool hasDrawnCards = false;

    private void OnEnable()
    {
        if (triggerZoom == null)
        {
            triggerZoom = GetComponent<ProCamera2DTriggerZoom>();
        }

        if (triggerZoom != null)
        {
            // Subscribe to zoom events
            triggerZoom.OnEnteredTrigger += HandleZoomIn;
            triggerZoom.OnExitedTrigger += HandleZoomOut;
        }
        else
        {
            Debug.LogError("CardDrawerBridge: No ProCamera2DTriggerZoom component found!");
        }
    }

    private void OnDisable()
    {
        if (triggerZoom != null)
        {
            // Unsubscribe from zoom events
            triggerZoom.OnEnteredTrigger -= HandleZoomIn;
            triggerZoom.OnExitedTrigger -= HandleZoomOut;
        }
    }

    private void HandleZoomIn()
    {
        if (!hasDrawnCards && DeckManager.Instance != null)
        {
            DeckManager.Instance.DrawCards(cardsToDrawOnZoom);
            hasDrawnCards = true;
            Debug.Log($"Drew {cardsToDrawOnZoom} cards from zoom trigger");
        }
    }

    private void HandleZoomOut()
    {
        hasDrawnCards = false;
    }
}
