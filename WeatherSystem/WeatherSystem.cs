using UnityEngine;
using System.Collections;

public class WeatherSystem : MonoBehaviour
{
    // Das GameObject, das den Regeneffekt repräsentiert (z. B. ein Particle System)
    public GameObject rainEffect;

    [Header("Abhängigkeiten")]
    // Referenz auf das Tageslicht-System (für Tageslänge etc.)
    public DayNightController dayNightController;

    [Header("Wetter-Einstellungen")]
    [Range(0, 100)]
    // Regenwahrscheinlichkeit in Prozent – wird bei automatischem Wetter verwendet
    public float rainChancePercent = 50f;

    // Interner Zustand: regnet es gerade?
    private bool isRaining = false;

    // Gibt an, ob sich das Wetter im manuellen Modus befindet
    private bool isManualMode = false;

    void Start()
    {
        // Initiales Wetter setzen
        SetWeather(isRaining);

        // Automatischen Wetterwechsel starten
        StartCoroutine(WeatherCycle());
    }

    void Update()
    {
        // Mit Taste X: manuellen Modus aktivieren & Regen umschalten
        if (Input.GetKeyDown(KeyCode.X))
        {
            isRaining = !isRaining;
            isManualMode = true;
            SetWeather(isRaining);
            Debug.Log("MANUELLER Modus: Regen aktiv = " + isRaining);
        }

        // Mit Taste C: zurück in den automatischen Modus
        if (Input.GetKeyDown(KeyCode.C))
        {
            isManualMode = false;
            Debug.Log("Automatischer Wettermodus wieder aktiv");
        }
    }

    // Wendet den Regenzustand auf das Effektobjekt an
    void SetWeather(bool rain)
    {
        isRaining = rain;
        rainEffect.SetActive(rain);
    }

    // Wetterlogik im Hintergrund – regelt automatisches An- und Ausschalten
    IEnumerator WeatherCycle()
    {
        while (true)
        {
            // Tageslänge in Sekunden berechnen
            float dayLengthSeconds = dayNightController.dayDurationInMinutes * 60f;

            // Regen- oder Trockenzeit zwischen 10% und 30% der Tageslänge
            float waitTime = Random.Range(dayLengthSeconds * 0.1f, dayLengthSeconds * 0.3f);

            // So lange warten, bevor Wetter sich ändert
            yield return new WaitForSeconds(waitTime);

            // Nur ändern, wenn NICHT im manuellen Modus
            if (!isManualMode)
            {
                // Entscheidung basierend auf Wahrscheinlichkeit
                bool shouldRain = Random.Range(0f, 100f) < rainChancePercent;
                SetWeather(shouldRain);
                Debug.Log("AUTOMATISCHES Wetter: Regen aktiv = " + shouldRain);
            }
        }
    }
}