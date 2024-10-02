using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickController : MonoBehaviour
{
    public GameObject cueBall; // Referensi ke objek bola putih
    public float moveDistance = 1f; // Jarak maksimum stick bergerak
    public float moveSpeed = 5f; // Kecepatan pergerakan stick

    private Vector3 initialPosition; // Posisi awal stick
    private bool isMoving = false; // Status pergerakan stick

    void Start()
    {
        // Simpan posisi awal stick
        initialPosition = transform.position;
    }

    void Update()
    {
        // Cek jika tombol Space ditekan
        if (Input.GetKey(KeyCode.Space))
        {
            isMoving = true; // Set status bergerak menjadi true
        }
        else
        {
            isMoving = false; // Set status bergerak menjadi false
        }

        // Pindahkan stick
        MoveStick();
    }

    void MoveStick()
    {
        if (isMoving)
        {
            // Menghitung arah dari stick ke bola putih
            Vector3 directionToCueBall = (cueBall.transform.position - transform.position).normalized;

            // Hitung posisi target stick di belakang bola putih
            Vector3 targetPosition = cueBall.transform.position - directionToCueBall * moveDistance;

            // Rotasi stick untuk menghadap ke arah pukulan (ke bola putih)
            transform.rotation = Quaternion.LookRotation(directionToCueBall);

            // Pindahkan stick ke posisi target
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        else
        {
            // Kembali ke posisi awal saat tombol dilepas
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        // Jika stick bergerak, pastikan posisi stick selalu mengikuti bola putih
        if (isMoving)
        {
            Vector3 directionToCueBall = (cueBall.transform.position - transform.position).normalized;
            Vector3 targetPosition = cueBall.transform.position - directionToCueBall * moveDistance; // Hitung posisi target stick
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
        }
    }
}
