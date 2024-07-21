using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField, Tooltip("落ちるまでの時間")] private float _fallDelay = 1f; // Delay 時間
    [SerializeField, Tooltip("消えるまでの時間")] private float _destroyDelay = 2f; // Destroy 時間
    [SerializeField, Tooltip("復活までの時間")] private float _recoveryDelay = 1f; // Recovery 時間
    [SerializeField, Tooltip("接地されてる地面")] private GameObject _ground;

    private Rigidbody _rb;
    private Vector3 _spawnPosition; // 位置とスケール記憶変数
    private bool _isFallen = false;


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = true;
        Physics.IgnoreCollision(
                gameObject.GetComponent<BoxCollider>(),
                _ground.GetComponent<MeshCollider>(),
                true);
    }

    private async void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && !_isFallen)
        {
            _isFallen = true;
            _spawnPosition = gameObject.transform.position; // 現在のPosを保持
            var fallTask = Fall(destroyCancellationToken);
            if (await fallTask.SuppressCancellationThrow()) { return; }
            _isFallen = false;
        }
    }

    private async UniTask Fall(CancellationToken ct)
    {
        // 触ってから落ちるまで
        await UniTask.Delay(TimeSpan.FromSeconds(_fallDelay), cancellationToken: ct);
        _rb.isKinematic = false;

        // 落ち始めてから消えるまで
        await UniTask.Delay(TimeSpan.FromSeconds(_destroyDelay), cancellationToken: ct);
        gameObject.SetActive(false);

        // 復活するまで
        await UniTask.Delay(TimeSpan.FromSeconds(_recoveryDelay), cancellationToken: ct);
        gameObject.SetActive(true);
        _rb.isKinematic = true;
        gameObject.transform.position = _spawnPosition;
    }
}
