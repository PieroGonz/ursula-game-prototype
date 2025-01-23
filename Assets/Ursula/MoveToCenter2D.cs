using Febucci.UI;
using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class MoveToCenter2D : MonoBehaviour
{
    public Personality currentPersonality;
    public Personality wizardPersonality;
    public Transform target; // The object (Ursula) that needs to be moved
    public float speed = 2.0f; // Speed of the movement
    public float playSpeedNormal = 2.0f; // Normal speed in play mode
    public float playSpeedExcited = 5.0f; // Excited speed in play mode
    public float minStayTime = 3.0f; // Minimum stay time at each gameplay object
    public float maxStayTime = 10.0f; // Maximum stay time at each gameplay object
    public List<Transform> gameplayObjects; // List of gameplay objects
    public string walkAnimation = "Walk"; // Name of the walk animation
    public string idleAnimation = "Idle"; // Name of the idle animation
    public string hatCollisionAnimation; // Name of the hat collision animation
    public string hatRefuseAnimation; // Name of the hat collision animation
    public Transform hatAttachmentBone; // Bone to attach the hat to
    public float collisionStopDuration = 2.0f; // Duration to stop movement upon collision
    public bool acceptHat = true; // Control to accept or refuse the hat
    public Transform currentGameplayObject; // Current target for debugging
    public LeanSelectByFinger _leanSelectByFinger;
    public bool refusedHat = false;
    public Canvas inventoryCanvas;
    public GameObject TextPrefab;



    private bool moveToWaypoint = false; // Flag to control movement to a waypoint



    private Camera mainCamera;
    public RiveSpriteRenderer riveTexture; // Reference to the RiveTexture script
    private bool isWalking = false;
    public bool isPlayMode = false;
    private bool movingToGameplayObject = false;
    public bool isStopped = false; // Flag to indicate if movement is stopped
    private Coroutine stayCoroutine;
    private bool _justRained = false;

    void Start()
    {
        mainCamera = Camera.main;
        riveTexture = target.GetComponent<RiveSpriteRenderer>(); // Get the RiveTexture component from the target
    }

    void Update()
    {
        if (!ursulaInsideCastle)
        {
            CheckRainState();
        }

        if (moveToWaypoint) return;

        if (isPlayMode)
        {
            if (isStopped) return;
            if (!movingToGameplayObject && currentGameplayObject == null)
            {
                ChooseNextGameplayObject();
            }
            //MoveToGameplayObject(); 
        }
        else
        {
            //MoveToCenter();
        }
    }

    public GameObject castleObject; // The other object to check collision with
    private bool ursulaInsideCastle = false;

    private void OnCollisionStay2D(Collision2D collision)
    {

    }

    // Optional: If you also want to know when the collision starts and ends
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision");

        if (collision.gameObject.CompareTag("Hat"))
        {
            StartCoroutine(HandleHatCollision(collision.gameObject));
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {

    }

    private Coroutine ursulaChoosesHat;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject == castleObject)
        {
            Debug.Log("Constant trigger collision with " + castleObject.name);
            // Additional logic for constant trigger collision

            if (!isPlayMode)
            {
                inventoryCanvas.gameObject.SetActive(true);

                if (currentHat == null)
                {
                    riveTexture.PlayAnimation("Looking Down Loop");
                }

                if (ursulaChoosesHat == null && currentHat == null)
                {
                    ursulaChoosesHat = StartCoroutine(ChooseHatAfterDelay());
                }
            }
            else
            {
                ursulaChoosesHat = null;
                inventoryCanvas.gameObject.SetActive(false);
            }    
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == castleObject)
        {
            Debug.Log("Started trigger colliding with " + castleObject.name);
            // Additional logic for trigger collision start

            ursulaInsideCastle = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == castleObject)
        {
            Debug.Log("Stopped trigger colliding with " + castleObject.name);
            // Additional logic for trigger collision end

            ursulaInsideCastle = false;
        }
    }

    public void SetMode(bool playMode)
    {
        isPlayMode = playMode;
        if (!isPlayMode)
        {
            currentGameplayObject = null;
            movingToGameplayObject = false;
            //StopAllCoroutines();
            stayCoroutine = null;
            //StartCoroutine(ChangeUrsulaSize(.3f, 9.5f));
        }
        else
        {
            //StartCoroutine(ChangeUrsulaSize(.3f, 12f));
        }
    }

    public IEnumerator ChangeUrsulaSize(float duration, float targetScale)
    {
        float originalScale = mainCamera.orthographicSize;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            mainCamera.orthographicSize = Mathf.Lerp(originalScale, targetScale, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.orthographicSize = targetScale; // Ensure the final scale is set
    }

    private void MoveToCenter()
    {
        // Get the center position of the camera in world space
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 10f);
        Vector3 worldCenter = mainCamera.ScreenToWorldPoint(screenCenter);

        // Calculate the target position (center of the camera)
        Vector3 targetPosition = new Vector3(worldCenter.x, worldCenter.y, worldCenter.z);

        // Move the target towards the center position
        target.position = Vector3.MoveTowards(target.position, targetPosition, Time.deltaTime * speed);

        // Check if the target has reached the center
        if (Vector3.Distance(target.position, targetPosition) < 0.1f)
        {
            // Set to idle animation once the target reaches the center

            if (currentHatData != null)
            {
                currentHatData.PlayIdleAnimation(riveTexture, transform);
            }
            else
            {
                currentHatData.PlayIdleAnimation(riveTexture, transform);
            }

            isWalking = false;
        }
        else
        {
            riveTexture.PlayAnimation(walkAnimation);
            isWalking = true;
        }
    }

    public void MoveToTarget(Transform target, float playSpeedExcited, float playSpeedNormal, string walkAnimation)
    {

        if (target == null) return;

        // Move towards the target
        float currentSpeed = Random.Range(0f, 1f) > 0.5f ? playSpeedExcited : playSpeedNormal;
        transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * currentSpeed);

        // Check if Ursula has reached the target
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            if (stayCoroutine == null)
            {
                //stayCoroutine = StartCoroutine(StayAtTarget());
            }
        }
        else
        {
            // Play the walk animation
            if (riveTexture != null)
            {
                riveTexture.PlayAnimation(walkAnimation);
            }
            isWalking = true;
            movingToGameplayObject = true;
        }
    }

    public void MoveToWaypoint(Vector3 waypoint)
    {
        moveToWaypoint = true;
        StartCoroutine(MoveToWaypointCoroutine(waypoint));
    }

    private IEnumerator MoveToWaypointCoroutine(Vector3 waypoint)
    {
        while (Vector3.Distance(target.position, waypoint) > 0.1f)
        {
            target.position = Vector3.MoveTowards(target.position, waypoint, Time.deltaTime * speed);
            riveTexture.PlayAnimation(walkAnimation);

            if (stayCoroutine == null)
            {
                stayCoroutine = StartCoroutine(StayAtGameplayObject());
            }

            yield return null;
        }

        if (currentHatData != null)
        {
            currentHatData.PlayIdleAnimation(riveTexture, transform);
        }
        else
        {
            riveTexture.PlayAnimation(idleAnimation);
        }
    }

    private void ChooseNextGameplayObject()
    {
        if (gameplayObjects.Count == 0) return;

        int randomIndex = Random.Range(0, gameplayObjects.Count);
        currentGameplayObject = gameplayObjects[randomIndex];
    }

    private IEnumerator StayAtGameplayObject()
    {
        if (currentHatData != null)
        {
            currentHatData.PlayIdleAnimation(riveTexture, transform);
        }
        else
        {
            riveTexture.PlayAnimation(idleAnimation);
        }
        isWalking = false;

        float stayTime = Random.Range(minStayTime, maxStayTime);
        yield return new WaitForSeconds(stayTime);

        movingToGameplayObject = false; // Ready to choose the next gameplay object
        currentGameplayObject = null;
        stayCoroutine = null;
        moveToWaypoint = false;
    }

    private void CheckRainState()
    {
        RainManager rainManager = GameObject.FindObjectOfType<RainManager>();

        if (rainManager != null && rainManager.IsRaining)
        {
            if (currentHatData == null || currentHatData.name != "UmbrellaHat")
            {
                //riveTexture.PlayAnimation("Dance Loop");
                _justRained = true;

                if (currentHatData != null && currentHatData.name == "WizardHat")
                {
                    //StartCoroutine(CheckRainAndStopCoroutine());
                }
            }
        }
        else if (_justRained || (currentHatData != null && currentHatData.name == "UmbrellaHat"))
        {
            //riveTexture.PlayAnimation("Dance Loop");

            if (currentHatData != null)
            {
                //currentHatData.PlayIdleAnimation(riveTexture, transform);
            }
            else
            {
                //riveTexture.PlayAnimation(idleAnimation);
            }

            _justRained = false;
        }
    }

    private IEnumerator CheckRainAndStopCoroutine()
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds

        // Check if it's still raining and if the wizard hat is equipped
        RainManager rainManager = GameObject.FindObjectOfType<RainManager>();
        if (rainManager != null && rainManager.IsRaining && currentHatData != null && currentHatData.name == "WizardHat")
        {
            // Play the special action animation and stop the rain
            currentHatData.PlaySpecialActionAnimation(riveTexture, transform);
            rainManager.StopRain();

            // Disable the special particle effect after some time (optional)
            yield return new WaitForSeconds(3f); // Adjust the duration as needed
            if (currentHatData != null)
            {
                currentHatData.ToggleParticleEffect(transform, currentHatData.specialParticleEffectName, false);
            }
        }
    }

    public void ChangeHatPreference()
    {
        acceptHat = true;
    }

    public Button[] hatButtons;

    private IEnumerator ChooseHatAfterDelay()
    {
        yield return new WaitForSeconds(3f); // Wait for 2 seconds

        Vector3 originalPosition = transform.position;

        // Choose a random hat button
        int randomIndex = Random.Range(0, hatButtons.Length);
        Button chosenButton = hatButtons[randomIndex];

        // Get the world position of the chosen button
        Vector3 buttonWorldPosition = GetWorldPositionOfButton(chosenButton);

        // Move Ursula to the chosen button position
        yield return MoveToPosition(buttonWorldPosition, 0.2f); // Move quickly (0.5s for example)

        // Simulate a button touch
        chosenButton.onClick.Invoke();

        // Move Ursula back to the original position
        yield return MoveToPosition(originalPosition, 0.1f); // Move quickly back

        // Call HandleHatCollision with the chosen hat game object
    }

    private Vector3 GetWorldPositionOfButton(Button button)
    {
        // Get the RectTransform component of the button
        RectTransform rectTransform = button.GetComponent<RectTransform>();

        // Convert the button's screen position to world position
        Vector3 screenPos = rectTransform.TransformPoint(rectTransform.rect.center);
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, mainCamera.nearClipPlane));
        worldPos.z = 0; // Ensure the z position is set to 0 for 2D

        return worldPos;
    }

    public bool cameraShouldnotFollow;

    private IEnumerator MoveToPosition(Vector3 targetPosition, float duration)
    {
        cameraShouldnotFollow = true;
        float elapsedTime = 0f;
        Vector3 startingPosition = transform.position;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition; // Ensure final position is set

        cameraShouldnotFollow = false;
    }

    public Hat[] hats; // Array of hats

    private GameObject currentHat; // Reference to the currently equipped hat
    public Hat currentHatData; // Data for the currently equipped hat

    public IEnumerator HandleHatCollision(GameObject hat, bool overrideAccept = false)
    {
        isStopped = true; // Stop movement

        if (acceptHat || overrideAccept)
        {
            // Unequip the current hat if there is one
            if (currentHat != null)
            {
                UnequipHat(currentHat);
            }

            _leanSelectByFinger.Deselect(hat.GetComponent<LeanSelectableByFinger>());

            riveTexture.PlayAnimation(hatCollisionAnimation); // Play collision animation

            // Disable physics on the hat
            Rigidbody2D hatRigidbody = hat.GetComponent<Rigidbody2D>();
            if (hatRigidbody != null)
            {
                hatRigidbody.simulated = false;
            }

            // Attach the hat to the specified bone
            hat.transform.SetParent(hatAttachmentBone);
            //hat.transform.localRotation = Quaternion.identity;
            hat.transform.localPosition = Vector3.zero;
            hat.SetActive(true);

            // Set the current hat reference and data
            currentHat = hat;
            currentHatData = GetHatData(hat.name); // Assuming the hat GameObject name matches the hat data name
            currentHatData.PlayIdleAnimation(riveTexture, transform);
        }
        else
        {
            refusedHat = true;
            // Apply force to the hat to throw it away
            riveTexture.PlayAnimation(hatRefuseAnimation); // Play collision animation

            Rigidbody2D hatRigidbody = hat.GetComponent<Rigidbody2D>();
            if (hatRigidbody != null)
            {
                Vector2 throwDirection = (new Vector2(hat.transform.position.x, hat.transform.position.y) - new Vector2(target.position.x, target.position.y)).normalized + Vector2.up;
                hatRigidbody.AddForce(throwDirection * 250f); // Adjust force as needed
            }
        }

        // Wait for the animation duration (or a fixed time)
        yield return new WaitForSeconds(collisionStopDuration);

        isStopped = false; // Resume movement
    }

    private void UnequipHat(GameObject hat)
    {

        if (currentHatData != null)
        {
            currentHatData.DisableIdleParticleEffect(transform);
        }
        // Detach the hat from the bone and re-enable its physics
        hat.transform.SetParent(null);
        hat.SetActive(false);

        Rigidbody2D hatRigidbody = hat.GetComponent<Rigidbody2D>();
        if (hatRigidbody != null)
        {
            hatRigidbody.simulated = true;
        }

        // Optionally, reset the hat's position and rotation
        //hat.transform.position = originalPosition; // Set this to the original position of the hat if needed
        hat.transform.rotation = Quaternion.identity;
    }

    private Hat GetHatData(string hatName)
    {
        foreach (Hat hat in hats)
        {
            if (hat.hatName == hatName)
            {
                return hat;
            }
        }
        return null;
    }

    public int tapCounter = 0;
    private Coroutine resetTapCounterCoroutine;
    public float tapResetInterval = 4f; // Set your desired reset interval in seconds
    public LeanFingerFilter Use = new LeanFingerFilter(true);
    public void OnUrsulaTapped()
    {
        var fingers = Use.UpdateAndGetFingers();

        if (fingers.Count > 0)
        {
            for (var i = 0; i < fingers.Count; i++)
            {
                var finger = fingers[i]; // tap and hold, then drag ursula

                if (finger.Age > 0.2f)
                    return;
            }
        }

        if (tapCounter >= 4)
        {
            currentPersonality.PlayTapAnnoyedAnimation(riveTexture, this, transform.GetComponent<VFXHandler>());

            transform.GetComponent<LeanSelectableByFinger>().enabled = false;
            StartCoroutine(EnableAfterDelay(4f));
            tapCounter = 0; // Reset the tap counter
        }
        else
        {
            tapCounter++;
            currentPersonality.PlayTapReactionAnimation(riveTexture, this, transform.GetComponent<VFXHandler>());

            // Start or restart the coroutine to reset the tap counter
            if (resetTapCounterCoroutine != null)
            {
                StopCoroutine(resetTapCounterCoroutine);
            }
            resetTapCounterCoroutine = StartCoroutine(ResetTapCounterAfterDelay());
        }
    }

    public void OnUrsulaDragging()
    {
        currentPersonality.PlayDragAnimation(riveTexture, this, transform.GetComponent<VFXHandler>());
    }

    public void OnUrsulaDragEnd()
    {
        currentPersonality.PlayDragEndAnimation(riveTexture, this, transform.GetComponent<VFXHandler>());
    }
    public void OnTellEquipHAt(GameObject hat)
    {
        currentPersonality.PlayEquipHatAnimation(riveTexture, this, transform.GetComponent<VFXHandler>());

        StartCoroutine(HandleHatCollision(hat.gameObject, true));

        currentPersonality = wizardPersonality;
    }

    private IEnumerator ResetTapCounterAfterDelay()
    {
        // Wait for the specified interval
        yield return new WaitForSeconds(tapResetInterval);

        // Reset the tap counter
        tapCounter = 0;
    }

    private IEnumerator EnableAfterDelay(float delay)
    {
        // Wait for the specified interval
        yield return new WaitForSeconds(delay);

        // Reset the tap counter
        transform.GetComponent<LeanSelectableByFinger>().enabled = true;
    }

    private Coroutine conversationCoroutine;

    public void ShowConversation(string message)
    {
        if (conversationCoroutine != null)
        {
            StopCoroutine(conversationCoroutine);
        }
        conversationCoroutine = StartCoroutine(ConversationRoutine(message));
    }

    public IEnumerator ConversationRoutine(string textValue)
    {

        TextAnimator_TMP _textAnimator = TextPrefab.GetComponentInChildren<TextAnimator_TMP>();
        TypewriterByCharacter _textTypewriter = TextPrefab.GetComponentInChildren<TypewriterByCharacter>();

        if (_textAnimator != null)
        {
            TextPrefab.SetActive(true);
            //_textAnimator.textFull = textValue;
            _textTypewriter.ShowText(textValue);

            yield return new WaitForSeconds(1.2f);

            _textTypewriter.StartDisappearingText();
        }
    }

    public void PerformSpecialAction()
    {
        if (currentHatData != null)
        {
            currentHatData.PlaySpecialActionAnimation(riveTexture,transform);
        }
    }
}
