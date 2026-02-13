using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerBehavior : MonoBehaviour
{

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Slider jumpChargeSlider;

    public float speed = 5f;

    public float maxJumpForce = 25f;
    public float chargeRate = 30f;
    
    private Rigidbody rb;
    private bool isGrounded = true;

    private float currentJumpForce = 0f;
    private bool isCharging = false;
    public float minJumpForce = 13f;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    public float acceleration = 20f;
    public float maxSpeed = 6f;
    public float groundDrag = 3f;
    public float airDrag = 0.5f;

    public float speedMultiplier = 1f;

    public int LastDirection { get; private set; } = 1;
    public float CameraRelativeX { get; private set; }

    private Vector3 currentMove = Vector3.zero;
     

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionStay (Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit (Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
    
    
    // Update is called once per frame
    void Update()
    {
        // Charging Jump
        if (Keyboard.current.spaceKey.isPressed && isGrounded)
        {
            if(!isCharging)
            {
                AudioManager.instance.StartJumpCharge();
            }
            isCharging = true;
            currentJumpForce += chargeRate * Time.deltaTime;
            currentJumpForce = Mathf.Clamp(currentJumpForce, 0f, maxJumpForce);
            jumpChargeSlider.value = currentJumpForce;
            
        }
        // Releasing Jump
        if (Keyboard.current.spaceKey.wasReleasedThisFrame && isCharging && isGrounded)
        {
            
            AudioManager.instance.StopJumpCharge();
            AudioManager.instance.PlayJump();
            if (Mathf.Abs(currentMove.x) > 0.01f)
            {
                LastDirection = currentMove.x > 0 ? 1 : -1;
            }

            float finalJumpForce = Mathf.Max(currentJumpForce, minJumpForce);
            rb.AddForce(Vector3.up * finalJumpForce, ForceMode.Impulse);
            currentJumpForce = 0f;
            isCharging = false;
            isGrounded = false;
            jumpChargeSlider.value = 0f;
        }
        if (!Keyboard.current.spaceKey.isPressed && isCharging)
        {
            AudioManager.instance.StopJumpCharge();
            isCharging = false;
        }
    }
    void FixedUpdate()
    {
        //Player Movement
        Vector3 input = Vector3.zero;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) input += Vector3.forward;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) input += Vector3.back;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) input += Vector3.left;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) input += Vector3.right;

        // Camera-relative directions
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        // Flatten so looking up/down doesnt add vertical movement
        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = (camForward * input.z + camRight * input.x).normalized;
        currentMove = move;
        float finalSpeed = speed * speedMultiplier;

        // Apply movement
        rb.MovePosition(rb.position + move * finalSpeed * Time.deltaTime);

        // Better Gravity
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Keyboard.current.spaceKey.isPressed)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }

        // Acceleration
        if (move != Vector3.zero)
        {
            Vector3 desiredVelocity = move.normalized * maxSpeed;
            Vector3 velocityChange = desiredVelocity - rb.linearVelocity;
            velocityChange.y = 0f;

            rb.AddForce(velocityChange * acceleration, ForceMode.Acceleration);
        }

        // Drag Controls Sliding
        rb.linearDamping = isGrounded ? groundDrag : airDrag;

        // Update facing based on horizontal movement
        if (Mathf.Abs(move.x) > 0.01f)
        {
            LastDirection = move.x > 0 ? 1 : -1;
        }
        CameraRelativeX = move.x;
    }
}
