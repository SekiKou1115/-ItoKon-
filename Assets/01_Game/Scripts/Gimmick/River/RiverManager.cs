using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RiverManager : MonoBehaviour
{
    // ---------------------------- SerializeField
    [SerializeField, Tooltip("流れのパワー(乗算)")] private float _flowPower = .5f;

    [SerializeField, Tooltip("出現地点")] GameObject _spawnPoint;
    [SerializeField, Tooltip("スポーンのX範囲")] private float _randX = 2;
    [SerializeField, Tooltip("スポーンのZ範囲")] private float _randZ = 2;


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
            // 触れたやつを一時格納
            var _tf = other.GetComponent<Transform>();
            
            // 行き先を計算
            Vector3 Pos = _spawnPoint.transform.position;
            Pos.x += Random.Range(-_randX, _randX);
            Pos.z += Random.Range(-_randZ, _randZ);

            // 出現
            _tf.position = Pos;

            //// 触れたやつを記録
            //GameObject _obj = other.GameObject();
            //Quaternion _qua = other.gameObject.transform.rotation;

            //// 行き先を計算
            //Vector3 Pos = _spawnPoint.transform.position;
            //Pos.x += Random.Range(-_randX, _randX);
            //Pos.z += Random.Range(-_randZ, _randZ);

            //// 削除出現
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
