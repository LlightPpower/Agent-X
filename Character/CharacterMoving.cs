using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterMoving : MonoBehaviour
{
    public float speed = 5f; // Hareket h�z�
    private Animator animator; // Animator bile�eni
    private Rigidbody rb; // Rigidbody bile�eni
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
            Debug.LogError("Animator bile�eni bulunamad�!");
        }

        if (rb == null)
        {
            Debug.LogError("Rigidbody bile�eni bulunamad�!");
        }
        heldKnife = knifeSpawnPoint.GetChild(0).gameObject;
        heldKnife.SetActive(true);
    }

    void FixedUpdate()
    {
        // Hareket girdi bilgilerini al
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Hareket vekt�r�n� olu�tur
        Vector3 move = new Vector3(horizontal, 0, vertical).normalized;

        // Hareket et
        if (move.magnitude > 0f)
        {
            rb.MovePosition(rb.position + move * speed * Time.fixedDeltaTime);

            // Karakteri hareket y�n�ne d�nd�r
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
        if (Input.GetMouseButtonDown(0)) // Ekrana dokunuldu�unda
        {
            ThrowKnife();
        }
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
    void ThrowKnife()
    {
        // Animasyonu ba�lat
        animator.SetTrigger("KnifeShoot");

        // Animasyonun belirli bir an�nda b��a�� f�rlat (Event ile kontrol edilir)
    }

    public void OnKnifeThrowEvent() // Animasyona ba�l� bir Event fonksiyonu
    {
        if (heldKnife != null)
        {
            heldKnife.SetActive(false); // Karakterin elindeki b��a�� gizle

            // B��ak prefab'ini olu�tur
            GameObject thrownKnife = Instantiate(knifePrefab, knifeSpawnPoint.position, knifeSpawnPoint.rotation);

            // Rigidbody ile b��a�� f�rlat
            Rigidbody2D rb = thrownKnife.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                targetPosition.z = 0;

                Vector2 throwDirection = (targetPosition - knifeSpawnPoint.position).normalized;
                rb.velocity = throwDirection * throwForce;
            }

            // Belirli bir s�re sonra b��a�� geri getir
            Invoke("ReturnKnife", 2f); // 2 saniye sonra b��a�� geri getir
        }
    }

    void ReturnKnife()
    {
        heldKnife.SetActive(true); // Elindeki b��a�� tekrar g�ster
    }
}