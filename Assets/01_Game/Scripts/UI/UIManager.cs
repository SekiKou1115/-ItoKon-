using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static BaseEnum;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    // ---------------------------- SerializeField
    [Header("全般")]
    [SerializeField, Tooltip("キャンバス")] private GameObject _baseCanvas;
    [SerializeField, Tooltip("デフォルト")] private GameObject _defaultUIFrame;

    [SerializeField, Tooltip("タイトル")] private GameObject _titleFrame;
    [SerializeField, Tooltip("ポーズ")] private GameObject _pauseFrame;
    [SerializeField, Tooltip("クリア")] private GameObject _clearFrame;
    [SerializeField, Tooltip("ゲームオーバー")] private GameObject _gameoverFrame;


    [Header("デフォルトUI")]
    [SerializeField, Tooltip("通常時ESCテキスト")] private TextMeshProUGUI _escText;
    [SerializeField, Tooltip("ハート枠UI")] private GameObject _emptyHeartUI;
    [SerializeField, Tooltip("ハートUI")] private GameObject[] _heartImage;
    /* HPが減ったときなど、アニメを使用する際に使う想定
    [SerializeField, Tooltip("Ease")] private Ease _heartEase;
    [SerializeField, Tooltip("変更サイズ")] private float _heartValue;
    [SerializeField, Tooltip("アニメ時間")] private float _heartDuration;
    */
    [SerializeField, Tooltip("スライダーコンポーネント")] private Slider _distanceSlider;
    [SerializeField, Tooltip("プレイヤーの位置")] private Transform _startTransform;
    [SerializeField, Tooltip("ゴールの位置")] private Transform _goalTransform;
    /*
    ミニマップ用
    */

    [Header("UI移動 -全般-")]
    [SerializeField, Tooltip("フェード時マスク画像")] private GameObject _fadeMask;
    [SerializeField, Tooltip("実行時間")] private float _fadeDuration;

    [Header("Unitask&DoTween")]
    [SerializeField] private float _waitTime;
    [SerializeField] private float _duration;
    [SerializeField] private Ease _ease;
    [SerializeField] private Transform[] _positions;
    //[SerializeField] private RectTransform[] _positions;
    [SerializeField] private float _scale;

    // ---------------------------- Field

    //private readonly float FADE_TIME = 2;
    //private readonly float FADE_SCALE = 3;

    private GameState _state;
    private readonly string DEFAULT_MAP = "Player", PAUSE_MAP = "Pause", EVENT_MAP = "Event";

    public bool _isUIMove = false;
    // ---------------------------- Property
    public GameState State { get { return _state; } }
    // HP枠


    // ---------------------------- UnityMessage
    private void Awake()
    {
        Instance = this;
        Cursor.visible = false;
    }

    private async void Start()
    {
        //  初期設定
        DG.Tweening.DOTween.SetTweensCapacity(tweenersCapacity: 5000, sequencesCapacity: 200);

        //  スタート時タスク
        var startTask = StartTask(destroyCancellationToken);
        if (await startTask.SuppressCancellationThrow()) { return; }

    }

    private void Update()
    {
        if (_startTransform != null || _goalTransform != null) // ヌルチェック
            _distanceSlider.value = Vector3.Distance(_startTransform.position, _goalTransform.position);

    }

    // ---------------------------- PublicMethod

    #region ------ TweenTask
    /// <summary>
    /// ディレイ
    /// </summary>
    /// <param name="ct"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public async UniTask DelayTime(float time, CancellationToken ct)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: ct);
    }

    /// <summary>
    /// 移動タスク
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="endValue"></param>
    /// <param name="duration"></param>
    /// <param name="ease"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async UniTask Move
        (GameObject obj
        , Vector3 endValue
        , float duration
        , Ease ease
        , CancellationToken ct)
    {
        _isUIMove = true;
        RayHit(false);
        await obj.transform.DOMove(endValue, duration)
            .SetEase(ease)
            .SetUpdate(true)
            .SetLink(obj)
            .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);
        RayHit(true);
        _isUIMove = false;
    }

    /// <summary>
    /// 拡縮タスク
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="endValue"></param>
    /// <param name="duration"></param>
    /// <param name="ease"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async UniTask ScaleTask
        (GameObject obj
        , float endValue
        , float duration
        , Ease ease
        , CancellationToken ct)
    {
        await obj.transform.DOScale(endValue, duration)
            .SetLink(obj)
            .SetEase(ease)
            .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);
    }

    #endregion

    public async UniTask StateChange(GameState state, CancellationToken ct)
    {
        switch (state)
        {
            case GameState.DEFAULT:
                SetState(_defaultUIFrame, DEFAULT_MAP);

                break;

            case GameState.PAUSE:
                SetState(_pauseFrame, PAUSE_MAP);

                break;

            case GameState.GAMECLEAR:
                Title._isDoneTitle = false;
                _defaultUIFrame.SetActive(false);
                SetState(_clearFrame, EVENT_MAP);

                break;

            case GameState.GAMEOVER:
                SetState(_gameoverFrame, EVENT_MAP);

                break;

            case GameState.EVENTS:
                SetState(_titleFrame, EVENT_MAP);

                break;
        }

        void SetState(GameObject frame, string actionMap)
        {
            frame.SetActive(true);
            _state = state;
            Cursor.visible = false;
            var input = PlayerManager.Instance.GetComponent<PlayerInput>();
            input.SwitchCurrentActionMap(actionMap);
        }
    }

    public async UniTask SceneLoad(int scene)
    {
        RayHit(false);

        var tasks = new List<UniTask>()
        {
            // フェードBGM
            // フェード
        };
        await UniTask.WhenAll(tasks);

        SceneManager.LoadScene(scene);
        RayHit(true);
        Time.timeScale = 1.0f;
    }

    //public async UniTask StateLoad(GameState state)
    //{
    //    RayHit(false);

    //    var tasks = new List<UniTask>()
    //    {
    //        // フェードBGM
    //        // フェード
    //    };
    //    await UniTask.WhenAll(tasks);

    //}

    public async void ApplicationQuit()
    {
        RayHit(false);
        // フェード

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //ゲームシーン終了
#else
        Application.Quit(); //build後にゲームプレイ終了が適用
#endif
    }
    public async void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public async void divOnClear()
    {
        if (!UIManager.Instance._isUIMove)
        {
            var closeTask = OpenClear(destroyCancellationToken);
            if (await closeTask.SuppressCancellationThrow()) { return; }
        }
    }
    private async UniTask OpenClear(CancellationToken ct)
    {
        await UIManager.Instance.StateChange(GameState.GAMECLEAR, ct);
    }

    public async void DivOnOver()
    {
        await UIManager.Instance.SceneLoad(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void HPDraw(int count)
    {
        for (int i = 0; i < _heartImage.Length; i++)
        {
            _heartImage[i].SetActive(!(i >= count));
        }
    }

    // ---------------------------- PrivateMethod

    /// <summary>
    /// 初期化
    /// </summary>
    private async UniTask InitState(CancellationToken ct)
    {
        if (_startTransform != null || _goalTransform != null) // ヌルチェック
            _distanceSlider.maxValue = Vector3.Distance(_startTransform.position, _goalTransform.position);
        _titleFrame.SetActive(true);
        _pauseFrame.SetActive(false);
        _clearFrame.SetActive(false);
        _gameoverFrame.SetActive(false);

        await StateChange(GameState.EVENTS, ct);

    }

    /// <summary>
    /// スタートタスク
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    private async UniTask StartTask(CancellationToken ct)
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            _titleFrame.SetActive(false);
            _gameoverFrame.SetActive(true);
        }
        else
        {
            await InitState(ct);
        }

        /*
        ////  待機
        //await DelayTime(_waitTime * 2, ct);

        ////  移動
        //foreach (var pos in _positions)
        //{
        //    await Move(gameObject, pos.position, _duration, _ease, ct);
        //}

        ////  移動　＆　拡大
        //var allTasks = new List<UniTask>()
        //{
        //    Move(gameObject, _positions[0].position, _duration, _ease, ct),
        //    DelayScaleTask(ct),
        //};
        ////  待機拡大
        //async UniTask DelayScaleTask(CancellationToken ct)
        //{
        //    await DelayTime(_duration / 2, ct);
        //    await ScaleTask(gameObject, _scale, _duration / 2, _ease, ct);
        //}

        //await UniTask.WhenAll(allTasks);
        */
    }

    /// <summary>
    /// 接触判定変更
    /// </summary>
    /// <param name="isActive"></param>
    private void RayHit(bool isActive)
    {
        _baseCanvas.GetComponent<CanvasGroup>().interactable = isActive;
    }

}
