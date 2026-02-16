using UnityEngine;

public class MushroomSpawner : MonoBehaviour
{
    [Header("Mushroom Prefabs")]
    public GameObject[] mushroomPrefabs;

    [Header("Spawn Area")]
    public Vector3 spawnAreaCenter = Vector3.zero;
    public Vector3 spawnAreaSize = new Vector3(50f, 0f, 50f);

    [Header("Spawn Settings")]
    public int numberOfMushrooms = 50;
    public float minScale = 0.5f;
    public float maxScale = 1.5f;
    public LayerMask groundLayer;

    [Header("Terrain Texture Filtering")]
    public Terrain terrain; 
    public int[] allowedTextureIndices; 
    public float textureThreshold = 0.5f; 

    [Header("Spawn on Start")]
    public bool spawnOnAwake = true;

    void Start()
    {
        if (spawnOnAwake)
        {
            SpawnMushrooms();
        }
    }

    public void SpawnMushrooms()
    {
        int spawnedCount = 0;
        int attempts = 0;
        int maxAttempts = numberOfMushrooms * 10; 

        while (spawnedCount < numberOfMushrooms && attempts < maxAttempts)
        {
            attempts++;

            // Random position within the spawn area
            float randomX = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
            float randomZ = Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2);
            
            Vector3 spawnPos = spawnAreaCenter + new Vector3(randomX, 100f, randomZ);

            // Raycast down to find ground
            if (Physics.Raycast(spawnPos, Vector3.down, out RaycastHit hit, 200f, groundLayer))
            {
                // Check if this position has the right texture
                if (IsValidTexture(hit.point))
                {
                    // Pick random mushroom from array
                    GameObject randomMushroom = mushroomPrefabs[Random.Range(0, mushroomPrefabs.Length)];
                    
                    // Spawn it
                    GameObject mushroom = Instantiate(randomMushroom, hit.point, Quaternion.identity);
                    
                    // Random rotation
                    mushroom.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
                    
                    // Random scale variation
                    float randomScale = Random.Range(minScale, maxScale);
                    mushroom.transform.localScale = Vector3.one * randomScale;
                    
                    mushroom.transform.parent = transform;

                    spawnedCount++;
                }
            }
        }

        Debug.Log($"Spawned {spawnedCount} mushrooms out of {numberOfMushrooms} requested (took {attempts} attempts)");
    }

    bool IsValidTexture(Vector3 worldPos)
    {
        if (terrain == null || allowedTextureIndices.Length == 0)
            return true;

        // Convert world position to terrain-local position
        Vector3 terrainPos = worldPos - terrain.transform.position;
        
        // Get terrain data
        TerrainData terrainData = terrain.terrainData;
        
        // Convert to alphamap coordinates (0-1 range)
        float x = terrainPos.x / terrainData.size.x;
        float z = terrainPos.z / terrainData.size.z;
        
        int mapX = Mathf.FloorToInt(x * terrainData.alphamapWidth);
        int mapZ = Mathf.FloorToInt(z * terrainData.alphamapHeight);
        
        mapX = Mathf.Clamp(mapX, 0, terrainData.alphamapWidth - 1);
        mapZ = Mathf.Clamp(mapZ, 0, terrainData.alphamapHeight - 1);
        
        // Get texture mix at this point
        float[,,] alphamap = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);
        
        // Check if any allowed texture is strong enough
        foreach (int textureIndex in allowedTextureIndices)
        {
            if (textureIndex < alphamap.GetLength(2)) // Make sure index is valid
            {
                float textureWeight = alphamap[0, 0, textureIndex];
                if (textureWeight >= textureThreshold)
                {
                    return true; 
                }
            }
        }
        
        return false; 
    }

    // Visualize spawn area in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(spawnAreaCenter, spawnAreaSize);
    }
}