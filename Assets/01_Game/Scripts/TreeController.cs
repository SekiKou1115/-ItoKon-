using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    // -------------------------------- PrivateField
    private bool _isFallDown = false;
    private Rigidbody _rb;
    private GameObject _thread;

    // -------------------------------- UnityMassege
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _thread = GameObject.FindWithTag("Thread");
        Physics.IgnoreCollision(
                _thread.gameObject.GetComponent<CapsuleCollider>(),
                gameObject.GetComponent<CapsuleCollider>(),
                false);
    }

    private void Update()
    {
        FallDown();
    }

    // -------------------------------- PrivateMethod
    private void FallDown()
    {
        if(transform.childCount == 0 && !_isFallDown)
        {
            _rb.isKinematic = false;
            _rb.AddForce(transform.forward, ForceMode.Impulse);
            gameObject.GetComponent<CapsuleCollider>().material = null;
            _isFallDown = true;
        }
        else if(_isFallDown && transform.localEulerAngles.x >= 90f)
        {
            _rb.isKinematic = true;
            gameObject.tag = "Ground";
        }
    }
}
