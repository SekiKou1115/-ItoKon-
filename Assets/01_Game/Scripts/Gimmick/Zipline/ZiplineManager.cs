using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using static BaseEnum;

public class ZiplineManager : MonoBehaviour
{
    // ---------------------------- SerializeField

    [SerializeField, Tooltip("�v���C���[")] private GameObject[] _players = new GameObject[2];

    /*
    enum ThisPole
    {
        Top = 0,
        Bottom = 1,
    }
    [SerializeField, Tooltip("���t�g�̈ʒu")] private ThisPole _thisPole;
    */
    [SerializeField, Tooltip("�J�n�C�x���g")] private GameObject _Events;
    [SerializeField,Tooltip("������")] private GameObject _lift;
    [SerializeField, Tooltip("�ړ���")] private Transform[] _points = new Transform[2];
    [SerializeField, Tooltip("�|�W�V�����Z�b�g")]private Transform[] _positonSet = new Transform[3];


    // ---------------------------- Field
    //private bool _isActive = false;

    // ---------------------------- Property
    //public bool IsActive { get { return _isActive; } set { _isActive = value; } }

    // ---------------------------- UnityMessage

    private void Start()
    {

    }

    // ---------------------------- PublicMethod

    public async void PlayZipLine()
    {
        //  �X�^�[�g���^�X�N
        var ziplineTask = zipline(destroyCancellationToken);
        if (await ziplineTask.SuppressCancellationThrow())
        { return; }
    }

    // ---------------------------- PrivateMethod

    /// <summary>
    /// �W�b�v���C�����n�߂�Ƃ��̏���
    /// </summary>
    private async UniTask zipline(CancellationToken ct)
    {
        // �g���K�[���I�t
        _Events.SetActive(false);
        // �v���C���[�̃A�N�V�����}�b�v �� Events
        ChangeActionMap("Event");
        // �v���C���[�̎��ʕύX
        _players[0].GetComponent<Rigidbody>().mass = 1;
        _players[1].GetComponent<Rigidbody>().mass = 1;
        // ���𐶐�
        GameObject Lift = Instantiate(_lift, _positonSet[2].position,Quaternion.identity,transform.GetChild(2));
        // �v���C���[���X�^�[�g�|�W�Ɉړ�
        _players[0].transform.position = _positonSet[0].position;
        _players[1].transform.position = _positonSet[1].position;
        await UniTask.Delay(TimeSpan.FromSeconds(0.1f), cancellationToken: ct);

        // �ړ��J�n �� �I��
        await Lift.transform.DOMove(_points[1].position, 5f).SetDelay(2f);

        // �v���C���[�W�����v
        _players[0].GetComponent<Rigidbody>().AddForce(new Vector3(0f, 5f, 5f), ForceMode.VelocityChange);
        _players[1].GetComponent<Rigidbody>().AddForce(new Vector3(0f, 5f, 5f), ForceMode.VelocityChange);
        await UniTask.Delay(TimeSpan.FromSeconds(0.1f), cancellationToken:ct);
        // �����폜
        Destroy(Lift);
        // �v���C���[�̎��ʕύX
        _players[0].GetComponent<Rigidbody>().mass = 100;
        _players[1].GetComponent<Rigidbody>().mass = 100;
        // �v���C���[�̃A�N�V�����}�b�v �� default
        ChangeActionMap("Player");
    }

    /// <summary>
    /// �A�N�V�����}�b�v(PlayerInput)�̐؂�ւ��֐�
    /// </summary>
    /// <param name="actionMap"></param>
    private void ChangeActionMap(string actionMap)
    {
        var input = PlayerManager.Instance.GetComponent<PlayerInput>();
        var input_p1 = _players[0].GetComponent<PlayerInput>();
        var input_p2 = _players[1].GetComponent<PlayerInput>();

        input.SwitchCurrentActionMap(actionMap);
        input_p1.SwitchCurrentActionMap(actionMap);
        input_p2.SwitchCurrentActionMap(actionMap);
    }
}
