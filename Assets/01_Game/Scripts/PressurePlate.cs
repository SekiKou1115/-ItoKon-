using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Is Pressed");
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Is Release");
    }
}
