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
    public float spinMultiplier = 0.1f; // Faktor pengali untuk kekeran

    private bool isMoving = false; // Menyimpan status pergerakan bola
    private bool isSpacePressed = false; // Menyimpan apakah tombol Space sedang ditekan
    private float currentForce = 0f; // Kekuatan sodokan saat ini
    private bool isCharging = false; // Deklarasi variabel untuk melacak status pengisian

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
            isCharging = true; // Set isCharging menjadi true ketika tombol Space ditekan
            ChargeForce(); // Akumulasi kekuatan sodokan
        }
        else
        {
            isCharging = false; // Set isCharging menjadi false ketika Space tidak ditekan
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

        // Terapkan kekeran berdasarkan kekuatan yang diterapkan
        ApplySpin(currentForce);
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

    // Method untuk menerapkan kekeran pada bola putih
    void ApplySpin(float force)
    {
        // Terapkan torque untuk efek kekeran berdasarkan kekuatan yang diterapkan
        float spinAmount = force * spinMultiplier; // Sesuaikan faktor pengali sesuai kebutuhan
        rb.AddTorque(Vector3.right * spinAmount, ForceMode.Impulse); // Kekeran ke arah sumbu X
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Mengatur bola agar melambat saat bertabrakan
        rb.drag = stopDrag; // Mengatur drag untuk menghentikan bola lebih cepat

        // Randomisasi sedikit arah bola setelah bertabrakan
        Vector3 randomDirection = new Vector3(
            rb.velocity.x + Random.Range(-0.2f, 0.2f),  // Tambah variasi acak pada arah X
            rb.velocity.y,  // Y tetap, karena kita tidak mau arah vertikal berubah
            rb.velocity.z + Random.Range(-0.2f, 0.2f)   // Tambah variasi acak pada arah Z
        );

        // Terapkan kecepatan baru dengan arah acak
        rb.velocity = randomDirection.normalized * rb.velocity.magnitude; // Normalisasi kecepatan agar tetap sama
    }

    // Update method untuk memperlambat bola jika sedang bergerak
    void FixedUpdate()
    {
        if (isMoving)
        {
            // Mengurangi kecepatan bola secara bertahap
            rb.velocity *= 0.98f; // Mengurangi kecepatan bola setiap frame

            // Jika kecepatan bola sudah sangat kecil, hentikan bola
            if (rb.velocity.magnitude < 0.1f)
            {
                rb.velocity = Vector3.zero; // Set kecepatan menjadi nol
                isMoving = false; // Set status bola berhenti
            }
        }
    }
}
