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
    [SerializeField, Tooltip("�o���n�_")] GameObject _spawnPoint;
    [SerializeField, Tooltip("�X�|�[����X�͈�")] private float _randX = 2;
    [SerializeField, Tooltip("�X�|�[����Z�͈�")] private float _randZ = 2;

    [SerializeField, Tooltip("���X�|�[���^�C��")] private float _waitTime = 2.0f;

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
        // �G�ꂽ����ꎞ�i�[
        Transform _tf = other.GetComponent<Transform>();
        Rigidbody _rb = other.GetComponent<Rigidbody>();

        // �s������v�Z
        Vector3 _pos = _spawnPoint.transform.position;
        _pos.x += Random.Range(-_randX, _randX);
        _pos.z += Random.Range(-_randZ, _randZ);

        // �o��
        var Task = Spawn(_tf, _pos,_rb, destroyCancellationToken);
        if (await Task.SuppressCancellationThrow()) { return; }

        
    }

    // ---------------------------- PrivateMethod


    private async UniTask Spawn(Transform tf,Vector3 pos,Rigidbody rb, CancellationToken ct)
    {
        await UIManager.Instance.DelayTime(_waitTime, ct);
        // �p���[���Z�b�g
        rb.velocity = Vector3.zero;
        // �ړ�
        tf.position = pos;
    }
}
