using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueBallController : MonoBehaviour
{
    private Rigidbody rb;

    // Variabel untuk kekuatan pergerakan bola, dapat diatur dari Inspector
    public float maxForce = 100f; // Kekuatan maksimum sodokan
    public float chargeSpeed = 50f; // Kecepatan pengisian kekuatan saat tombol spasi ditekan
    public float maxSpeed = 10f; // Kecepatan maksimum bola
    public float stopDrag = 2f; // Drag saat bola berhenti
    public float moveDrag = 0.5f; // Drag saat bola bergerak

    private bool isMoving = false; // Menyimpan status pergerakan bola
    private bool isSpacePressed = false; // Menyimpan apakah tombol Space sedang ditekan
    private float currentForce = 0f; // Kekuatan sodokan saat ini

    // Start is called before the first frame update
    void Start()
    {
        // Ambil komponen Rigidbody untuk bola putih
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero; // Pastikan bola diam saat game dimulai
        rb.drag = stopDrag; // Mengatur drag awal untuk mempercepat pelambatan
        rb.isKinematic = true; // Membuat bola tidak terpengaruh oleh physics sebelum tombol ditekan
    }

    // Update is called once per frame
    void Update()
    {
        // Cek jika tombol Space ditekan
        if (Input.GetKey(KeyCode.Space))
        {
            ChargeForce(); // Akumulasi kekuatan sodokan
        }

        // Jika tombol Space dilepaskan dan bola belum bergerak
        if (Input.GetKeyUp(KeyCode.Space) && isSpacePressed && !isMoving)
        {
            rb.isKinematic = false; // Aktifkan physics bola
            rb.drag = moveDrag; // Mengatur drag saat bola bergerak
            isMoving = true; // Set status bola sedang bergerak
            MoveBallForward(); // Gerakkan bola
            currentForce = 0f; // Reset kekuatan setelah bola bergerak
            isSpacePressed = false; // Reset status Space
        }

        // Jika bola sudah berhenti, reset isMoving
        if (isMoving && rb.velocity.magnitude < 0.1f)
        {
            isMoving = false; // Set status bola berhenti
        }
    }

    // Method untuk mengakumulasi kekuatan sodokan
    void ChargeForce()
    {
        // Tingkatkan kekuatan sodokan sampai maksimum
        currentForce += chargeSpeed * Time.deltaTime;
        currentForce = Mathf.Clamp(currentForce, 0f, maxForce); // Batasi kekuatan agar tidak melebihi maksimum
        isSpacePressed = true; // Tandai bahwa tombol Space sedang ditekan
    }

    // Method untuk menggerakkan bola putih ke arah sumbu Z
    void MoveBallForward()
    {
        // Arah dorong ke depan (sumbu Z positif)
        Vector3 direction = Vector3.forward; // Menggunakan arah positif sumbu Z

        // Terapkan gaya pada bola putih sesuai dengan kekuatan yang diakumulasikan
        ApplyForce(direction, currentForce);
    }

    // Method untuk menerapkan gaya ke bola putih
    public void ApplyForce(Vector3 direction, float force)
    {
        // Terapkan gaya ke bola putih
        rb.AddForce(direction * force, ForceMode.Impulse);

        // Batasi kecepatan maksimum bola
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed; // Batasi kecepatan
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Mengatur bola agar melambat saat bertabrakan
        rb.drag = stopDrag; // Mengatur drag untuk menghentikan bola lebih cepat
    }

    private void OnCollisionExit(Collision collision)
    {
        // Reset drag setelah keluar dari tabrakan
        rb.drag = 0f; // Kembalikan drag menjadi nol agar bola tidak melambat lebih lambat
    }
}
