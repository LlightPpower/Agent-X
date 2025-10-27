using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterMoving : MonoBehaviour
{
    public float speed = 5f; // Hareket hýzý
    private Animator animator; // Animator bileþeni
    private Rigidbody rb; // Rigidbody bileþeni
    public GameObject knifePrefab;
    public Transform knifeSpawnPoint;
    public float throwForce = 10f;
    private GameObject heldKnife;
    public float rotationSpeed = 360f; // Derece/sn
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        if (animator == null)
        {
            Debug.LogError("Animator bileþeni bulunamadý!");
        }

        if (rb == null)
        {
            Debug.LogError("Rigidbody bileþeni bulunamadý!");
        }
        heldKnife = knifeSpawnPoint.GetChild(0).gameObject;
        heldKnife.SetActive(true);
    }

    void FixedUpdate()
    {
        // Hareket girdi bilgilerini al
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Hareket vektörünü oluþtur
        Vector3 move = new Vector3(horizontal, 0, vertical).normalized;

        // Hareket et
        if (move.magnitude > 0f)
        {
            rb.MovePosition(rb.position + move * speed * Time.fixedDeltaTime);

            // Karakteri hareket yönüne döndür
            transform.forward = move;

            // isWalking parametresini true yap
            animator.SetBool("isWalking", true);
        }
        else
        {
            // Hareket yoksa isWalking parametresini false yap
            animator.SetBool("isWalking", false);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Ekrana dokunulduðunda
        {
            ThrowKnife();
        }
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
    void ThrowKnife()
    {
        // Animasyonu baþlat
        animator.SetTrigger("KnifeShoot");

        // Animasyonun belirli bir anýnda býçaðý fýrlat (Event ile kontrol edilir)
    }

    public void OnKnifeThrowEvent() // Animasyona baðlý bir Event fonksiyonu
    {
        if (heldKnife != null)
        {
            heldKnife.SetActive(false); // Karakterin elindeki býçaðý gizle

            // Býçak prefab'ini oluþtur
            GameObject thrownKnife = Instantiate(knifePrefab, knifeSpawnPoint.position, knifeSpawnPoint.rotation);

            // Rigidbody ile býçaðý fýrlat
            Rigidbody2D rb = thrownKnife.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                targetPosition.z = 0;

                Vector2 throwDirection = (targetPosition - knifeSpawnPoint.position).normalized;
                rb.velocity = throwDirection * throwForce;
            }

            // Belirli bir süre sonra býçaðý geri getir
            Invoke("ReturnKnife", 2f); // 2 saniye sonra býçaðý geri getir
        }
    }

    void ReturnKnife()
    {
        heldKnife.SetActive(true); // Elindeki býçaðý tekrar göster
    }
}