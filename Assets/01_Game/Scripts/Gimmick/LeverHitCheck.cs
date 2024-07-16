using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BaseEnum;

public class LeverHitCheck : MonoBehaviour
{
    // -------------------------------- Field
    private bool _isActive = false;
    private LeverController _parent;
    private int _iD;

    // ---------------------------- Property
    public int ID { /*get { return _iD; }*/ set { _iD = value; } }


    // -------------------------------- UnityMassege
    private void Start()
    {
        _parent = GetComponentInParent<LeverController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Thread"))
        {
            if(!_isActive)
            {
                _isActive = true;
                _parent.CheckPoint(_iD,true);
            }
            else
            {
                _isActive = false;
                _parent.CheckPoint(_iD,false);
            }
        }
    }

}
