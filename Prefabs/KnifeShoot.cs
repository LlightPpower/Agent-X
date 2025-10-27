using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeShoot : MonoBehaviour
{
    public GameObject knifePrefab; // At�lacak b��ak
    public Transform knifeSpawnPoint; // B��a��n elden ��k�� noktas�
    public Transform playerHand; // Karakterin eli
    public float throwSpeed = 20f; // B��a��n h�z�n� ayarlar

    private Animator animator; // Animator bile�eni

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // T�klama veya dokunmay� kontrol eder
        if (Input.GetMouseButtonDown(0)) // Sol t�k veya mobilde dokunma
        {
            // Animasyonu tetikler
            animator.SetTrigger("ShootKnife");
        }
    }

    // Animasyonun belirli bir noktas�nda �a�r�lacak fonksiyon
    public void ThrowKnife(Vector3 targetPosition)
    {
        // B��a�� olu�tur
        GameObject thrownKnife = Instantiate(knifePrefab, knifeSpawnPoint.position, Quaternion.identity);

        // B��a�� hedefe do�ru hareket ettir
        Vector3 direction = (targetPosition - knifeSpawnPoint.position).normalized;
        Rigidbody rb = thrownKnife.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * throwSpeed;
        }

        // Ge�ici olarak karakterin elindeki b��a�� sakla
        playerHand.gameObject.SetActive(false);

        // Elindeki b��a�� belli bir s�re sonra tekrar g�r�n�r yap
        Invoke("ShowKnife", 0.5f);
    }

    // Karakterin elindeki b��a�� tekrar g�r�n�r yapar
    void ShowKnife()
    {
        playerHand.gameObject.SetActive(true);
    }
}
