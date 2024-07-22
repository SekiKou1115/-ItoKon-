using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static BaseEnum;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEditor.PlayerSettings;
using static UnityEngine.EventSystems.StandaloneInputModule;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Tooltip("�V�Y�V�w")] private PlayerManager.Name _name;
    [SerializeField, Tooltip("�ړ����x")] private float _speed;
    [SerializeField, Tooltip("��]���x")] private float _rotateSpeed;
    [SerializeField, Tooltip("�W�����v�J�n���x")] private float _jumpSpeed;
    [SerializeField, Tooltip("�ő嗎������")] private float _maxDropDistance;
    [SerializeField, Tooltip("����")] private GameObject _partner;

    [Header("Effect")]
    [SerializeField, Tooltip("�ړ�")] private ParticleSystem _moveEffect;
    [SerializeField, Tooltip("��")] private ParticleSystem _recoveryEffect;
    [SerializeField, Tooltip("�s���s�\")] private GameObject _stunEffect;

    [Header("Audio")]
    [SerializeField, Tooltip("�_���[�W")] private UnityEvent _seDamage;
    [SerializeField, Tooltip("���n")] private UnityEvent _seLand;
    [SerializeField, Tooltip("�ړ�")] private AudioSource _seWalkGround;
    [SerializeField, Tooltip("�ړ�")] private AudioSource _seWalkStone;


    private bool _isHitGround; // �n�ʂɐG��Ă��邩����
    private Rigidbody _rb;
    private Vector2 _inputMove;
    private float _dropDistance; // ��������
    private bool _isIncapacitated; // �s���s�\����
    private bool _Attracted; // ���������Ă��邩����

    public PlayerManager.Name Name => _name;
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
        {
            // ���͒l���Z�b�g
            _inputMove = new Vector2();
            return;
        }

        // ���͒l
        _inputMove = context.ReadValue<Vector2>();
        Instantiate(_moveEffect, gameObject.transform);
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

    /// <summary>
    /// �����񂹂���
    /// </summary>
    public async UniTask Attracted(CancellationToken ct)
    {
        _Attracted = true;
        // �v���C���[���m�̓����蔻�菉����
        Physics.IgnoreCollision(
                gameObject.GetComponent<CapsuleCollider>(),
                _partner.GetComponent<CapsuleCollider>(),
                true);

        await transform.DOMove(_partner.transform.position, 5)
                 .SetLink(gameObject)
                 .SetEase(Ease.OutExpo)
                 .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);

        // �v���C���[���m�̓����蔻�菉����
        Physics.IgnoreCollision(
                gameObject.GetComponent<CapsuleCollider>(),
                _partner.GetComponent<CapsuleCollider>(),
                false);
        _Attracted = false;
    }

    /// <summary>
    /// �_���[�W����
    /// </summary>
    public async void Damage()
    {
        // �s���s�\����Ȃ����A�����񂹂��ĂȂ���
        if (!_isIncapacitated && !_Attracted)
        {
            // �_���[�W��
            _seDamage?.Invoke();
            _isIncapacitated = true;
            _stunEffect.SetActive(_isIncapacitated);

            if (PlayerManager.Instance.MovePlayerName == _name)
            {
                PlayerManager.Instance.OnSwitch();
            }
            PlayerManager.Instance.Damage();
            var attractedTask = Attracted(this.destroyCancellationToken);
            if (await attractedTask.SuppressCancellationThrow()) { return; }
        }
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _isHitGround = false;
        _dropDistance = gameObject.transform.position.y;
        _isIncapacitated = false;
        _stunEffect.SetActive(_isIncapacitated);
    }

    private void Update()
    {
        // �L�������ҋ@����Ȃ����A����L�����̎��������鎞
        if ((!PlayerManager.Instance.IsWait ||
            PlayerManager.Instance.MovePlayerName == _name) &&
            !_isIncapacitated)
        {
            // ����L�����̎�
            PlayerMove();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �n�ʂɐG�ꂽ�Ƃ�
        if (collision.gameObject.CompareTag("Ground"))
        {
            // ��������
            LandingDamage();
            if (GameManager.Instance.State == GameState.DEFAULT)
            {
                // ���n��
                _seLand?.Invoke();
            }
        }
        // �����ɐG�ꂽ�Ƃ�
        else if (collision.gameObject.CompareTag("Player") && _isIncapacitated)
        {
            _isIncapacitated = false;
            _stunEffect.SetActive(_isIncapacitated);

            Instantiate(_recoveryEffect, gameObject.transform);
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
            _dropDistance = gameObject.transform.position.y;
            _isHitGround = false;

            // �ړ���
            _seWalkStone?.Stop();
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

        if (moveForward != Vector3.zero &&
            !_seWalkStone.isPlaying &&
            _isHitGround)
        {
            // �ړ���
            _seWalkStone?.Play();
        }
        else if (moveForward == Vector3.zero)
        {
            // �ړ���
            _seWalkStone?.Stop();
        }
    }

    /// <summary>
    /// �����_���[�W
    /// </summary>
    private void LandingDamage()
    {
        var dis = _dropDistance - gameObject.transform.position.y;

        // �W�����v�����ʒu���������ʒu���Ⴂ��
        if (dis > _maxDropDistance)
        {
            Damage();
        }
    }
}
