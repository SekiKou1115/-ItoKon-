using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowVectorController : MonoBehaviour
{
    // ---------------------------- SerializeField
    [SerializeField, Tooltip("X方向に流れるパワー")] private float _dicX = 0.5f;
    [SerializeField, Tooltip("Y方向に流れるパワー")] private float _dicY = 0f;
    [SerializeField, Tooltip("Z方向に流れるパワー")] private float _dicZ = 0.5f;


    // ---------------------------- Field

    // ---------------------------- Property

    // ---------------------------- UnityMessage

    private void OnTriggerStay(Collider other)
    {
        RiverFlow(other);
    }

    // ---------------------------- PublicMethod

    // ---------------------------- PrivateMethod

    private void RiverFlow(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            var _rb = other.GetComponent<Rigidbody>();
            Vector3 _force = new Vector3(_dicX, _dicY, _dicZ);

            _rb.AddForce(_force,ForceMode.Impulse);
        }
    }
}
