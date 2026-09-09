using UnityEngine;

public class araba : MonoBehaviour
{
    [Header("Tekerlek Tanımları")]
    public WheelCollider onSol, onSag, arkaSol, arkaSag;

    [Header("Ayarlar")]
    public float motorGucu = 8000f;
    public float donusAcisi = 45f;
    public float frenGucu = 5000f;

    private Rigidbody rb;

    // Dokunmatik buton durumları
    private float dikey = 0f;
    private float yatay = 0f;
    private bool frenBasili = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = 1200f;
        rb.linearDamping = 0.05f;
        rb.angularDamping = 0.05f;
        rb.centerOfMass = new Vector3(0, -0.7f, 0);
    }

    void FixedUpdate()
    {
        // Klavye + dokunmatik ikisi birden çalışır
        float toplamDikey = dikey + Input.GetAxis("Vertical");
        float toplamYatay = yatay + Input.GetAxis("Horizontal");

        toplamDikey = Mathf.Clamp(toplamDikey, -1f, 1f);
        toplamYatay = Mathf.Clamp(toplamYatay, -1f, 1f);

        arkaSol.motorTorque = toplamDikey * motorGucu;
        arkaSag.motorTorque = toplamDikey * motorGucu;

        onSol.steerAngle = toplamYatay * donusAcisi;
        onSag.steerAngle = toplamYatay * donusAcisi;

        if (frenBasili || Input.GetKey(KeyCode.Space))
            ApplyBrakes(frenGucu);
        else
            ApplyBrakes(0);
    }

    void ApplyBrakes(float kuvvet)
    {
        onSol.brakeTorque = kuvvet;
        onSag.brakeTorque = kuvvet;
        arkaSol.brakeTorque = kuvvet;
        arkaSag.brakeTorque = kuvvet;
    }

    // ——— Buton Fonksiyonları (Event Trigger'a bağlanacak) ———

    public void GazBas()   { dikey =  1f; }
    public void GazBirak() { dikey =  0f; }

    public void GeriBas()   { dikey = -1f; }
    public void GeriBirak() { dikey =  0f; }

    public void SolBas()   { yatay = -1f; }
    public void SolBirak() { yatay =  0f; }

    public void SagBas()   { yatay =  1f; }
    public void SagBirak() { yatay =  0f; }

    public void FrenBas()   { frenBasili = true;  }
    public void FrenBirak() { frenBasili = false; }
}