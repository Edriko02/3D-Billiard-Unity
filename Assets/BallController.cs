using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = 1f; // Atur nilai drag untuk melambatkan bola
    }
}
