using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTutorial : MonoBehaviour
{
    [SerializeField] Tutorial _tutorial;
    [SerializeField] private int _iD;
    //[SerializeField] private float _delayTime;

    private void OnTriggerEnter(Collider other)
    {
        _tutorial.OnPlay(_iD);
        this.gameObject.SetActive(false);
    }


}
