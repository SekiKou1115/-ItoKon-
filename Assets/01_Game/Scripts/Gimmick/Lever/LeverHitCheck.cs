using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BaseEnum;

public class LeverHitCheck : MonoBehaviour
{
    // -------------------------------- PrivateField
    private bool _isActive = false;
    private LeverController _parent;
    private int _iD;

    // ---------------------------- Property
    public int ID { set { _iD = value; } }

    // -------------------------------- UnityMassege
    private void Start()
    {
        _parent = GetComponentInParent<LeverController>();
    }

    // -------------------------------- PublicMethod
    public void OnChange()
    {
        _isActive = !_isActive;
        _parent.CheckPoint(_iD, _isActive);
    }
}
