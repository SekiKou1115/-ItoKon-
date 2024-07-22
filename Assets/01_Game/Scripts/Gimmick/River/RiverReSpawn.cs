// Œ»óFã‚ÉÚ‚Á‚Ä‚à’u‚«‹‚è‚É‚³‚ê‚éB

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverReSpawn : MonoBehaviour
{
    // ---------------------------- SerializeField

    // ---------------------------- Field
    [SerializeField] private RiverManager _riverManager;

    // ---------------------------- Property

    // ---------------------------- UnityMessage

    private void Start()
    {
        if(!_riverManager)
        _riverManager = GetComponentInParent<RiverManager>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _riverManager.Respawn(other);
        }
    }

    // ---------------------------- PublicMethod

    // ---------------------------- PrivateMethod

}
