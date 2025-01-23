using Lean.Touch;
using UnityEngine;

public class CenterCamera : MonoBehaviour
{
    public MoveToCenter2D target; // The target to center on (e.g., Ursula)
    public float centerSpeed = 2.0f; // Speed of centering on the target
    public Camera camera;

    private LeanDragCamera dragCamera;

    private bool isCentering = false;

    public bool disableCameraMovement = false;

    void Start()
    {
        gameObject.TryGetComponent<LeanDragCamera>(out dragCamera);
    }
    void Update()
    {
        if (target.isPlayMode)
        {
            dragCamera.enabled = true;
        }
        else if (!target.cameraShouldnotFollow)
        {
            dragCamera.enabled = false;
            CenterOnTarget();
        }
    }

    void CenterOnTarget()
    {
        Vector3 targetPosition = new Vector3(target.transform.position.x, target.transform.position.y - 1f, camera.transform.position.z);
        camera.transform.position = Vector3.Lerp(camera.transform.position, targetPosition, Time.deltaTime * centerSpeed);

        if (Vector3.Distance(camera.transform.position, targetPosition) < 0.1f)
        {
            isCentering = false; // Stop centering when close enough
        }
    }

    public void ActivateCenterMode()
    {
        isCentering = true;
    }

    public void DeactivateCenterMode()
    {
        isCentering = false;
    }
}
