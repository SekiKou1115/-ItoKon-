using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class RiverManager : MonoBehaviour
{
    // ---------------------------- SerializeField
    [SerializeField, Tooltip("出現地点")] GameObject _spawnPoint;
    [SerializeField, Tooltip("スポーンのX範囲")] private float _randX = 2;
    [SerializeField, Tooltip("スポーンのZ範囲")] private float _randZ = 2;

    [SerializeField, Tooltip("リスポーンタイム")] private float _waitTime = 2.0f;

    // ---------------------------- Field

    // ---------------------------- Property

    // ---------------------------- UnityMessage

    private void Start()
    {
        _randX *= 0.5f;
        _randZ *= 0.5f;
    }

    // ---------------------------- PublicMethod

    public async void Respawn(Collider other)
    {
        // 触れたやつを一時格納
        Transform _tf = other.GetComponent<Transform>();
        Rigidbody _rb = other.GetComponent<Rigidbody>();

        // 行き先を計算
        Vector3 _pos = _spawnPoint.transform.position;
        _pos.x += Random.Range(-_randX, _randX);
        _pos.z += Random.Range(-_randZ, _randZ);

        // 出現
        var Task = Spawn(_tf, _pos,_rb, destroyCancellationToken);
        if (await Task.SuppressCancellationThrow()) { return; }

        
    }

    // ---------------------------- PrivateMethod


    private async UniTask Spawn(Transform tf,Vector3 pos,Rigidbody rb, CancellationToken ct)
    {
        await UIManager.Instance.DelayTime(_waitTime, ct);
        // パワーリセット
        rb.velocity = Vector3.zero;
        // 移動
        tf.position = pos;
    }
}
