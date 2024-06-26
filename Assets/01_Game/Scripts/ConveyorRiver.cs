using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorRiver : MonoBehaviour
{
    [SerializeField] private float _speed;  //速度
    [SerializeField] private Vector3 _direction; //向き
    [SerializeField] private List<GameObject> _onRiver;

    private void Update()
    {
        for (int i = 0; i <= _onRiver.Count - 1; i++) 
        {
            _onRiver[i].GetComponent<Rigidbody>().velocity = _speed*_direction*Time.deltaTime;
        }

        //　material用
        //float offset = Time.time * _speed;
        //GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, -offset);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _onRiver.Add(collision.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        _onRiver.Remove(collision.gameObject);
    }
}
