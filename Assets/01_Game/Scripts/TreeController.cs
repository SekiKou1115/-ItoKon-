using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    // ---------------------------- SerializeField
    [SerializeField, Tooltip("ì|ÇÍÇÈäpìx")] private float _angle = 90f;

    // -------------------------------- PrivateField
    private bool _isFallDown = false;
    private Rigidbody _rb;

    // -------------------------------- UnityMassege
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        FallDown();
    }

    // -------------------------------- PrivateMethod
    /// <summary>
    /// ì|ÇÍÇÈèàóù
    /// </summary>
    private void FallDown()
    {
        if (transform.childCount == 0 && !_isFallDown)
        {
            _rb.isKinematic = false;

            // ñÿÇì|Ç∑
            _rb.AddForce(
                transform.forward * _rb.mass,
                ForceMode.Impulse);

            gameObject.GetComponent<CapsuleCollider>().material = null;

            _isFallDown = true;
        }
        else if (_isFallDown && transform.localEulerAngles.x >= _angle)
        {
            _rb.isKinematic = true;

            gameObject.tag = "Ground";
        }
    }
}
