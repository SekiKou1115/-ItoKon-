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

    [SerializeField, Tooltip("プレイヤー")] private GameObject[] _players = new GameObject[2];

    /*
    enum ThisPole
    {
        Top = 0,
        Bottom = 1,
    }
    [SerializeField, Tooltip("リフトの位置")] private ThisPole _thisPole;
    */
    [SerializeField, Tooltip("開始イベント")] private GameObject _Events;
    [SerializeField,Tooltip("透明床")] private GameObject _lift;
    [SerializeField, Tooltip("移動先")] private Transform[] _points = new Transform[2];
    [SerializeField, Tooltip("ポジションセット")]private Transform[] _positonSet = new Transform[3];


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
        //  スタート時タスク
        var ziplineTask = zipline(destroyCancellationToken);
        if (await ziplineTask.SuppressCancellationThrow())
        { return; }
    }

    // ---------------------------- PrivateMethod

    /// <summary>
    /// ジップラインを始めるときの処理
    /// </summary>
    private async UniTask zipline(CancellationToken ct)
    {
        // トリガーをオフ
        _Events.SetActive(false);
        // プレイヤーのアクションマップ → Events
        ChangeActionMap("Event");
        // プレイヤーの質量変更
        _players[0].GetComponent<Rigidbody>().mass = 1;
        _players[1].GetComponent<Rigidbody>().mass = 1;
        // 床を生成
        GameObject Lift = Instantiate(_lift, _positonSet[2].position,Quaternion.identity,transform.GetChild(2));
        // プレイヤーをスタートポジに移動
        _players[0].transform.position = _positonSet[0].position;
        _players[1].transform.position = _positonSet[1].position;
        await UniTask.Delay(TimeSpan.FromSeconds(0.1f), cancellationToken: ct);

        // 移動開始 → 終了
        await Lift.transform.DOMove(_points[1].position, 5f).SetDelay(2f);

        // プレイヤージャンプ
        _players[0].GetComponent<Rigidbody>().AddForce(new Vector3(0f, 5f, 5f), ForceMode.VelocityChange);
        _players[1].GetComponent<Rigidbody>().AddForce(new Vector3(0f, 5f, 5f), ForceMode.VelocityChange);
        await UniTask.Delay(TimeSpan.FromSeconds(0.1f), cancellationToken:ct);
        // 床を削除
        Destroy(Lift);
        // プレイヤーの質量変更
        _players[0].GetComponent<Rigidbody>().mass = 100;
        _players[1].GetComponent<Rigidbody>().mass = 100;
        // プレイヤーのアクションマップ → default
        ChangeActionMap("Player");
    }

    /// <summary>
    /// アクションマップ(PlayerInput)の切り替え関数
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
