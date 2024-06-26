using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float fallDelay; // Delay 
    [SerializeField] private float destroyDelay = 2f; //Destroy  
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
            Invoke("DestroyPlatform", destroyDelay);
            hasFallen = true;
        }
    }

    void Fall()
    {
        rb.isKinematic = false; // Enable gravity
    }

    void DestroyPlatform()
    {
        Destroy(gameObject);  // Destroy the platform GameObject
    }
}
    