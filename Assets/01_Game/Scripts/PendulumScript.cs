using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumScript : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _limit;
    private bool _randomStart = false;
    private float random = 0;

    private void Awake()
    {
        if (_randomStart)
            random = Random.Range(0f,1f);
    }

    private void Update()
    {
        float angle = _limit * Mathf.Sin(Time.time+random*_speed);
        transform.localRotation = Quaternion.Euler(0,0,angle);  
    }

}
