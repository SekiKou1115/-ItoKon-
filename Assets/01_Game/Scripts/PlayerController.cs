using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.InputSystem;
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


    private bool _isHitGround; // 地面に触れているか判定
    private Rigidbody _rb;
    private Vector2 _inputMove;
    private float _dropDistance; // 落下距離
    private bool _isIncapacitated; // 行動不能判定

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
            return;

        // 入力値
        _inputMove = context.ReadValue<Vector2>();
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
    public async void Attracted(CancellationToken ct)
    {
        Debug.Log("引き寄せられる");
        await transform.DOMove(_partner.transform.position, 5)
                 .SetLink(gameObject)
                 .SetEase(Ease.OutExpo)
                 .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _isHitGround = false;
        _dropDistance = gameObject.transform.position.y;
        _isIncapacitated = false;
    }

    private void Update()
    {
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
        }
        // 相方に触れたとき
        else if (collision.gameObject.CompareTag("Player"))
        {
            _isIncapacitated = false;
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
    }

    /// <summary>
    /// 落下ダメージ
    /// </summary>
    private void LandingDamage()
    {
        var dis = _dropDistance - gameObject.transform.position.y;
        Debug.Log(dis);
        // ジャンプした位置が
        if (_dropDistance - gameObject.transform.position.y > _maxDropDistance)
        {
            PlayerManager.Instance.Damage();
            PlayerManager.Instance.OnSwitch();
            Attracted(this.destroyCancellationToken);
            _isIncapacitated = true;
        }
    }
}
