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
        // スケールを変更させない
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
    /// 糸の長さ調整
    /// </summary>
    private void ThreadLengthChange()
    {
        // プレイヤー2人の距離を取得
        var dist = Vector3.Distance(
            _player1.transform.position,
            _player2.transform.position) - 1f;

        // プレイヤー2人の中点の座標に糸の座標を調整
        _thread.transform.position = Vector3.Lerp(
            _player1.transform.position,
            _player2.transform.position,
            0.5f);

        // プレイヤー2人の距離に応じて糸の長さを調整
        _thread.transform.localScale = new Vector3(
            _thread.transform.localScale.x,
            _initialScale * dist,
            _thread.transform.localScale.z);
    }

    /// <summary>
    /// 糸の向き調整
    /// </summary>
    private void ThreadAngleChange()
    {
        // 糸をプレイヤー2の方向に向かせる
        _thread.transform.LookAt(_player2.transform);

        // そこにx軸に90回転させて糸を横にする。
        _thread.transform.rotation *= Quaternion.Euler(90f, 0, 0);
    }
}
