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
        transform.localRotation = Quaternion.Euler(0, 90, angle);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("É_ÉÅÅ[ÉW");
            //collision.gameObject.GetComponent<PlayerController>().IsIncapacitated = true;
            PlayerManager.Instance.Damage();
        }
    }
}
