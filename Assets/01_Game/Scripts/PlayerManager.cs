using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public enum Name
    {
        GROOM = 0,
        BRIDE = 1,
    }

    [Header("�v���C���[")]
    [SerializeField, Tooltip("����L������")] private Name _movePlayerName;
    [SerializeField, Tooltip("���݂̃��C�t")] private int _life;
    [SerializeField, Tooltip("�ő僉�C�t")] private int _maxLife;
    [SerializeField, Tooltip("�����񂹎n�߂鋗��")] private float _maxAttract;
    [SerializeField, Tooltip("�ҋ@���̓����C")] private float _waitDynFriction;
    [SerializeField, Tooltip("�s�����̓����C")] private float _moveDynFriction;

    [Header("�J����")]
    [SerializeField, Tooltip("�J�����ꗗ")] private CinemachineFreeLook[] _freeLookCameraList;
    [SerializeField, Tooltip("��I�����D��x")] private int _unselectedPriority = 0;
    [SerializeField, Tooltip("�I�����D��x")] private int _selectedPriority = 10;

    [Header("Audio")]
    [SerializeField, Tooltip("�؂�ւ�")] private UnityEvent _seChange;
    [SerializeField, Tooltip("������")] private UnityEvent _sePull;

    private GameObject[] _player; // �q�I�u�W�F�N�g
    private bool _isWait = true; // �Ǐ]�ҋ@���f
    private int _currentCamera = 0; // �I�𒆂̃o�[�`�����J�����̃C���f�b�N�X
    private Collider _coll; // �R���C�_�[�̃}�e���A���ς���悤

    public bool IsWait => _isWait;
    public Name MovePlayerName
    {
        get { return _movePlayerName; }
        set { _movePlayerName = value; }
    }

    /// <summary>
    /// ����L�����؂�ւ�
    /// </summary>
    /// <param name="context"></param>
    public void BrideAndGroomSwitch(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        OnSwitch();
    }
    public void OnSwitch()
    {
        if ((_freeLookCameraList == null || _freeLookCameraList.Length <= 0))
            return;

        // �ȑO�̃o�[�`�����J�������I��
        var vCamPrev = _freeLookCameraList[_currentCamera];
        vCamPrev.Priority = _unselectedPriority;

        // �Ǐ]�Ώۂ����Ԃɐ؂�ւ�
        if (++_currentCamera >= _freeLookCameraList.Length)
            _currentCamera = 0;

        // ���̃o�[�`�����J������I��
        var vCamCurrent = _freeLookCameraList[_currentCamera];
        vCamCurrent.Priority = _selectedPriority;

        // ����L�����؂�ւ�
        if (_movePlayerName == Name.BRIDE)
        {
            _movePlayerName = Name.GROOM;

            // �s���J�n����Ƃ����C�͂�����
            _coll = _player[0].GetComponent<Collider>();
            _coll.material.dynamicFriction = _moveDynFriction;
            // �ҋ@�ɂȂ�Ƃ����C�͂�����
            _coll = _player[1].GetComponent<Collider>();
            _coll.material.dynamicFriction = _waitDynFriction;
        }
        else if (_movePlayerName == Name.GROOM)
        {
            _movePlayerName = Name.BRIDE;

            // �s���J�n����Ƃ����C�͂�����
            _coll = _player[1].GetComponent<Collider>();
            _coll.material.dynamicFriction = _moveDynFriction;
            // �ҋ@�ɂȂ�Ƃ����C�͂�����
            _coll = _player[0].GetComponent<Collider>();
            _coll.material.dynamicFriction = _waitDynFriction;
        }

        // �؂�ւ���
        _seChange?.Invoke();
    }

    /// <summary>
    /// �ҋ@�Ǐ]�؂�ւ�
    /// </summary>
    /// <param name="context"></param>
    public void WaitFollowUpChange(InputAction.CallbackContext context)
    {
        _isWait = !_isWait;
        Attract();
    }

    /// <summary>
    /// �_���[�W
    /// </summary>
    public void Damage()
    {
        _life--;
        UIManager.Instance.HPDraw(_life);
    }


    private void Awake()
    {
        // �V���O���g�[��
        Instance = this;

        // �q�I�u�W�F�N�g�擾
        _player = GetChildren(gameObject);
        // �v���C���[���m�̓����蔻�菉����
        Physics.IgnoreCollision(
                _player[0].GetComponent<CapsuleCollider>(),
                _player[1].GetComponent<CapsuleCollider>(),
                false);

        // ���C�t�����Z�b�g
        _life = _maxLife;

        // �o�[�`�����J�������ݒ肳��Ă��Ȃ���΁A�������Ȃ�
        if (_freeLookCameraList == null || _freeLookCameraList.Length <= 0)
            return;

        // �o�[�`�����J�����̗D��x��������
        for (var i = 0; i < _freeLookCameraList.Length; ++i)
        {
            _freeLookCameraList[i].Priority =
                (i == _currentCamera ? _selectedPriority : _unselectedPriority);
        }
    }

    private void Start()
    {
        // �s���J�n����Ƃ����C�͂�����
        _coll = _player[0].GetComponent<Collider>();
        _coll.material.dynamicFriction = _moveDynFriction;
        // �ҋ@�ɂȂ�Ƃ����C�͂�����
        _coll = _player[1].GetComponent<Collider>();
        _coll.material.dynamicFriction = _waitDynFriction;
    }

    private void Update()
    {
        // �Q�[���I�[�o�[
        if (_life <= 0 ||
            (_player[0].GetComponent<PlayerController>().IsIncapacitated &&
            _player[1].GetComponent<PlayerController>().IsIncapacitated))
        {
            UIManager.Instance.DivOnOver();
        }
    }

    /// <summary>
    /// �����񂹂�
    /// </summary>
    private async void Attract()
    {
        var distance = Vector3.Distance(_player[0].transform.position, _player[1].transform.position);

        if (distance >= _maxAttract)
        {
            foreach (var obj in _player)
            {
                if (obj.GetComponent<PlayerController>().Name != _movePlayerName)
                {
                    // �����񂹉�
                    _sePull?.Invoke();

                    var attractedTask = obj.GetComponent<PlayerController>().Attracted(this.destroyCancellationToken);
                    if (await attractedTask.SuppressCancellationThrow()) { return; }
                    break;
                }
            }
        }
    }


    /// <summary>
    /// �q�I�u�W�F�N�g�̎擾
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    private static GameObject[] GetChildren(GameObject parent)
    {
        var children = new GameObject[parent.transform.childCount];
        for (var i = 0; i < children.Length; ++i)
        {
            children[i] = parent.transform.GetChild(i).gameObject;
        }
        return children;
    }
}
