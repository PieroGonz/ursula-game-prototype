using UnityEngine;
using System.Collections;

public class MasterScriptExtensions : MonoBehaviour
{
    private MasterScript masterScript;

    void Start()
    {
        masterScript = GetComponent<MasterScript>();
    }

    public void TeleportToBeach()
    {
        // Implement your Teleport logic here.
        // For example, change the position of the player to the beach location.
        Debug.Log("Teleporting to the beach...");
        // Set the position of the player to the beach coordinate.
        // You can change the transform directly, or you can use a method on the player object that does this.

        // Example:
        // masterScript.player.transform.position = new Vector3(beachX, beachY, beachZ);
    }
}
