using UnityEngine;
using UnityEngine.VFX;

public class WeatherManager : MonoBehaviour
{
    [Header("Rain")]
    public VisualEffect rainVFX;

    [Header("Timing")]
    public float minDryTime = 30f;   // min seconds between rain
    public float maxDryTime = 120f;  // max seconds between rain
    public float minRainDuration = 10f;
    public float maxRainDuration = 40f;

    public float fadeSpeed = 500f;     // how quickly rain fades in/out

    private bool isRaining = false;
    private float timer = 0f;
    private float targetSpawnRate = 0f;
    private float currentSpawnRate = 0f;
    private float nextDuration = 0f;

    public AudioManager audioManager;


    void Start()
    {
        rainVFX.SetFloat("SpawnRate", 0f);
        timer = Random.Range(minDryTime, maxDryTime); // wait before first rain
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (!isRaining && timer <= 0f)
        {
            StartRain();
        }
        else if (isRaining && timer <= 0f)
        {
            StopRain();
        }

        // Smoothly fade spawn rate in/out
        currentSpawnRate = Mathf.MoveTowards(currentSpawnRate, targetSpawnRate, fadeSpeed * Time.deltaTime);
        rainVFX.SetFloat("SpawnRate", currentSpawnRate);
    }

    void StartRain()
    {

        isRaining = true;
        nextDuration = Random.Range(minRainDuration, maxRainDuration);
        timer = nextDuration;
        targetSpawnRate = Random.Range(3000, 15000);
        Debug.Log($"Rain started, will last {nextDuration:F0}s at an amount of {targetSpawnRate}");   
        audioManager.StartRain();
    }

    void StopRain()
    {
        isRaining = false;
        timer = Random.Range(minDryTime, maxDryTime);
        targetSpawnRate = 0f;
        Debug.Log($"Rain stopped, next rain in {timer:F0}s");
        audioManager.StopRain();
    }
} 