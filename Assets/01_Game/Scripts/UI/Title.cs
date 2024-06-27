using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static BaseEnum;

public class Title : MonoBehaviour
{
    // ---------------------------- SerializeField
    [Header("タイトル")]
    [SerializeField, Tooltip("ロゴ")] private GameObject _logoFrame;
    [SerializeField, Tooltip("テキスト")] private GameObject _textFrame;

    [Header("Unitask&DoTween")]
    [SerializeField] private float _waitTime;
    [SerializeField] private float _duration;
    [SerializeField] private Ease _ease;
    //[SerializeField] private Transform[] _positions;
    [SerializeField] private RectTransform[] _positions;
    [SerializeField] private float _scale;

    // ---------------------------- Field
    public static bool _isDoneTitle;
    // ---------------------------- Property
    // ---------------------------- UnityMessage

    private async void Start()
    {
        if (!(_isDoneTitle))
        {
            //  スタート時タスク
            var startTask = StartTitle(destroyCancellationToken);
            if (await startTask.SuppressCancellationThrow()) { return; }

        }
        else
        {
            var skipTask = SkipTitle(destroyCancellationToken);
            if (await skipTask.SuppressCancellationThrow()) { return; }
        }
        
    }

    private void Awake()
    {
    }

    // ---------------------------- PublicMethod

    /// <summary>
    /// ボタンが押されたとき
    /// </summary>
    public async void OnPlay()
    {
        var skipTask = SkipTitle(destroyCancellationToken);
        if (await skipTask.SuppressCancellationThrow()) { return; }
    }

    // ---------------------------- PrivateMethod

    /// <summary>
    /// スタート時自動
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    private async UniTask StartTitle(CancellationToken ct)
    {
        var tasks = new List<UniTask>
        {
            // フェード
            DelayMove(ct),

        };
        // 待機移動
        async UniTask DelayMove(CancellationToken ct) 
        {
            await UIManager.Instance.DelayTime(_waitTime, ct);
            await UIManager.Instance.Move(_logoFrame, _positions[0].position, _duration, _ease, ct);
        }
        await UniTask.WhenAll(tasks);

        //  待機2
        await UIManager.Instance.DelayTime(_waitTime, ct);

        _textFrame.GetComponent<CanvasGroup>().alpha = 1;
        _textFrame.GetComponent<CanvasGroup>().interactable = true;

    }

    /// <summary>
    /// ﾀｲﾄﾙ終了処理
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    private async UniTask EndTitle(CancellationToken ct)
    {
        if(!_isDoneTitle)
        {
            //  待機
            await UIManager.Instance.DelayTime(_waitTime, ct);

            var tasks = new List<UniTask>
        {
            // フェード
            DelayMove(ct),

        };
            // 待機移動
            async UniTask DelayMove(CancellationToken ct)
            {
                await UIManager.Instance.DelayTime(_waitTime, ct);
                await UIManager.Instance.Move(_logoFrame, _positions[1].position, _duration, Ease.InBack, ct);
            }
            await UniTask.WhenAll(tasks);

            //  待機
            await UIManager.Instance.DelayTime(_waitTime * 2, ct);
        }

        _isDoneTitle = true;

        // 状態遷移
        await UIManager.Instance.StateChange(GameState.DEFAULT, ct);

    }

    private async UniTask SkipTitle(CancellationToken ct)
    {
        if (!UIManager.Instance._isUIMove)
        {
            _textFrame.GetComponent<CanvasGroup>().alpha = 0;
            _textFrame.GetComponent<CanvasGroup>().interactable = false;
            var endTask = EndTitle(destroyCancellationToken);
            if (await endTask.SuppressCancellationThrow()) { return; }
            this.gameObject.SetActive(false);
        }
    }
}
