using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pocket : MonoBehaviour
{
    // Method untuk mendeteksi bola yang masuk ke dalam lubang
    private void OnTriggerEnter(Collider other)
    {
        // Cek apakah objek yang masuk adalah bola
        if (other.CompareTag("Ball"))
        {
            // Menghapus atau menonaktifkan bola yang masuk ke lubang
            other.gameObject.SetActive(false); // Menonaktifkan bola (bola "hilang")
        }
    }
}
