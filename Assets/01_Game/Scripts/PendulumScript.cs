using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumScript : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _limit;

    private float _random = 0;

    private void Update()
    {
        float angle = _limit * Mathf.Sin(Time.time + _random * _speed);
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerManager.Instance.Damage();
        }
    }
}
