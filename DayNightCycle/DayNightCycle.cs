using UnityEngine;

public class DayNightController : MonoBehaviour
{
    [Header("Light Settings")]
    public Light sunLight; // Unser Directional Light!

    [Header("Time Settings")]
    [Range(0, 24)]
    public float currentTimeOfDay = 12f; // Startzeit (Mittag = 12h)
    public float dayDurationInMinutes = 5f; // Echtzeit-Minuten für einen kompletten Tag

    [Header("Orbit Settings")]
    public Vector3 orbitCenter = new Vector3(500f, 0f, 450f); // Mittelpunkt des Sonnenkreises
    public float orbitRadius = 600f; // Radius der Kreisbahn

    private float timeMultiplier;

    void Start()
    {
        SetDayDuration(dayDurationInMinutes);
        sunLight.color = Color.white; // Initial auf Weiß setzen
    }

    void Update()
    {
        UpdateTimeOfDay();
        UpdateSunPositionAndRotation();
        UpdateSunIntensity();
    }

    void UpdateTimeOfDay()
    {
        currentTimeOfDay += Time.deltaTime * timeMultiplier;
        if (currentTimeOfDay >= 24f)
        {
            currentTimeOfDay = 0f;
        }
    }

    void UpdateSunPositionAndRotation()
    {
        float angle = ((currentTimeOfDay - 12f) / 24f) * 360f;
        float radians = angle * Mathf.Deg2Rad;

        float y = Mathf.Cos(radians) * orbitRadius;
        float z = Mathf.Sin(radians) * orbitRadius;

        Vector3 sunPosition = new Vector3(orbitCenter.x, orbitCenter.y + y, orbitCenter.z + z);

        sunLight.transform.position = sunPosition;
        sunLight.transform.rotation = Quaternion.LookRotation(orbitCenter - sunPosition);
    }

    void UpdateSunIntensity()
    {
        float heightAboveCenter = sunLight.transform.position.y - orbitCenter.y;
        float normalizedHeight = Mathf.InverseLerp(-orbitRadius, orbitRadius, heightAboveCenter);

        sunLight.intensity = Mathf.Clamp(normalizedHeight, 0f, 1f) * 1.5f;
    }

    public void SetTime(float hour)
    {
        currentTimeOfDay = Mathf.Clamp(hour, 0f, 24f);
    }

    public void SetDayDuration(float minutes)
    {
        dayDurationInMinutes = minutes;
        timeMultiplier = 24f / (dayDurationInMinutes * 60f);
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        SetDayDuration(dayDurationInMinutes);
        if (sunLight != null)
        {
            sunLight.color = Color.white; // Sicherstellen, dass Licht weiß bleibt
        }
    }
#endif
}
