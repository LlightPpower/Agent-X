using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeShoot : MonoBehaviour
{
    public GameObject knifePrefab; // Atýlacak býçak
    public Transform knifeSpawnPoint; // Býçaðýn elden çýkýþ noktasý
    public Transform playerHand; // Karakterin eli
    public float throwSpeed = 20f; // Býçaðýn hýzýný ayarlar

    private Animator animator; // Animator bileþeni

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Týklama veya dokunmayý kontrol eder
        if (Input.GetMouseButtonDown(0)) // Sol týk veya mobilde dokunma
        {
            // Animasyonu tetikler
            animator.SetTrigger("ShootKnife");
        }
    }

    // Animasyonun belirli bir noktasýnda çaðrýlacak fonksiyon
    public void ThrowKnife(Vector3 targetPosition)
    {
        // Býçaðý oluþtur
        GameObject thrownKnife = Instantiate(knifePrefab, knifeSpawnPoint.position, Quaternion.identity);

        // Býçaðý hedefe doðru hareket ettir
        Vector3 direction = (targetPosition - knifeSpawnPoint.position).normalized;
        Rigidbody rb = thrownKnife.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * throwSpeed;
        }

        // Geçici olarak karakterin elindeki býçaðý sakla
        playerHand.gameObject.SetActive(false);

        // Elindeki býçaðý belli bir süre sonra tekrar görünür yap
        Invoke("ShowKnife", 0.5f);
    }

    // Karakterin elindeki býçaðý tekrar görünür yapar
    void ShowKnife()
    {
        playerHand.gameObject.SetActive(true);
    }
}
