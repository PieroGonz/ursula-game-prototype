using UnityEngine;

public class ParabolicBall : MonoBehaviour
{
    public float initialKickForce = 10f;        // Horizontal force applied on kick
    public float throwVerticalForce = 15f;      // Initial vertical force (height) on kick
    public float gravity = 9.8f;                // Gravity effect on the height (Z axis)
    public float groundLevel = 35f;             // Height of the ground in the Z axis
    public Transform shadowTransform;           // Reference to the shadow object
    public Transform rotationTransform;         // Separate transform for controlling rotation
    public float clickRange = 1.5f;             // Distance threshold for clicking to kick the ball
    public float bounceDampening = 0.6f;        // Dampening factor for vertical bounce
    public float dragCoefficient = 0.05f;       // Drag coefficient for air resistance
    public float frictionCoefficient = 0.1f;    // Friction coefficient for ground resistance
    public float minimalVelocityThreshold = 0.1f; // Minimum velocity before stopping the ball
    public float rotationSpeedFactor = 10f;     // Multiplier for rotation speed based on velocity

    private Rigidbody2D rb;
    private float zHeight;                      // Fake height in the Z axis relative to the ground level
    private float verticalVelocity;             // Velocity for controlling the height (Z axis)
    private bool isAirborne = false;            // Tracks if the ball is in the air

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        zHeight = groundLevel;                  // Set the ball�s initial position at ground level
        verticalVelocity = throwVerticalForce;  // Initial vertical velocity when kicked
    }

    void Update()
    {
        // Check for mouse click to kick the ball
        if (Input.GetMouseButtonDown(0) && !isAirborne) // Left mouse click and ball is on ground
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector2.Distance(mousePosition, transform.position) <= clickRange)
            {
                // Kick the ball toward the click position
                Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;
                //Kick(direction);
            }
        }

        // Manage height (Z-axis) gravity effect when airborne
        if (isAirborne)
        {
            verticalVelocity -= gravity * Time.deltaTime; // Apply gravity to vertical velocity
            zHeight += verticalVelocity * Time.deltaTime; // Update fake Z height based on velocity

            // Clamp zHeight to ensure it doesn't go below ground level
            if (zHeight < groundLevel)
            {
                zHeight = groundLevel; // Set height to ground level
                verticalVelocity *= -bounceDampening; // Reverse and dampen vertical velocity for bounce effect

                // Stop bouncing if vertical velocity is minimal
                if (Mathf.Abs(verticalVelocity) < minimalVelocityThreshold)
                {
                    isAirborne = false;
                    verticalVelocity = 0f;
                }
            }

            // Apply drag while in the air
            ApplyAirDrag();
        }
        else
        {
            // Apply friction when grounded
            ApplyGroundFriction();
        }

        // Update rotation based on velocity
        UpdateRotation();

        // Update sprite position based on zHeight
        UpdatePositionWithHeight();
        UpdateShadow();
        UpdateSortingOrder();
    }

    public void Kick()
    {
        // Apply horizontal kick force in X-Y plane
        //rb.velocity = direction * initialKickForce; // Set initial X-Y velocity

        // Set initial vertical velocity for parabolic arc
        verticalVelocity = throwVerticalForce;
        zHeight = groundLevel;
        isAirborne = true;
    }

    private void ApplyAirDrag()
    {
        // Apply drag force based on current horizontal velocity while in the air
        if (rb.linearVelocity.magnitude > minimalVelocityThreshold)
        {
            Vector2 dragForce = -rb.linearVelocity.normalized * dragCoefficient * rb.linearVelocity.sqrMagnitude;
            rb.AddForce(dragForce, ForceMode2D.Force);

            // Stop completely if the drag brings us below the threshold
            if (rb.linearVelocity.magnitude < minimalVelocityThreshold)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }

    private void ApplyGroundFriction()
    {
        // Apply friction force based on current horizontal velocity when on the ground
        if (rb.linearVelocity.magnitude > minimalVelocityThreshold)
        {
            Vector2 frictionForce = -rb.linearVelocity.normalized * frictionCoefficient;
            rb.AddForce(frictionForce, ForceMode2D.Force);

            // Stop completely if the friction brings us below the threshold
            if (rb.linearVelocity.magnitude < minimalVelocityThreshold)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }

    private void UpdateRotation()
    {
        if (rotationTransform != null && rb.linearVelocity.magnitude > minimalVelocityThreshold) // Only update rotation if moving
        {
            // Calculate the movement direction of the ball based on its velocity
            Vector3 movementDirection = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, verticalVelocity).normalized;

            // Determine the target rotation that aligns with the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection, Vector3.right);

            // Smoothly rotate the ball to face the movement direction
            //rotationTransform.rotation = Quaternion.Slerp(rotationTransform.rotation, targetRotation, Time.deltaTime * rotationSpeedFactor);

            // Calculate the rolling axis using Vector3.Cross for a perpendicular direction
            Vector3 rollingAxis = Vector3.Cross(movementDirection, Vector3.right).normalized;

            // Calculate rolling effect based on speed
            float rollAmount = rb.linearVelocity.magnitude * rotationSpeedFactor * Time.deltaTime;

            // Apply the rolling effect around the calculated rolling axis
            rotationTransform.Rotate(rollingAxis, rollAmount, Space.World);
        }
    }



    private void UpdatePositionWithHeight()
    {
        // Update the position based on the X-Y position and Z height (pseudo 3D effect)
        transform.position = new Vector3(transform.position.x, transform.position.y, zHeight);
    }

    private void UpdateShadow()
    {
        // Set the shadow position to the X-Y ground position of the ball
        if (shadowTransform != null)
        {
            shadowTransform.position = new Vector3(transform.position.x, transform.position.y, groundLevel);

            // Adjust shadow size based on the ball's height (zHeight)
            float maxShadowScale = 1f;   // Shadow size when the ball is at ground level
            float minShadowScale = 0.5f; // Shadow size when the ball is at its highest
            float shadowScale = Mathf.Lerp(minShadowScale, maxShadowScale, 1 - (zHeight - groundLevel) / throwVerticalForce);

            shadowTransform.localScale = new Vector3(shadowScale, shadowScale, 1);
        }
    }

    private void UpdateSortingOrder()
    {
        // Adjust sorting order based on zHeight for depth effect
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingOrder = (int)(-transform.position.y * 10); // Lower Y position means higher sorting order
        }
    }
}
