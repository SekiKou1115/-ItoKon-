using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZiplineHitCheck : MonoBehaviour
{
    // ---------------------------- SerializeField
    [SerializeField] private ZiplineManager _ziplineManager;

    // ---------------------------- Field
    private int _flag = 0;

    // ---------------------------- Property

    // ---------------------------- UnityMessage

    private void Start()
    {
        if (!_ziplineManager) _ziplineManager = transform.parent.parent.GetComponent<ZiplineManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("flag++");
            _flag++;
        }
        if (_flag == 2)
        {
            Debug.Log("startZipline");
            // èàóù
            _ziplineManager.PlayZipLine();
            _flag = 0;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player_Groom" || other.name == "Player_Bride")
        {
            Debug.Log("flag--");
            _flag--;
        }
    }

    // ---------------------------- PublicMethod

    // ---------------------------- PrivateMethod

}
