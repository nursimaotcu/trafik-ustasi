using UnityEngine;
using TMPro;

public class TrafficUIManager : MonoBehaviour
{
    public static TrafficUIManager Instance;

    [Header("UI Elemanları")]
    public TextMeshProUGUI warningText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI speedLimitText;
    public TextMeshProUGUI scoreText;

    [Header("Oyun Sonu Ekranı")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI gameOverReasonText;

    private CarController car;
    private float warningTimer = 0f;
    private bool gameEnded = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        car = Object.FindFirstObjectByType<CarController>();

        if (warningText)    warningText.gameObject.SetActive(false);
        if (speedLimitText) speedLimitText.text = "Limit: 50 km/h";
        if (gameOverPanel)  gameOverPanel.SetActive(false);

        // Başlangıç skoru göster
        if (scoreText && car != null) scoreText.text = "Puan: " + car.score;
    }

    void Update()
    {
        if (gameEnded || car == null) return;

        if (speedText)
            speedText.text = "Hız: " + Mathf.RoundToInt(car.GetSpeedKMH()) + " km/h";

        if (scoreText)
            scoreText.text = "Puan: " + car.score;

        // Uyarı zamanlayıcısı
        if (warningTimer > 0f)
        {
            warningTimer -= Time.deltaTime;
            if (warningTimer <= 0f) HideWarning();
        }
    }

    public void ShowWarning(string message)
    {
        if (warningText == null) return;
        warningText.text = message;
        warningText.gameObject.SetActive(true);
        warningTimer = 3f;
    }

    public void HideWarning()
    {
        if (warningText != null)
            warningText.gameObject.SetActive(false);
    }

    public void UpdateSpeedLimit(float limit)
    {
        if (speedLimitText)
            speedLimitText.text = "Limit: " + limit + " km/h";
    }

    public void EndGame(string reason)
    {
        if (gameEnded) return;
        gameEnded = true;

        HideWarning();

        // Arabayı durdur
        CarController foundCar = Object.FindFirstObjectByType<CarController>();
        if (foundCar != null)
        {
            foundCar.SetCanMove(false);
            if (finalScoreText) finalScoreText.text = "Skorun: " + foundCar.score;
        }
        else
        {
            if (finalScoreText) finalScoreText.text = "Skorun: 0";
        }

        if (gameOverReasonText) gameOverReasonText.text = reason;
        if (gameOverPanel)      gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    // Oyunu yeniden başlatmak için butona bağlayabilirsin
    public void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}