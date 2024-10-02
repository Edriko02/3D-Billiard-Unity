using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    private bool isGameStarted = false;

    // Start is called before the first frame update
    void Start()
    {   
        // Panggil method untuk memulai game
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameStarted)
        {
            // Logika game berjalan
            Debug.Log("Game is running...");
        }
    }

    // Method untuk memulai game
    void StartGame()
    {
        // Set status bahwa game sudah dimulai
        isGameStarted = true;
        Debug.Log("Game has started!");
        
        // Inisialisasi elemen-elemen game (contoh: skor, timer, posisi karakter, dll.)
        InitializeGame();
    }

    // Method untuk inisialisasi game
    void InitializeGame()
    {
        // Contoh: inisialisasi skor, posisi objek, dll.
        Debug.Log("Initializing game elements...");
    }
}
