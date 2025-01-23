using System.Collections;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;
using System.Net;
using Lean.Touch;

public class MasterScript : MonoBehaviour
{
    public CinemachineVirtualCamera introCamera; // The camera for the intro
    public CinemachineVirtualCamera followCamera; // The camera that follows Ursula
    public CinemachineVirtualCamera targetCamera; // The camera that follows Ursula
    public MoveToCenter2D ursulaScript;
    public RainManager rainManager;
    public LeanSelectByFinger SelectManager;

    public Transform rightPoint; // Point on the right side of the arena
    public Transform leftPoint; // Point on the left side where Ursula is

    public GameObject _foundUrsulaVFX;
    public GameObject _surprisedEmoji;

    public GameObject _postShowerMidPoint;
    public GameObject _postShowerTree;
    public GameObject _textShowerTree;

    public GameObject _microphoneGameObject;

    public GameObject _dragSequenceFirstPos;
    public GameObject _dragSequenceText;

    public GameObject hatToEquip;

    public enum DemoState
    {
        CameraMoveToRight,
        CameraMoveToLeft,
        Showering,
        RunAway,
        ActivateMicrophone,
        DragSequence,
        WaitingForHatGame,
        HatGame,
        WaitForEquipSequence,
        HatEquipSequence,
        WaitToTeleportSequence,
        TeleportToBeachSequence,
        End
    }

    public DemoState currentState;

    void Start()
    {
        currentState = DemoState.CameraMoveToRight;
        StartCoroutine(RunDemoSequence());

    }

    private IEnumerator RunDemoSequence()
    {
        while (currentState != DemoState.End)
        {
            switch (currentState)
            {
                case DemoState.CameraMoveToRight:
                    yield return StartCoroutine(MoveCameraToPoint(rightPoint, 1.5f));
                    currentState = DemoState.CameraMoveToLeft;
                    break;

                case DemoState.CameraMoveToLeft:
                    yield return StartCoroutine(MoveCameraToPoint(leftPoint, 1.5f));
                    currentState = DemoState.Showering;
                    break;

                case DemoState.Showering:
                    yield return StartCoroutine(HandleShowering());
                    break;

                case DemoState.RunAway:
                    yield return StartCoroutine(PostShowerSequence());
                    break;

                case DemoState.ActivateMicrophone:
                    yield return StartCoroutine(HandlePlayingWithBall());
                    break;

                case DemoState.DragSequence:
                    yield return StartCoroutine(DragSequence());
                    break;

                case DemoState.WaitingForHatGame:
                    yield return null;
                    break;

                case DemoState.HatGame:
                    yield return StartCoroutine(HandleHatGame());
                    break;
                case DemoState.WaitForEquipSequence:
                    yield return null;
                    break;
                case DemoState.HatEquipSequence:
                    yield return StartCoroutine(EquipHatSequence());
                    break;
                case DemoState.WaitToTeleportSequence:
                    yield return null;
                    break;
                case DemoState.TeleportToBeachSequence:
                    yield return StartCoroutine(TeleportToBeach());
                    break;
            }
        }
    }

