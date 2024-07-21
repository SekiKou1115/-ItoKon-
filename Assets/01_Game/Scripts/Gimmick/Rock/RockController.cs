using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockController : MonoBehaviour
{
    // -------------------------------- SerializeField
    [SerializeField] private GameObject _player1;
    [SerializeField] private GameObject _player2;

    // -------------------------------- PrivateField
    private Rigidbody _rb;

    // -------------------------------- UnityMassege
    private void Start()
    {
        _rb = this.GetComponent<Rigidbody>();
    }

    // -------------------------------- PublicMethod
    public void OnPush()
    {
        var middlePos = Vector3.Lerp(
            _player1.transform.position,
            _player2.transform.position,
            0.5f);

        var dir = -(middlePos - this.transform.position).normalized;

        _rb.AddForce(dir * _rb.mass, ForceMode.Force);
    }
}
