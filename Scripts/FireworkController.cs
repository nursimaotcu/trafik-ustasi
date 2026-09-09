using UnityEngine;

public class FireworkController : MonoBehaviour
{
    public ParticleSystem trailSystem; // Trail objesini buraya sürükle
    public ParticleSystem burstSystem; // Burst objesini buraya sürükle
    
    [Header("Ses Ayarları")]
    public AudioClip explosionClip;    // Ses dosyasını (MP3/WAV) doğrudan buraya sürükle
    private AudioSource audioSource;   // Bunu Inspector'da görmene gerek yok, kod halledecek

    private bool hasShot = false;

    void Start()
    {
        // Oyun başladığında arka planda otomatik bir AudioSource oluşturur
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // Sesi 3D yapar (karakterden uzaklığı hissettirir)
    }

    void Update()
    {
        // F tuşuna basıldığında havai fişeği ateşle
        if (Input.GetKeyDown(KeyCode.F))
        {
            LaunchFirework();
        }
    }

    public void LaunchFirework()
    {
        if (!hasShot)
        {
            hasShot = true;
            Invoke("PlayTrail", 0f);    // Önce iz başlar
            Invoke("PlayBurst", 1f);    // 1 saniye sonra patlama
        }
    }

    void PlayTrail()
    {
        if (trailSystem != null) trailSystem.Play();
    }

    void PlayBurst()
    {
        if (burstSystem != null)
    {
        // Çok daha canlı bir renk seçimi
        Color randomColor = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
        
        var mainModule = burstSystem.main;
        // Rengi "MinMaxGradient" olarak atamak Unity'yi tazelemeye zorlar
        mainModule.startColor = new ParticleSystem.MinMaxGradient(randomColor); 

        burstSystem.Clear(); // Eski beyaz parçacıkları temizle
        burstSystem.Play();
    }

        if (audioSource != null && explosionClip != null)
        {
            audioSource.PlayOneShot(explosionClip);
        }

        Invoke("ResetFirework", 2f);
    }

    void ResetFirework()
    {
        hasShot = false;
    }
}