using UnityEngine;
using MoreMountains.Tools;

public class TeleportToBackground : MonoBehaviour
{
    public Transform backgroundPosition;
    private MoveToCenter2D moveToCenter2D;

    void Start()
    {
        moveToCenter2D = GetComponent<MoveToCenter2D>();
    }

    public void Teleport()
    {
        moveToCenter2D.MoveToTarget(backgroundPosition, moveToCenter2D.playSpeedExcited, moveToCenter2D.playSpeedNormal, moveToCenter2D.walkAnimation);
    }
}
