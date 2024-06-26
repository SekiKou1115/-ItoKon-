using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using static BaseEnum;

public class Pause : MonoBehaviour
{
    // ---------------------------- SerializeField
    [Header("ポーズ")]
    [SerializeField, Tooltip("ポーズ枠")] private GameObject _pauseFrame;
    [SerializeField, Tooltip("ポーズ時背景")] private GameObject _pauseBack;

    [Header("Unitask&DoTween")]
    [SerializeField] private float _waitTime;
    [SerializeField] private float _duration;
    [SerializeField] private Ease _ease;
    //[SerializeField] private Transform[] _positions;
    [SerializeField] private RectTransform[] _positions;
    [SerializeField] private float _scale;

    // ---------------------------- Field


    // ---------------------------- Property

    // ---------------------------- UnityMessage

    private void Start()
    {

    }

    private void Awake()
    {
    }

    // ---------------------------- PublicMethod

    public async void OnPause()
    {
        if (!UIManager.Instance._isUIMove)
        {
            var openTask = OpenPause(destroyCancellationToken);
            if (await openTask.SuppressCancellationThrow()) { return; }
        }
    }


    public async void OnBack()
    {
        if (!UIManager.Instance._isUIMove)
        {
            
            var closeTask = ClosePause(destroyCancellationToken);
            if (await closeTask.SuppressCancellationThrow()) { return; }
        }
    }

    public async void OnRestart()
    {
       await UIManager.Instance.SceneLoad(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnQuit()
    {
        UIManager.Instance.ApplicationQuit();
    }



    // ---------------------------- PrivateMethod

    private async UniTask OpenPause(CancellationToken ct)
    {
        Cursor.visible = true;
        // 状態遷移
        await UIManager.Instance.StateChange(GameState.PAUSE, ct);
        _pauseBack.SetActive(true);
        Time.timeScale = 0;
        //  移動
        await UIManager.Instance.Move(_pauseFrame, _positions[0].position, _duration, _ease, ct);
    }

    private async UniTask ClosePause(CancellationToken ct)
    {
        Cursor.visible = false;
        // 状態遷移
        await UIManager.Instance.StateChange(GameState.DEFAULT, ct);
        _pauseBack.SetActive(false); 
        Time.timeScale = 1.0f;
        //  移動
        await UIManager.Instance.Move(_pauseFrame, _positions[1].position, _duration, _ease, ct);
        _pauseFrame.SetActive(false);
    }

   
}
