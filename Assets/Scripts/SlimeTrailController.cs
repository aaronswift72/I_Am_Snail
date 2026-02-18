using UnityEngine;

public class SlimeTrailController : MonoBehaviour
{
    public TrailRenderer trailRenderer;
    
    private bool isGrounded = true;

    void Update()
    {
        trailRenderer.emitting = isGrounded;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}