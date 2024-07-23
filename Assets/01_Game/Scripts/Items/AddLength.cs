using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddLength : MonoBehaviour
{
    // -------------------------------- SerializeField
    [SerializeField, Tooltip("ëùâ¡ó ")] private float _addValue;

    // -------------------------------- UnityMessage
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            ThreadController.Instance.GetSetMaxDist += _addValue;
            Destroy(this.gameObject);
        }
    }
}
