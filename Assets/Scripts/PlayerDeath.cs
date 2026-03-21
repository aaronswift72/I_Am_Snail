using UnityEngine;
using System.Collections;

public class PlayerDeath : MonoBehaviour
{
    [Header("Respawn")]
    public Transform respawnPoint;

    [Header("Invincibility After Respawn")]
    public float invincibilityDuration = 2f;

    [Header("References")]
    public GiantSnailAI giantSnail;

    private bool isDead = false;
    private bool isInvincible = false;

    private Rigidbody rb;
    private PlayerBehavior playerBehavior;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerBehavior = GetComponent<PlayerBehavior>();

        if (giantSnail == null)
            giantSnail = FindFirstObjectByType<GiantSnailAI>();
            
    }

    public void Die()
    {
        if (isDead || isInvincible) return;

        isDead = true;
        AudioManager.instance?.PlaySplat();
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        // Freeze snail
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }
        if (playerBehavior != null)
            playerBehavior.enabled = false;

        yield return new WaitForSeconds(0.5f);

        // tp to spawn point
        Vector3 spawnPos = respawnPoint != null ? respawnPoint.position : Vector3.zero;
        transform.position = spawnPos;

        // Restore control
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
        }
        if (playerBehavior != null)
            playerBehavior.enabled = true;

        isDead = false;

        if (giantSnail != null)
            giantSnail.ResetAttack();

        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }
}