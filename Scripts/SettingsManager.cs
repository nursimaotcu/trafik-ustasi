using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsPanel;
    public Toggle musicToggle;
    public Toggle effectToggle;
    public Slider musicSlider;
    public Slider effectSlider;

    private CarController car;

    void Start()
    {
        car = Object.FindFirstObjectByType<CarController>();

        musicSlider.value = 0.4f;
        effectSlider.value = 1f;

        musicToggle.onValueChanged.AddListener(OnMusicToggle);
        effectToggle.onValueChanged.AddListener(OnEffectToggle);
        musicSlider.onValueChanged.AddListener(OnMusicSlider);
        effectSlider.onValueChanged.AddListener(OnEffectSlider);
    }

    void OnMusicToggle(bool isOn)
    {
        if (car != null && car.arkaplanMuzik != null)
            car.arkaplanMuzik.mute = !isOn;
    }

    void OnEffectToggle(bool isOn)
    {
        if (car != null && car.motorSes != null)
            car.motorSes.mute = !isOn;
    }

    void OnMusicSlider(float value)
    {
        if (car != null && car.arkaplanMuzik != null)
            car.arkaplanMuzik.volume = value;
    }

    void OnEffectSlider(float value)
    {
        if (car != null && car.motorSes != null)
            car.motorSes.volume = value;
    }

    public void AyarlariAc()
    {
        settingsPanel.SetActive(true);
    }

    public void GeriDon()
    {
        settingsPanel.SetActive(false);
    }
}