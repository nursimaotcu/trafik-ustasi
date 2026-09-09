using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    [Header("Işık Süreleri (saniye)")]
    public float greenDuration = 5f;
    public float yellowDuration = 2f;
    public float redDuration = 5f;

    [Header("Işık Objeleri")]
    public GameObject redLight;
    public GameObject yellowLight;
    public GameObject greenLight;

    [Header("Durdurma Alanı")]
    public Collider stopZone;

    public enum LightState { Red, Yellow, Green }
    public LightState currentState = LightState.Green;

    private float timer;

    void Start()
    {
        timer = greenDuration;
        SetState(LightState.Green);
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            switch (currentState)
            {
                case LightState.Green:  SetState(LightState.Yellow); break;
                case LightState.Yellow: SetState(LightState.Red);    break;
                case LightState.Red:    SetState(LightState.Green);  break;
            }
        }
    }

    void SetState(LightState newState)
    {
        currentState = newState;

        if (redLight)    redLight.SetActive(false);
        if (yellowLight) yellowLight.SetActive(false);
        if (greenLight)  greenLight.SetActive(false);

        switch (newState)
        {
            case LightState.Red:
                timer = redDuration;
                if (redLight) redLight.SetActive(true);
                if (stopZone) stopZone.tag = "RedLight";
                break;

            case LightState.Yellow:
                timer = yellowDuration;
                if (yellowLight) yellowLight.SetActive(true);
                if (stopZone) stopZone.tag = "RedLight";
                break;

            case LightState.Green:
                timer = greenDuration;
                if (greenLight) greenLight.SetActive(true);
                if (stopZone) stopZone.tag = "GreenLight";
                break;
        }
    }
}