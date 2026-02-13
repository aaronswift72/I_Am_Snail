using UnityEngine;
using UnityEngine.InputSystem;

public class MouseCamera : MonoBehaviour
{

    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Rotation")]
    public float mouseSensitivity = 150f;
    public float minY = -40f;
    public float maxY = 70f;

    [Header("Distance")]
    public float distance = 6f;
    public float minDistance = 1.5f;
    public float cameraRadius = 0.3f;

    [Header("Smoothing")]
    public float smoothSpeed = 10f;

    private float yaw;
    private float pitch;

    private Vector3 currentVelocity;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    void LateUpdate()
    {
        if (target == null) return;

        //mouse input
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;
        pitch =Mathf.Clamp(pitch, minY, maxY);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        // Desired camera pos
        Vector3 desiredPosition = target.position - rotation * Vector3.forward * distance;

        // Wall clipping prevention
        Vector3 direction = (desiredPosition - target.position).normalized;
        float targetDistance = distance;

        if (Physics.SphereCast(target.position, cameraRadius, direction, out RaycastHit hit, distance))
        {
            targetDistance = Mathf.Max(hit.distance - 0.1f, minDistance);
        }

        Vector3 finalPosition = target.position - rotation * Vector3.forward * targetDistance;

        transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref currentVelocity, 1f / smoothSpeed);
        
        transform.rotation = rotation;

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
