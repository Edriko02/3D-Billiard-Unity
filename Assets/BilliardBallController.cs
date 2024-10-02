using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BilliardBallController : MonoBehaviour
{
    private Rigidbody rb;
    public float stopDrag = 2f; // Hambatan saat bola berhenti
    public float moveDrag = 0.5f; // Hambatan saat bola bergerak
    public float minVelocity = 0.1f; // Kecepatan minimum sebelum bola dianggap berhenti
    public float maxForce = 100f; // Kekuatan maksimum saat bola disodok
    public float chargeSpeed = 50f; // Kecepatan pengisian kekuatan saat tombol spasi ditekan
    private float currentForce = 0f; // Kekuatan sodokan saat ini

    private bool isMoving = false; // Menyimpan status pergerakan bola
    private bool isCharging = false; // Menyimpan status pengisian kekuatan

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = stopDrag; // Mengatur drag awal untuk menghentikan bola lebih cepat
    }

    // Update is called once per frame
    void Update()
    {
        // Cek jika tombol Space ditekan
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isMoving) // Jika bola tidak sedang bergerak, mulai gerakan
            {
                StartMoving();
            }
            else // Jika bola sedang bergerak, reset dan sodok bola lagi
            {
                currentForce = 0f; // Reset kekuatan
                ChargeForce(); // Mulai mengisi kekuatan
            }
        }

        // Jika bola bergerak dengan kecepatan sangat rendah, set bola menjadi tidak bergerak
        if (isMoving && rb.velocity.magnitude < minVelocity)
        {
            rb.Sleep(); // Memastikan bola berhenti sepenuhnya
            rb.velocity = Vector3.zero; // Menghentikan bola sepenuhnya
            isMoving = false; // Bola tidak lagi bergerak
        }
    }

    // Method untuk memulai gerakan bola
    void StartMoving()
    {
        rb.isKinematic = false; // Aktifkan fisika pada bola
        ChargeForce(); // Mulai mengisi kekuatan
    }

    // Method untuk mengakumulasi kekuatan sodokan
    void ChargeForce()
    {
        // Tingkatkan kekuatan sodokan sampai maksimum
        currentForce += chargeSpeed * Time.deltaTime;
        currentForce = Mathf.Clamp(currentForce, 0f, maxForce); // Batasi kekuatan agar tidak melebihi maksimum

        // Terapkan gaya pada bola putih sesuai dengan kekuatan yang diakumulasikan
        rb.AddForce(Vector3.forward * currentForce, ForceMode.Impulse);

        // Jika bola bergerak setelah diberikan gaya, set status menjadi bergerak
        if (currentForce > 0)
        {
            isMoving = true; // Tandai bola sedang bergerak
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Ketika terjadi tabrakan, beri drag untuk memperlambat bola
        rb.drag = stopDrag; // Mengatur drag untuk memperlambat bola
    }

    private void OnCollisionExit(Collision collision)
    {
        // Setelah keluar dari tabrakan, kembalikan drag bola untuk gerakan bebas
        rb.drag = moveDrag;
    }
}
