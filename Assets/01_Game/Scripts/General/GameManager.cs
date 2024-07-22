using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static BaseEnum;
using static UnityEngine.CullingGroup;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    // ---------------------------- SerializeField

    //[SerializeField, Tooltip("�X���C�_�[�R���|�[�l���g")] private Slider _distanceSlider;
    [SerializeField, Tooltip("�v���C���[")] private GameObject[] _players;
    [SerializeField, Tooltip("�S�[���̈ʒu")] private Transform _goalTransform;
    [SerializeField, Tooltip("�S�[������(�����p)")] private float _goalDist = 2f;

    // ---------------------------- Field

    //private Slider _distanceSlider;
    private GameState _state = GameState.EVENTS;
    private readonly string DEFAULT_MAP = "Player", PAUSE_MAP = "Pause", EVENT_MAP = "Event";

    // ---------------------------- Property
    public GameState State { get { return _state; } /*set { _state = value; } */}


    // ---------------------------- UnityMessage

    private async void Start()
    {
        //  �����ݒ�
        DG.Tweening.DOTween.SetTweensCapacity(tweenersCapacity: 5000, sequencesCapacity: 200);

        //  �X�^�[�g���^�X�N
        var startTask = StartTask(destroyCancellationToken);
        if (await startTask.SuppressCancellationThrow())
        { return; }
    }
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        DistanceCalculation(false);
    }

    // ---------------------------- PublicMethod

    public async UniTask StateChange(GameState state, CancellationToken ct)
    {
        switch (state)
        {
            case GameState.DEFAULT:
                Cursor.visible = false;
                SetState(DEFAULT_MAP);

                break;

            case GameState.PAUSE:
                SetState(PAUSE_MAP);

                break;

            case GameState.GAMECLEAR:
                Title._isDoneTitle = false;
                SetState(EVENT_MAP);

                break;

            case GameState.GAMEOVER:
                SetState(EVENT_MAP);

                break;

            case GameState.EVENTS:
                SetState(EVENT_MAP);

                break;
        }

        void SetState(string actionMap)
        {
            _state = state;

            var input = PlayerManager.Instance.GetComponent<PlayerInput>();
            var input_p1 = _players[0].GetComponent<PlayerInput>();
            var input_p2 = _players[1].GetComponent<PlayerInput>();

            input.SwitchCurrentActionMap(actionMap);
            input_p1.SwitchCurrentActionMap(actionMap);
            input_p2.SwitchCurrentActionMap(actionMap);

            UIManager.Instance.SetFrame();

        }
    }

    // ---------------------------- PrivateMethod

    #region Goal

    /// <summary>
    /// �S�[���̔���p
    /// </summary>
    private void ClearJuge()
    {
        if (UIManager.Instance.DistanceSlider.value < _goalDist)
        {
            UIManager.Instance.divOnClear();
        }
    }

    /// <summary>
    /// �����v�Z 
    /// </summary>
    /// <param name="isStart"></param>
    private void DistanceCalculation(bool isStart)
    {
        Vector3 _playerDistance;

        switch (isStart)
        {
            case true:
                _playerDistance = Vector3.Lerp(_players[0].transform.position, _players[1].transform.position, .5f);
                UIManager.Instance.DistanceSlider.maxValue = Vector3.Distance(_playerDistance, _goalTransform.position);
                break;

            case false:
                _playerDistance = Vector3.Lerp(_players[0].transform.position, _players[1].transform.position, .5f);
                UIManager.Instance.DistanceSlider.value = Vector3.Distance(_playerDistance, _goalTransform.position);

                ClearJuge();
                break;
        }

        _playerDistance = Vector3.Lerp(_players[0].transform.position, _players[1].transform.position, .5f);
        UIManager.Instance.DistanceSlider.value = Vector3.Distance(_playerDistance, _goalTransform.position);
    }

    #endregion

    /// <summary>
    /// �X�^�[�g�^�X�N
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    private async UniTask StartTask(CancellationToken ct)
    {
        if
            (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Cursor.visible = false;
            DistanceCalculation(true);
            await StateChange(GameState.EVENTS, ct);
        }
        else if
            (SceneManager.GetActiveScene().buildIndex == 1)
        {
            Cursor.visible = true;
            await StateChange(GameState.GAMEOVER, ct);

        }

    }
}