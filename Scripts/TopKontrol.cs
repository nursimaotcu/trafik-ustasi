using UnityEngine;

public class TopKontrol : MonoBehaviour
{
    public float ziplamaGucu = 7f; // Zıplama yüksekliğini buradan ayarla
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Telefon dokunuşu
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Zipla();
        }

        // Bilgisayar testi (Sol tık veya Boşluk tuşu)
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            Zipla();
        }
    }

    public void Zipla()
    {
        // Mevcut dikey hızı sıfırlıyoruz ki üst üste basınca hız katlanmasın
        Vector3 yeniHiz = rb.linearVelocity;
        yeniHiz.y = 0;
        rb.linearVelocity = yeniHiz;

        // Yukarı doğru güç uygula
        rb.AddForce(Vector3.up * ziplamaGucu, ForceMode.Impulse);
    }
}