using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.EventSystems.StandaloneInputModule;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Tooltip("�V�Y�V�w")] private PlayerManager.Name _name;
    [SerializeField, Tooltip("�ړ����x")] private float _speed;
    [SerializeField, Tooltip("��]���x")] private float _rotateSpeed;
    [SerializeField, Tooltip("�W�����v�J�n���x")] private float _jumpSpeed;
    [SerializeField, Tooltip("�ő嗎������")] private float _maxDropDistance;


    private bool _isHitGround; // �n�ʂɐG��Ă��邩����
    Rigidbody _rb;
    private Vector2 _inputMove;
    private float _dropDistance; // ��������
    private bool _isIncapacitated; // �s���s�\����

    public bool IsIncapacitated
    {
        get { return _isIncapacitated; }
        set { _isIncapacitated = value; }
    }

    /// <summary>
    /// �v���C���[�ړ�
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        // �s���s�\�������́A�ҋ@�̎�������L��������Ȃ��̎�
        if (_isIncapacitated ||
            (PlayerManager.Instance.IsWait &&
            PlayerManager.Instance.MovePlayerName != _name))
            return;

        // ���͒l
        _inputMove = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// �W�����v
    /// </summary>
    public void OnJump(InputAction.CallbackContext context)
    {
        // �s���s�\�������́A�ҋ@�̎�������L��������Ȃ��̎�
        if (_isIncapacitated ||
            (PlayerManager.Instance.IsWait &&
            PlayerManager.Instance.MovePlayerName != _name))
            return;

        // �{�^���������ꂽ�u�Ԃ����n���Ă��鎞��������
        if (!context.performed)
            return;

        if (_isHitGround)
        {
            _isHitGround = false;
            // �W�����v
            _rb.AddForce(transform.up * _jumpSpeed,
                ForceMode.VelocityChange);
        }
    }


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _isHitGround = false;
        _dropDistance = 0;
        _isIncapacitated = false;
    }

    private void Update()
    {
        if ((!PlayerManager.Instance.IsWait ||
            PlayerManager.Instance.MovePlayerName == _name) &&
            !_isIncapacitated)
        {
            // ����L�����̎�
            PlayerMove();
        }

        // ��������
        LandingDamage();
    }


    private void OnCollisionEnter(Collision collision)
    {
        // �n�ʂɐG�ꂽ�Ƃ�
        if (collision.gameObject.CompareTag("Ground"))
        {
            // �������Ԃ��_���[�W�l�𖞂�������
            if (_dropDistance >= _maxDropDistance)
            {
                _dropDistance = 0;
                _isIncapacitated = true;
                PlayerManager.Instance.Damage();
            }
        }
        // �����ɐG�ꂽ�Ƃ�
        else if (collision.gameObject.CompareTag("Player"))
        {
            _isIncapacitated = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // �n�ʂɐG��Ă���Ƃ�
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isHitGround = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // �n�ʂ��痣�ꂽ�Ƃ�
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isHitGround = false;
        }
    }

    /// <summary>
    /// �v���C���[�̓�������
    /// </summary>
    private void PlayerMove()
    {
        // �J�����̕�������AX-Z���ʂ̒P�ʃx�N�g�����擾
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        // �����L�[�̓��͒l�ƃJ�����̌�������A�ړ�����������
        Vector3 moveForward = cameraForward * _inputMove.y + Camera.main.transform.right * _inputMove.x;

        // �ړ������ɃX�s�[�h���|����B�W�����v�◎��������ꍇ�́A�ʓrY�������̑��x�x�N�g���𑫂��B
        _rb.velocity = moveForward * _speed + new Vector3(0, _rb.velocity.y, 0);

        // �L�����N�^�[�̌�����i�s������
        if (moveForward != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveForward);
            gameObject.transform.rotation
                = Quaternion.RotateTowards(transform.rotation,
                targetRotation,
                _rotateSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// �����_���[�W
    /// </summary>
    private void LandingDamage()
    {
        // �������Ԍv��
        if (!_isHitGround)
        {
            _dropDistance += Time.deltaTime;
        }
        else
        {
            _dropDistance = 0;
        }
    }
}
