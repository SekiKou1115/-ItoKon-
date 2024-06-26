using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    // ---------------------------- SerializeField
    [Header("ゲームオーバー")]
   
    [SerializeField, Tooltip("エフェクト")] private ParticleSystem _Effect;
   
    [SerializeField, Tooltip("ロゴ")] private GameObject _Logo;
    [SerializeField, Tooltip("UI")] private GameObject _UI;

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

    private async void Start()
    {
        //  スタート時タスク
        var startTask = OpenOver(destroyCancellationToken);
        if (await startTask.SuppressCancellationThrow()) { return; }
    }
    private void Awake()
    {
    }

    // ---------------------------- PublicMethod

    // ---------------------------- PrivateMethod
    private async UniTask OpenOver(CancellationToken ct)
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
            await UIManager.Instance.Move(_Logo, _positions[0].position, _duration, _ease, ct);
            await UIManager.Instance.Move(_UI, _positions[0].position, _duration, _ease, ct);
        }
        await UniTask.WhenAll(tasks);

        //  待機2
        await UIManager.Instance.DelayTime(_waitTime, ct);

        _UI.GetComponent<CanvasGroup>().alpha = 1;
        _UI.GetComponent<CanvasGroup>().interactable = true;

    }
}
