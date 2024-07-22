using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowVectorController : MonoBehaviour
{
    // ---------------------------- SerializeField
    [SerializeField, Tooltip("X�����ɗ����p���[")] private float _dicX = 0.5f;
    [SerializeField, Tooltip("Y�����ɗ����p���[")] private float _dicY = 0f;
    [SerializeField, Tooltip("Z�����ɗ����p���[")] private float _dicZ = 0.5f;


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
