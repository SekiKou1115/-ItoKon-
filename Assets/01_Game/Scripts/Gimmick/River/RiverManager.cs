using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RiverManager : MonoBehaviour
{
    // ---------------------------- SerializeField
    [SerializeField, Tooltip("����̃p���[(��Z)")] private float _flowPower = .5f;

    [SerializeField, Tooltip("�o���n�_")] GameObject _spawnPoint;
    [SerializeField, Tooltip("�X�|�[����X�͈�")] private float _randX = 2;
    [SerializeField, Tooltip("�X�|�[����Z�͈�")] private float _randZ = 2;


    // ---------------------------- Field

    // ---------------------------- Property

    // ---------------------------- UnityMessage

    private void OnTriggerStay(Collider other)
    {
        RiverFlow(other);
    }

    // ---------------------------- PublicMethod

    public void Respawn(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            // �G�ꂽ����ꎞ�i�[
            var _tf = other.GetComponent<Transform>();
            
            // �s������v�Z
            Vector3 Pos = _spawnPoint.transform.position;
            Pos.x += Random.Range(-_randX, _randX);
            Pos.z += Random.Range(-_randZ, _randZ);

            // �o��
            _tf.position = Pos;

            //// �G�ꂽ����L�^
            //GameObject _obj = other.GameObject();
            //Quaternion _qua = other.gameObject.transform.rotation;

            //// �s������v�Z
            //Vector3 Pos = _spawnPoint.transform.position;
            //Pos.x += Random.Range(-_randX, _randX);
            //Pos.z += Random.Range(-_randZ, _randZ);

            //// �폜�o��
            //Destroy(other.gameObject);
            //Instantiate(_obj, Pos,_qua, transform.parent.parent);

        }
    }

    // ---------------------------- PrivateMethod

    private void RiverFlow(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            var _rb = other.GetComponent<Rigidbody>();

            _rb.AddForce(
                transform.right * _flowPower,
                ForceMode.Impulse);
        }
    }
}
