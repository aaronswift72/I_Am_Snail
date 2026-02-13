using UnityEngine;

public class SnailMovementSprite : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    [SerializeField] private Transform CameraTransform;

    [Header("Movement Sprites")]
    public Sprite left;
    public Sprite right;

    [Header("Air Sprites (Left-facing)")]
    public Sprite jump;
    public Sprite hardLand;

    [Header("Tuning")]
    public float deadZone = 0.1f;
    public float jumpThreshold = 0.5f;
    public float hardLandFallSpeed = 6f;
    public float hardLandDuration = 0.15f;

    private Rigidbody rb;
    private bool isGrounded;
    private bool wasGrounded;

    private float maxDownVelocity;
    private float landingTimer;

    [Header("Landing Slowdown")]
    public float landingSlowMultiplier = 0.4f;
    public float landingRecoveryTime = 1f;

    private float slowdownTimer;
    private PlayerBehavior playerMovement;
    public Sprite move;


    [SerializeField] private PlayerBehavior player;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        if (player == null)
        {
            playerMovement = GetComponent<PlayerBehavior>();
        }

    }

    void LateUpdate()
    {
        float camRelativeX = player.CameraRelativeX;

        // Horizontal-only billboard
        if (Camera.main != null)
        {
            Vector3 flatForward = CameraTransform.forward;
            flatForward.y = 0f;

            if (flatForward.sqrMagnitude > 0.001f)
            {
                flatForward.Normalize();
                transform.forward = flatForward;
            }
        }

        //Camera rekative left/right movement
        //float camRelativeX = 0f;

        if (Camera.main != null)
        {
            Vector3 camRight = Camera.main.transform.right;
            camRight.y = 0f;
            camRight.Normalize();

            camRelativeX = Vector3.Dot(rb.linearVelocity, camRight);
        }

        // Recovery after fall
        if (slowdownTimer > 0f && playerMovement != null)
        {
            slowdownTimer -= Time.deltaTime;

            float t = 1f - (slowdownTimer / landingRecoveryTime);
            playerMovement.speedMultiplier = Mathf.Lerp(landingSlowMultiplier, 1f, t);
        }


        float verticalVelocity = rb.linearVelocity.y;

        // Track fall speed while airborne
       // Grounded idle / movement sprite
        if (isGrounded)
        {
            // Only update sprite if actually moving
            if (Mathf.Abs(camRelativeX) > deadZone)
            {
                spriteRenderer.sprite = camRelativeX < 0f ? left : right;
                spriteRenderer.flipX = false;
            }
        }    
        // Airborne states
        else
        {
            maxDownVelocity = Mathf.Min(maxDownVelocity, verticalVelocity);
            // Jump sprite while rising
            if (verticalVelocity > jumpThreshold)
            {
                spriteRenderer.sprite = jump;
            }
            // Fall sprite when descending (but not hard landing)
            else
            {
                spriteRenderer.sprite = move;
            }
            spriteRenderer.flipX = player.LastDirection == -1;
        }

        // Landing detection 
        if (!wasGrounded && isGrounded)
        {
            if (Mathf.Abs(maxDownVelocity) >= hardLandFallSpeed)
            {
                AudioManager.instance.PlaySplat();
                landingTimer = hardLandDuration;
                slowdownTimer = landingRecoveryTime;

                if (playerMovement != null)
                playerMovement.speedMultiplier = landingSlowMultiplier;
            }

            maxDownVelocity = 0f;
        }

        // Landing sprite overrides briefly
        if (landingTimer > 0f)
        {
            landingTimer -= Time.deltaTime;
            spriteRenderer.sprite = hardLand;
            spriteRenderer.flipX = player.LastDirection == -1;
            return;
        }
        wasGrounded = isGrounded;
    }

    // Ground detection
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }
}

