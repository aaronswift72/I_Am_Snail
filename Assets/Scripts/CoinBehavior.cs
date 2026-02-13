using UnityEngine;

public class CoinBehavior : MonoBehaviour
{
    [Header("Rotation")]
    public float rotationSpeed = 100f;

    private Vector3 startPosition;
    
    [Header("Proximity Sound")]
    //public float shimmerDistance = 15f; // Distance at which shimmer starts
    public bool isPlayerNear = false;

private Transform playerTransform;

    void Start()
    {
        startPosition = transform.position;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player!= null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        // Rotate the coin
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            AudioManager.instance.PlayCoinCollect();
            
            CoinManager.instance.AddCoin();
            Destroy(gameObject);
        }
    }
}