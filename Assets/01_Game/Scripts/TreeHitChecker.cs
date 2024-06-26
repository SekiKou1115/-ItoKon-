using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeHitChecker : MonoBehaviour
{
    // -------------------------------- UnityMassege
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Thread"))
        {
            Destroy(gameObject);
        }
    }
}
