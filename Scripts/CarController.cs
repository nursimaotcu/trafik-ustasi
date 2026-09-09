using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float maxSpeed = 27.8f;
    public float acceleration = 5f;
    public float brakeForce = 8f;
    public float turnSpeed = 80f;

    [Header("Durum")]
    public float currentSpeed = 0f;
    public bool isBraking = false;

    [Header("Ses Ayarları")]
    public AudioSource motorSes;
    public AudioSource arkaplanMuzik;
    public AudioClip dogruCevapSes;
    public AudioClip yanlisCevapSes;
    public float minPitch = 0.8f;
    public float maxPitch = 2.0f;

    private Rigidbody rb;
    private bool canMove = true;
    public int score = 40;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.linearDamping = 1f;
        rb.angularDamping = 5f;

        if (arkaplanMuzik != null)
        {
            arkaplanMuzik.loop = true;
            arkaplanMuzik.volume = 0.4f;
            arkaplanMuzik.Play();
        }

        if (motorSes != null)
        {
            motorSes.loop = true;
            motorSes.volume = 0.6f;
        }
    }

    void Update()
    {
        HandleInput();
        UpdateMotorSes();
    }

    void FixedUpdate()
    {
        MoveCarPhysics();
    }

    void UpdateMotorSes()
    {
        if (motorSes == null) return;

        if (Mathf.Abs(currentSpeed) > 0.1f)
        {
            if (!motorSes.isPlaying) motorSes.Play();
            float speedRatio = Mathf.Abs(currentSpeed) / maxSpeed;
            motorSes.pitch = Mathf.Lerp(minPitch, maxPitch, speedRatio);
        }
        else
        {
            if (motorSes.isPlaying) motorSes.Stop();
        }
    }

   public void DogruSesCal()
{
    if (dogruCevapSes != null)
    {
        Debug.Log("Dogru ses caliyor!");
        motorSes.PlayOneShot(dogruCevapSes, 1f);
    }
    else
    {
        Debug.Log("dogruCevapSes NULL!");
    }
}

public void YanlisSesCal()
{
    if (yanlisCevapSes != null)
    {
        motorSes.PlayOneShot(yanlisCevapSes, 1f);
    }
}

    void HandleInput()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        if (!canMove)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, brakeForce * Time.deltaTime);
            return;
        }

        if (vertical > 0)
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed * vertical, acceleration * Time.deltaTime);
        else if (vertical < 0)
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed * vertical * 0.5f, brakeForce * Time.deltaTime);
        else
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, brakeForce * 0.5f * Time.deltaTime);

        if (Mathf.Abs(currentSpeed) > 0.5f)
        {
            float turnAmount = horizontal * turnSpeed * Time.deltaTime;
            if (currentSpeed < 0) turnAmount = -turnAmount;
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, turnAmount, 0f));
        }
    }

    void MoveCarPhysics()
    {
        Vector3 forwardMove = transform.forward * currentSpeed;
        rb.linearVelocity = new Vector3(forwardMove.x, rb.linearVelocity.y, forwardMove.z);
    }

    public void ApplyPenalty(int amount)
    {
        score -= amount;
        if (score <= 0)
        {
            score = 0;
            TrafficUIManager.Instance?.EndGame("Puanın bitti! Oyun bitti.");
        }
    }

    public void SetCanMove(bool value) => canMove = value;
    public float GetSpeedKMH() => currentSpeed * 3.6f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RedLight"))
        {
            TrafficUIManager.Instance?.ShowWarning("Kırmızı ışık ihlali! -10 Puan");
            ApplyPenalty(10);
        }
        if (other.CompareTag("GreenLight"))
            TrafficUIManager.Instance?.HideWarning();

        if (other.CompareTag("SpeedSign"))
        {
            SpeedSign sign = other.GetComponent<SpeedSign>();
            if (sign != null)
            {
                float kmh = GetSpeedKMH();
                TrafficUIManager.Instance?.UpdateSpeedLimit(sign.speedLimit);
                if (kmh > sign.speedLimit)
                {
                    TrafficUIManager.Instance?.ShowWarning($"Hız limitini ({sign.speedLimit} km/h) aşarak geçtin! -10 Puan");
                    ApplyPenalty(10);
                }
            }
        }

        if (other.CompareTag("StopSign"))
        {
            if (currentSpeed > 0.5f)
            {
                TrafficUIManager.Instance?.ShowWarning("DUR tabelasında durmadın! -15 Puan");
                ApplyPenalty(15);
            }
        }
    }
}