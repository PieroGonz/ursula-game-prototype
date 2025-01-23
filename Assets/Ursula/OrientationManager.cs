using UnityEngine;

public class OrientationManager : MonoBehaviour
{
    public DeviceOrientation currentOrientation;
    private bool _isPlayModeActive = false;
    public Camera mainCamera;
    public MoveToCenter2D moveToCenter2D; // Reference to the MoveToCenter2D script
    private RenderTexture temporaryTexture;

    public Canvas _equipTheHatUrsula;

    void Start()
    {
        currentOrientation = Input.deviceOrientation;
        mainCamera = Camera.main;
        CheckOrientation();

        if (currentOrientation == DeviceOrientation.Unknown)
        {
            //EnterTalkingMode();
        }
    }

    void Update()
    {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        if (Input.deviceOrientation != currentOrientation)
        {
            currentOrientation = Input.deviceOrientation;
            CheckOrientation();
        }
#endif
    }

    public void SimulateOrientation(DeviceOrientation orientation)
    {
        currentOrientation = orientation;
        CheckOrientation();
    }

    void CheckOrientation()
    {
        if (currentOrientation == DeviceOrientation.LandscapeLeft ||
            currentOrientation == DeviceOrientation.LandscapeRight)
        {
            //EnterPlayMode();
        }
        else if (currentOrientation == DeviceOrientation.Portrait ||
                 currentOrientation == DeviceOrientation.PortraitUpsideDown)
        {
            //EnterTalkingMode();
        }
    }

    public void SwitchPerspective()
    {
        if (_isPlayModeActive)
        {
            //EnterTalkingMode();
        }
        else
        {
           // EnterPlayMode();
        }
    }

    public void EnterPlayMode()
    {
        _isPlayModeActive = true;
        moveToCenter2D.SetMode(true); // Set to play mode
        ClearBackground();

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        mainCamera.rect = new Rect(0, 0, 1, 1); // Full screen view for editor and Windows
#endif

        Debug.Log("Switched to Play Mode");
        _equipTheHatUrsula.gameObject.SetActive(false);
    }

    public void EnterTalkingMode()
    {
        _isPlayModeActive = false;
        moveToCenter2D.SetMode(false); // Set to talking mode
        ClearBackground();

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        mainCamera.rect = new Rect(0.25f, 0, 0.5f, 1); // Centered narrow view for editor and Windows
#endif
        if(moveToCenter2D.refusedHat)
        {
            _equipTheHatUrsula.gameObject.SetActive(true);
        }

        Debug.Log("Switched to Talking Mode");
    }

    private void ClearBackground()
    {
        mainCamera.clearFlags = CameraClearFlags.SolidColor;
        mainCamera.backgroundColor = Color.black;

        if (temporaryTexture == null)
        {
            temporaryTexture = new RenderTexture(Screen.width, Screen.height, 24);
        }

        RenderTexture originalTexture = mainCamera.targetTexture;
        mainCamera.targetTexture = temporaryTexture;
        mainCamera.Render();
        mainCamera.targetTexture = originalTexture;
        mainCamera.clearFlags = CameraClearFlags.Skybox;
        temporaryTexture.Release();
    }
}
