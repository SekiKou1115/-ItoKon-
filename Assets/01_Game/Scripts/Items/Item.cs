using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Item : MonoBehaviour
{
    private float _rotationSpeed = 100f;
    [SerializeField] private GameObject _effectPrefab;
    //[SerializeField] private UnityEvent _onGetSe;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //_onGetSe?.Invoke();
            Instantiate(_effectPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
