using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float fallDelay = 1f; // Delay 
    private Rigidbody rb;
    private bool hasFallen = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && !hasFallen)
        {
            Invoke("Fall", fallDelay);
            hasFallen = true;
        }
    }

    void Fall()
    {
        rb.isKinematic = false; // Enable gravity
    }
}
