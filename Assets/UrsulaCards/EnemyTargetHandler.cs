using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyTargetHandler : MonoBehaviour, IPointerClickHandler
{
    private void Start()
    {
        // Ensure this object has the Enemy tag
        if (tag != "Enemy")
        {
            tag = "Enemy";
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Check if this enemy is a valid target
        if (CardTargetingSystem.Instance != null)
        {
            CardTargetingSystem.Instance.ApplyCardToTarget(gameObject);
        }
    }
}
