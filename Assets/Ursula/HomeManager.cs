using UnityEngine;
using UnityEngine.UI;

public class HomeManager : MonoBehaviour
{
    public GameObject waypoint;
    public MoveToCenter2D ursulaManager;

    void Start()
    {

    }

    public void OnObjectClicked()
    {
        ursulaManager.MoveToWaypoint(waypoint.transform.position);
    }
}
