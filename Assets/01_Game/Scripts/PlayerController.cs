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
    [SerializeField, Tooltip("新郎新婦")] private PlayerManager.Name _name;
    [SerializeField, Tooltip("移動速度")] private float _speed;
    [SerializeField, Tooltip("回転速度")] private float _rotateSpeed;
    [SerializeField, Tooltip("ジャンプ開始速度")] private float _jumpSpeed;
    [SerializeField, Tooltip("最大落下距離")] private float _maxDropDistance;
    [SerializeField, Tooltip("相方")] private GameObject _partner;

    [Header("Effect")]
    [SerializeField, Tooltip("移動")] private ParticleSystem _moveEffect;
    [SerializeField, Tooltip("回復")] private ParticleSystem _recoveryEffect;
    [SerializeField, Tooltip("行動不能")] private GameObject _stunEffect;

    [Header("Audio")]
    [SerializeField, Tooltip("ダメージ")] private UnityEvent _seDamage;
    [SerializeField, Tooltip("着地")] private UnityEvent _seLand;
    [SerializeField, Tooltip("移動")] private AudioSource _seWalkGround;
    [SerializeField, Tooltip("移動")] private AudioSource _seWalkStone;


    private bool _isHitGround; // 地面に触れているか判定
    private Rigidbody _rb;
    private Vector2 _inputMove;
    private float _dropDistance; // 落下距離
    private bool _isIncapacitated; // 行動不能判定
    private bool _Attracted; // 引っ張られているか判定

    public PlayerManager.Name Name => _name;
    public bool IsIncapacitated
    {
        get { return _isIncapacitated; }
        set { _isIncapacitated = value; }
    }

    /// <summary>
    /// プレイヤー移動
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        // 行動不能もしくは、待機の時かつ操作キャラじゃないの時
        if (_isIncapacitated ||
            (PlayerManager.Instance.IsWait &&
            PlayerManager.Instance.MovePlayerName != _name))
        {
            // 入力値リセット
            _inputMove = new Vector2();
            return;
        }

        // 入力値
        _inputMove = context.ReadValue<Vector2>();
        Instantiate(_moveEffect, gameObject.transform);
    }

    /// <summary>
    /// ジャンプ
    /// </summary>
    public void OnJump(InputAction.CallbackContext context)
    {
        // 行動不能もしくは、待機の時かつ操作キャラじゃないの時
        if (_isIncapacitated ||
            (PlayerManager.Instance.IsWait &&
            PlayerManager.Instance.MovePlayerName != _name))
            return;

        // ボタンが押された瞬間かつ着地している時だけ処理
        if (!context.performed)
            return;

        if (_isHitGround)
        {
            _isHitGround = false;
            // ジャンプ
            _rb.AddForce(transform.up * _jumpSpeed,
                ForceMode.VelocityChange);
        }
    }

    /// <summary>
    /// 引き寄せられる
    /// </summary>
    public async UniTask Attracted(CancellationToken ct)
    {
        _Attracted = true;
        // プレイヤー同士の当たり判定初期化
        Physics.IgnoreCollision(
                gameObject.GetComponent<CapsuleCollider>(),
                _partner.GetComponent<CapsuleCollider>(),
                true);

        await transform.DOMove(_partner.transform.position, 5)
                 .SetLink(gameObject)
                 .SetEase(Ease.OutExpo)
                 .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);

        // プレイヤー同士の当たり判定初期化
        Physics.IgnoreCollision(
                gameObject.GetComponent<CapsuleCollider>(),
                _partner.GetComponent<CapsuleCollider>(),
                false);
        _Attracted = false;
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    public async void Damage()
    {
        // 行動不能じゃない時、引き寄せられてない時
        if (!_isIncapacitated && !_Attracted)
        {
            // ダメージ音
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
        // キャラが待機じゃないか、操作キャラの時かつ動ける時
        if ((!PlayerManager.Instance.IsWait ||
            PlayerManager.Instance.MovePlayerName == _name) &&
            !_isIncapacitated)
        {
            // 操作キャラの時
            PlayerMove();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 地面に触れたとき
        if (collision.gameObject.CompareTag("Ground"))
        {
            // 落下処理
            LandingDamage();
            if (GameManager.Instance.State == GameState.DEFAULT)
            {
                // 着地音
                _seLand?.Invoke();
            }
        }
        // 相方に触れたとき
        else if (collision.gameObject.CompareTag("Player") && _isIncapacitated)
        {
            _isIncapacitated = false;
            _stunEffect.SetActive(_isIncapacitated);

            Instantiate(_recoveryEffect, gameObject.transform);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // 地面に触れているとき
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isHitGround = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // 地面から離れたとき
        if (collision.gameObject.CompareTag("Ground"))
        {
            _dropDistance = gameObject.transform.position.y;
            _isHitGround = false;

            // 移動音
            _seWalkStone?.Stop();
        }
    }

    /// <summary>
    /// プレイヤーの動き処理
    /// </summary>
    private void PlayerMove()
    {
        // カメラの方向から、X-Z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        // 方向キーの入力値とカメラの向きから、移動方向を決定
        Vector3 moveForward = cameraForward * _inputMove.y + Camera.main.transform.right * _inputMove.x;

        // 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
        _rb.velocity = moveForward * _speed + new Vector3(0, _rb.velocity.y, 0);

        // キャラクターの向きを進行方向に
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
            // 移動音
            _seWalkStone?.Play();
        }
        else if (moveForward == Vector3.zero)
        {
            // 移動音
            _seWalkStone?.Stop();
        }
    }

    /// <summary>
    /// 落下ダメージ
    /// </summary>
    private void LandingDamage()
    {
        var dis = _dropDistance - gameObject.transform.position.y;

        // ジャンプした位置が落ちた位置より低い時
        if (dis > _maxDropDistance)
        {
            Damage();
        }
    }
}
