using UnityEngine;
using UnityEngine.UI;

public class DayNightSliderUI : MonoBehaviour
{
    public DayNightController dayNightController; // Referenz zum Controller
    public Slider timeSlider;
    public Text timeLabel; // Optional: Anzeige der Uhrzeit

    private bool isChanging = false;

    void Start()
    {
        if (timeSlider != null)
        {
            timeSlider.minValue = 0f;
            timeSlider.maxValue = 24f;
            timeSlider.value = dayNightController.currentTimeOfDay;
            timeSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
    }

    void Update()
    {
        // Synchronisiere Slider mit Spielzeit (aber nur wenn nicht gerade manuell verschoben)
        if (!isChanging && timeSlider != null)
        {
            timeSlider.value = dayNightController.currentTimeOfDay;
        }

        // Optional: Uhrzeit anzeigen
        if (timeLabel != null)
        {
            int hours = Mathf.FloorToInt(dayNightController.currentTimeOfDay);
            int minutes = Mathf.FloorToInt((dayNightController.currentTimeOfDay % 1f) * 60f);
            timeLabel.text = $"{hours:00}:{minutes:00}";
        }
    }

    void OnSliderValueChanged(float value)
    {
        isChanging = true;
        dayNightController.SetTime(value);
        isChanging = false;
    }
}
