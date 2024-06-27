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

    [Header("プレイヤー")]
    [SerializeField, Tooltip("操作キャラ名")] private Name _movePlayerName;
    [SerializeField, Tooltip("現在のライフ")] private int _life;
    [SerializeField, Tooltip("最大ライフ")] private int _maxLife;

    [Header("カメラ")]
    [SerializeField, Tooltip("カメラ一覧")] private CinemachineFreeLook[] _freeLookCameraList;
    [SerializeField, Tooltip("非選択時優先度")] private int _unselectedPriority = 0;
    [SerializeField, Tooltip("選択時優先度")] private int _selectedPriority = 10;


    private GameObject[] _player; // 子オブジェクト
    private bool _isWait = true; // 追従待機判断
    private int _currentCamera = 0; // 選択中のバーチャルカメラのインデックス

    public bool IsWait => _isWait;
    public Name MovePlayerName => _movePlayerName;

    /// <summary>
    /// 操作キャラ切り替え
    /// </summary>
    /// <param name="context"></param>
    public void BrideAndGroomSwitch(InputAction.CallbackContext context)
    {
        if ((_freeLookCameraList == null || _freeLookCameraList.Length <= 0) || !context.performed)
            return;

        // 以前のバーチャルカメラを非選択
        var vCamPrev = _freeLookCameraList[_currentCamera];
        vCamPrev.Priority = _unselectedPriority;

        // 追従対象を順番に切り替え
        if (++_currentCamera >= _freeLookCameraList.Length)
            _currentCamera = 0;

        // 次のバーチャルカメラを選択
        var vCamCurrent = _freeLookCameraList[_currentCamera];
        vCamCurrent.Priority = _selectedPriority;

        // 操作キャラ切り替え
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
    /// 待機追従切り替え
    /// </summary>
    /// <param name="context"></param>
    public void WaitFollowUpChange(InputAction.CallbackContext context)
    {
        _isWait = !_isWait;
    }

    /// <summary>
    /// ダメージ
    /// </summary>
    public void Damage()
    {
        _life--;
        UIManager.Instance.HPDraw(_life);
    }


    private void Awake()
    {
        // シングルトーン
        Instance = this;

        // 子オブジェクト取得
        _player = GetChildren(gameObject);

        // ライフ数リセット
        _life = _maxLife;

        // バーチャルカメラが設定されていなければ、何もしない
        if (_freeLookCameraList == null || _freeLookCameraList.Length <= 0)
            return;

        // バーチャルカメラの優先度を初期化
        for (var i = 0; i < _freeLookCameraList.Length; ++i)
        {
            _freeLookCameraList[i].Priority =
                (i == _currentCamera ? _selectedPriority : _unselectedPriority);
        }
    }

    private void Update()
    {
        // ゲームオーバー
        if (_life <= 0 ||
            (_player[0].GetComponent<PlayerController>().IsIncapacitated &&
            _player[1].GetComponent<PlayerController>().IsIncapacitated))
        {
            UIManager.Instance.DivOnOver();
        }
    }

    /// <summary>
    /// 子オブジェクトの取得
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
