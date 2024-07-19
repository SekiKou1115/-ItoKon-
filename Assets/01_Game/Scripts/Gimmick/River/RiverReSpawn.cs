// ����F��ɍڂ��Ă��u������ɂ����B

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverReSpawn : MonoBehaviour
{
    // ---------------------------- SerializeField

    // ---------------------------- Field
    [SerializeField] private RiverManager _parent;

    // ---------------------------- Property

    // ---------------------------- UnityMessage

    private void Start()
    {
        _parent = GetComponentInParent<RiverManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _parent.Respawn(other);
        }
    }

    // ---------------------------- PublicMethod

    // ---------------------------- PrivateMethod

}
