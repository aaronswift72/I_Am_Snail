using UnityEngine;

public class GiantSnailAI : MonoBehaviour
{
    [Header("References")]
    public Transform playerTransform;

    [Header("Movement")]
    public float moveSpeed = 1.5f;

    [Header("Sprite")]
    public SpriteRenderer spriteRenderer;

    [Header("Billboard")]
    [SerializeField] private Transform cameraTransform;

    private bool hasAttacked = false;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (playerTransform == null) return;

        // Move toward the player
        Vector3 targetPos = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        // Billboard: always face the camera
        if (cameraTransform != null && spriteRenderer != null)
        {
            Vector3 flatForward = cameraTransform.forward;
            flatForward.y = 0f;
            if (flatForward.sqrMagnitude > 0.001f)
            {
                flatForward.Normalize();
                transform.forward = flatForward;
            }

            // Flip sprite based on movement relative to camera
            Vector3 camRight = cameraTransform.right;
            camRight.y = 0f;
            camRight.Normalize();

            Vector3 toPlayer = (playerTransform.position - transform.position).normalized;
            float dot = Vector3.Dot(toPlayer, camRight);
            spriteRenderer.flipX = dot < 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasAttacked)
        {
            hasAttacked = true;

            PlayerDeath death = other.GetComponent<PlayerDeath>();
            if (death != null)
                death.Die();
        }
    }

    public void ResetAttack()
    {
        hasAttacked = false;
    }
}