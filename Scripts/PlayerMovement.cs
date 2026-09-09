using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Animator anim;
    
    [Header("Gelişmiş Efekt Ayarları")]
    public ParticleSystem jumpEffect; // Buraya zıplama efektini sürükle

    public float speed = 5f;
    public float gravity = -30f;
    public float jumpHeight = 3f;

    Vector3 velocity;

    void Update()
    {
        // 1. WASD Girişleri
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(x, 0, z);

        if (move.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            controller.Move(move * speed * Time.deltaTime);
        }

        // 2. ADIM ATMA / YÜRÜME ÇÖZÜMÜ
        if (anim != null)
        {
            // 'Speed' parametresi Animator'da yürüme animasyonunu tetikler
            anim.SetFloat("Speed", move.magnitude);
        }

        // 3. ZIPLAMA (Garantili Çalışması İçin Grounded Kontrolü Esnetildi)
        if (Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            
            if (anim != null)
            {
                anim.SetTrigger("Jump");
            }

            // GELİŞMİŞ EFEKT: Sadece zıplayınca bir kez çalışır
            if (jumpEffect != null)
            {
                jumpEffect.Play();
            }
        }

        // 4. Yerçekimi ve Hareket Uygulama
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Zemine çakılma kontrolü (Sonsuz düşüşü engeller)
        if (transform.position.y < 0.05f)
        {
            Vector3 pos = transform.position;
            pos.y = 0;
            transform.position = pos;
            if(velocity.y < 0) velocity.y = -2f;
        }
    }
}