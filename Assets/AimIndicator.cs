using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimIndicator : MonoBehaviour
{
    public GameObject cueBall; // Referensi ke objek bola putih
    public GameObject stick; // Referensi ke objek stick
    public LineRenderer lineRenderer; // Referensi ke komponen LineRenderer
    public float lineLength = 5f; // Panjang garis indikator

    private void Start()
    {
        lineRenderer.positionCount = 2; // Kita butuh 2 titik untuk garis
        lineRenderer.enabled = false; // Sembunyikan garis saat awal
    }

    private void Update()
    {
        // Cek jika tombol Space ditekan
        if (Input.GetKey(KeyCode.Space))
        {
            ShowAimIndicator(); // Tampilkan indikator arah
        }
        else
        {
            HideAimIndicator(); // Sembunyikan indikator arah
        }
    }

    private void ShowAimIndicator()
    {
        // Hitung arah dari stick ke bola putih
        Vector3 directionToCueBall = (cueBall.transform.position - stick.transform.position).normalized;

        // Hitung titik akhir dari garis
        Vector3 lineEndPoint = cueBall.transform.position - directionToCueBall * lineLength;

        // Atur posisi titik pada LineRenderer
        lineRenderer.SetPosition(0, stick.transform.position); // Titik awal di posisi stick
        lineRenderer.SetPosition(1, lineEndPoint); // Titik akhir di posisi yang dihitung

        lineRenderer.enabled = true; // Tampilkan garis
    }

    private void HideAimIndicator()
    {
        lineRenderer.enabled = false; // Sembunyikan garis
    }
}
