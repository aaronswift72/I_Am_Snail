using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Cycle Settings")]
    public float dayDurationSeconds = 120f; // how long a full day takes

    [Header("Sun Colors")]
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    [Header("Ambient")]
    public Gradient ambientColor;

    private float timeOfDay = 0.35f; // start at morning (0=midnight, 0.5=noon)

    void Update()
    {
        timeOfDay += Time.deltaTime / dayDurationSeconds;
        if (timeOfDay >= 1f) timeOfDay -= 1f;

        // Rotate sun — 0=midnight top, 0.25=sunrise, 0.5=noon, 0.75=sunset
        transform.localRotation = Quaternion.Euler((timeOfDay * 360f) - 90f, 170f, 0f);

        // Apply sun color and intensity
        GetComponent<Light>().color = sunColor.Evaluate(timeOfDay);
        GetComponent<Light>().intensity = sunIntensity.Evaluate(timeOfDay);

        // Apply ambient lighting
        RenderSettings.ambientLight = ambientColor.Evaluate(timeOfDay);
    }
}