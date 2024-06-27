using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [Header("�J����")]
    [SerializeField, Tooltip("�J�����ꗗ")] private CinemachineFreeLook[] _freeLookCameraList;
    [SerializeField, Tooltip("��I�����D��x")] private int _unselectedPriority = 0;
    [SerializeField, Tooltip("�I�����D��x")] private int _selectedPriority = 10;


    private GameObject[] _player; // �q�I�u�W�F�N�g
    private bool _isWait = true; // �Ǐ]�ҋ@���f
    private int _currentCamera = 0; // �I�𒆂̃o�[�`�����J�����̃C���f�b�N�X

    public bool IsWait => _isWait;
    public Name MovePlayerName => _movePlayerName;

    /// <summary>
    /// ����L�����؂�ւ�
    /// </summary>
    /// <param name="context"></param>
    public void BrideAndGroomSwitch(InputAction.CallbackContext context)
    {
        if ((_freeLookCameraList == null || _freeLookCameraList.Length <= 0) || !context.performed)
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
        }
        else if (_movePlayerName == Name.GROOM)
        {
            _movePlayerName = Name.BRIDE;
        }
    }

    /// <summary>
    /// �ҋ@�Ǐ]�؂�ւ�
    /// </summary>
    /// <param name="context"></param>
    public void WaitFollowUpChange(InputAction.CallbackContext context)
    {
        _isWait = !_isWait;
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
