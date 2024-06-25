using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreadManager : MonoBehaviour
{
    // -------------------------------- SerializeField
    [SerializeField] private GameObject _player1;
    [SerializeField] private GameObject _player2;
    [SerializeField] private GameObject _thread;

    // -------------------------------- PrivateField
    private const float _initialScale = 0.5f;

    // -------------------------------- UnityMassege
    private void Start()
    {
        // �X�P�[����ύX�����Ȃ�
        _thread.transform.localScale = new Vector3(
            _thread.transform.localScale.x,
            _initialScale,
            _thread.transform.localScale.z);
    }

    private void Update()
    {
        ThreadLengthChange();

        ThreadAngleChange();
    }

    // -------------------------------- PrivateMethod
    /// <summary>
    /// ���̒�������
    /// </summary>
    private void ThreadLengthChange()
    {
        // �v���C���[2�l�̋������擾
        var dist = Vector3.Distance(
            _player1.transform.position,
            _player2.transform.position) - 1f;

        // �v���C���[2�l�̒��_�̍��W�Ɏ��̍��W�𒲐�
        _thread.transform.position = Vector3.Lerp(
            _player1.transform.position,
            _player2.transform.position,
            0.5f);

        // �v���C���[2�l�̋����ɉ����Ď��̒����𒲐�
        _thread.transform.localScale = new Vector3(
            _thread.transform.localScale.x,
            _initialScale * dist,
            _thread.transform.localScale.z);
    }

    /// <summary>
    /// ���̌�������
    /// </summary>
    private void ThreadAngleChange()
    {
        // �����v���C���[2�̕����Ɍ�������
        _thread.transform.LookAt(_player2.transform);

        // ������x����90��]�����Ď������ɂ���B
        _thread.transform.rotation *= Quaternion.Euler(90f, 0, 0);
    }
}