    private IEnumerator MoveCameraToPoint(Transform targetPoint, float duration)
    {
        // Set the intro camera as active
        introCamera.Priority = 10;
        followCamera.Priority = 5;
        targetCamera.Priority = 4;

        float elapsedTime = 0f;
        Vector3 startPosition = introCamera.transform.position;

        while (elapsedTime < duration)
        {
            // SmoothStep function for smoother interpolation
            float t = elapsedTime / duration;
            t = Mathf.SmoothStep(0f, 1f, t); // Apply smoothstep to make the movement smoother

            // Lerp the camera's position towards the target point using the smooth factor
            introCamera.transform.position = Vector3.Lerp(startPosition, targetPoint.position, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rainManager.StartRain();
        ursulaScript.currentPersonality.PlayShoweringAnimation(ursulaScript.riveTexture);

        if (currentState == DemoState.CameraMoveToRight)
        {
            yield return new WaitForSeconds(0.1f);
        }

        // Snap to the final position just in case
        introCamera.transform.position = targetPoint.position;

        // After moving to the left point, blend to the follow camera to lock onto Ursula
        if (currentState == DemoState.CameraMoveToLeft)
        {
            _foundUrsulaVFX.SetActive(true);
            introCamera.Priority = 5;
            followCamera.Priority = 10;
        }
    }

    private IEnumerator ActivateEmoji(GameObject emoji, float time)
    {
        emoji.SetActive(true);

        yield return new WaitForSeconds(time);

        emoji.SetActive(false);
    }

    private IEnumerator HandleShowering()
    {
        // Trigger showering sequence
        // Starts the rain for the shower
        ursulaScript.SetMode(false);  // Non-play mode, she’s stationary

        yield return new WaitForSeconds(1.5f);

        ursulaScript.currentPersonality.PlaySurprisedAnimation(ursulaScript.riveTexture);
        StartCoroutine(ActivateEmoji(_surprisedEmoji, 2f));

        yield return new WaitForSeconds(0.5f);

        currentState = DemoState.RunAway;

        // Wait for a defined duration of showering

        // Stop the rain
        //rainManager.StopRain();

        // Proceed to the next state
    }

    private IEnumerator TeleportScene()
    {
        // Trigger showering sequence
        // Starts the rain for the shower
        //ursulaScript.SetMode(false);  // Non-play mode, she’s stationary

        //yield return new WaitForSeconds(1.5f);

        ursulaScript.currentPersonality.PlayTeleportToBackgroundScene(ursulaScript.riveTexture);
        //StartCoroutine(ActivateEmoji(_surprisedEmoji, 2f));

        yield return new WaitForSeconds(0.5f);

        currentState = DemoState.RunAway;

        // Wait for a defined duration of showering

        // Stop the rain
        //rainManager.StopRain();

        // Proceed to the next state
    }



    private IEnumerator PostShowerSequence()
    {
        // Brief moment for Ursula's reaction after being caught in the shower
        yield return new WaitForSeconds(0.2f);

        // Move Ursula from the shower position to the first point (e.g., surprised and running away)

        yield return StartCoroutine(MoveToTargetBetweenPoints(_postShowerMidPoint.transform, 40f, _postShowerTree.transform));

        // Optionally, you can add logic here for Ursula hiding, like playing a hiding animation
        // PlayHideAnimation();

        currentState = DemoState.ActivateMicrophone;
    }

    private IEnumerator MoveToTargetBetweenPoints(Transform pointA, float speed, Transform pointB = null)
    {
        if (pointA == null) yield break;

        // Move to the first point
        while (Vector3.Distance(ursulaScript.transform.position, pointA.position) > 0.1f)
        {
            ursulaScript.MoveToTarget(pointA, speed, speed, "RunAnimation");
            ursulaScript.currentPersonality.PlayMoveAnimation(ursulaScript.riveTexture);

            yield return null; // Wait for the next frame and continue moving
        }

        // Check if pointB is provided
        if (pointB != null)
        {
            // Move to the second point
            while (Vector3.Distance(ursulaScript.transform.position, pointB.position) > 0.1f)
            {
                ursulaScript.MoveToTarget(pointB, speed, speed, "RunAnimation");
                ursulaScript.currentPersonality.PlayMoveAnimation(ursulaScript.riveTexture);
                yield return null; // Wait for the next frame and continue moving
            }
        }

        ursulaScript.currentPersonality.PlayIdleAnimation(ursulaScript.riveTexture);

        yield return new WaitForSeconds(0.3f);

        if (currentState == DemoState.RunAway)
            StartCoroutine(ActivateEmoji(_textShowerTree, 4f));
    }

    bool micActivatedOnce = false;

    private IEnumerator HandlePlayingWithBall()
    {
        // Start ball-playing sequence
        //ursulaMovement.SetMode(true);  // Play mode, allow movement
        yield return new WaitForSeconds(3f);
        if (!micActivatedOnce)
        {
            _microphoneGameObject.SetActive(true);
            micActivatedOnce = true;
        }
        //_microphoneGameObject.SetActive(true);

        // Add logic for playing with the ball (e.g., moving to a ball object)
        //ursulaMovement.MoveToGameplayObject();  // Move towards the ball

        // Wait for the interaction to complete
    }

    public void ActivateDragSequence()
    {
        if (currentState == DemoState.ActivateMicrophone)
        {
            currentState = DemoState.DragSequence;

            StartCoroutine(ActivateEmoji(_dragSequenceText, 4f));
        }
        else if (currentState == DemoState.WaitingForHatGame)
        {
            currentState = DemoState.HatGame;

            //StartCoroutine(ActivateEmoji(_dragSequenceText, 4f));
        }
        else if (currentState == DemoState.WaitForEquipSequence)
        {
            currentState = DemoState.HatEquipSequence;

            //StartCoroutine(ActivateEmoji(_dragSequenceText, 4f));
        }
        else if (currentState == DemoState.WaitToTeleportSequence)
        {
            currentState = DemoState.TeleportToBeachSequence;

            //StartCoroutine(ActivateEmoji(_dragSequenceText, 4f));
        }
    }

    private IEnumerator DragSequence()
    {
        // Start ball-playing sequence
        //ursulaMovement.SetMode(true);  // Play mode, allow movement

        yield return StartCoroutine(MoveToTargetBetweenPoints(_dragSequenceFirstPos.transform, 10f));

        _microphoneGameObject.SetActive(true);

        if (ursulaScript.transform.GetComponent<LeanSelectableByFinger>().IsSelected)
        {
            Debug.Log("SELECTED");
        }

        // Add logic for playing with the ball (e.g., moving to a ball object)
        //ursulaMovement.MoveToGameplayObject();  // Move towards the ball

        // Wait for the interaction to complete
        yield return new WaitForSeconds(2.0f);

        // Proceed to the next state
        currentState = DemoState.WaitingForHatGame;
    }

    private IEnumerator HandleHatGame()
    {
        ursulaScript.currentPersonality.PlayHatGameAnimation(ursulaScript.riveTexture, ursulaScript);

        targetCamera.Priority = 25;

        // Start the hat game sequence
        ursulaScript.SetMode(true);  // Play mode for hat game

        // Simulate hat game interaction (could involve moving to a specific object or reacting to inputs)
        //yield return ursulaMovement.HandleHatCollision(ursulaMovement.hats[0].gameObject, true);  // Simulate picking up a hat

        // Wait for the interaction to complete
        yield return new WaitForSeconds(3.0f);

        // End the demo
        currentState = DemoState.WaitForEquipSequence;
    }

    private IEnumerator EquipHatSequence()
    {
        ursulaScript.currentPersonality.PlayEquipHatAnimation(ursulaScript.riveTexture, ursulaScript);

        yield return new WaitForSeconds(2.0f);

        ursulaScript.OnTellEquipHAt(hatToEquip);

        followCamera.Priority = 25;

        yield return new WaitForSeconds(5.0f);

        currentState = DemoState.WaitToTeleportSequence;
    }

    private IEnumerator TeleportToBeach()
    {
        ursulaScript.currentPersonality.PlayTeleportAnimation(ursulaScript.riveTexture, ursulaScript, ursulaScript.transform.GetComponent<VFXHandler>());

        yield return new WaitForSeconds(2.0f);

        followCamera.Priority = 25;

        yield return new WaitForSeconds(5.0f);

        currentState = DemoState.End;
    }
}
