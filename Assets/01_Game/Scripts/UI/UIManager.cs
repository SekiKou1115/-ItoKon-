using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
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
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    // ---------------------------- SerializeField
    [Header("�S��")]
    [SerializeField, Tooltip("�L�����o�X")] private GameObject _baseCanvas;
    
    [SerializeField, Tooltip("�f�t�H���g")] private GameObject _defaultUIFrame;
    [SerializeField, Tooltip("�^�C�g��")] private GameObject _titleFrame;
    [SerializeField, Tooltip("�|�[�Y")] private  GameObject _pauseFrame;
    [SerializeField, Tooltip("�N���A")] private GameObject _clearFrame;
    [SerializeField, Tooltip("�Q�[���I�[�o�[")] private GameObject _gameoverFrame;
    [SerializeField, Tooltip("�`���[�g���A��")] private GameObject _tutorialFrame;
    [SerializeField, Tooltip("�`���[�g���A��")] private GameObject _tutorialFrame2;// ��

    [Header("�f�t�H���gUI")]
    [SerializeField, Tooltip("�ʏ펞ESC�e�L�X�g")] private TextMeshProUGUI _escText;
    [SerializeField, Tooltip("�n�[�g�gUI")] private GameObject _emptyHeartUI;
    [SerializeField, Tooltip("�n�[�gUI")] private GameObject[] _heartImage;
    /* HP���������Ƃ��ȂǁA�A�j�����g�p����ۂɎg���z��
    [SerializeField, Tooltip("Ease")] private Ease _heartEase;
    [SerializeField, Tooltip("�ύX�T�C�Y")] private float _heartValue;
    [SerializeField, Tooltip("�A�j������")] private float _heartDuration;
    */
    [SerializeField, Tooltip("�X���C�_�[�R���|�[�l���g")] private Slider _distanceSlider;
    //[SerializeField, Tooltip("�v���C���[")] private GameObject[] _players;
    //[SerializeField, Tooltip("�S�[���̈ʒu")] private Transform _goalTransform;
    /*
    �~�j�}�b�v�p
    */

    [Header("UI�ړ� -�S��-")]
    [SerializeField, Tooltip("�t�F�[�h���}�X�N�摜")] private GameObject _fadeMask;
    [SerializeField, Tooltip("���s����")] private float _fadeDuration;

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

    //private static Vector3 _playerDistance;

    //private GameState _state;
    private readonly string DEFAULT_MAP = "Player", PAUSE_MAP = "Pause", EVENT_MAP = "Event";

    public bool _isUIMove = false;
    // ---------------------------- Property
    
    public Slider DistanceSlider { get { return _distanceSlider; } set { _distanceSlider = value; } }



    // ---------------------------- UnityMessage
    private void Awake()
    {
        Instance = this;
        Cursor.visible = false;
    }

    private async void Start()
    {
        
    }

    private void Update()
    {
        
    }



    // ---------------------------- PublicMethod

    #region ------ TweenTask
    /// <summary>
    /// �f�B���C
    /// </summary>
    /// <param name="ct"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public async UniTask DelayTime(float time, CancellationToken ct)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: ct);
    }

    /// <summary>
    /// �ړ��^�X�N
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
            .SetUpdate(true)
            .SetEase(ease)
            .SetUpdate(true)
            .SetLink(obj)
            .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);
        RayHit(true);
        _isUIMove = false;
    }

    /// <summary>
    /// �g�k�^�X�N
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


    public void SetFrame()
    {
        switch(GameManager.Instance.State)
        {
            case GameState.DEFAULT:
                _defaultUIFrame.SetActive(true);
                break;

            case GameState.PAUSE:
                _pauseFrame.SetActive(true);
                break;

            case GameState.GAMECLEAR:
                _defaultUIFrame.SetActive(false);
                _clearFrame.SetActive(true);
                break;

            case GameState.GAMEOVER:
                _gameoverFrame.SetActive(true);
                break;

            case GameState.EVENTS:
                _titleFrame.SetActive(true);
                break;
            case GameState.TUTORIAL:
                _tutorialFrame.SetActive(true);
                break;
            case GameState.TUTORIAL2:// ��
                _tutorialFrame2.SetActive(true);
                break;

        }
        
    }

    public async UniTask SceneLoad(int scene)
    {
        RayHit(false);

        var tasks = new List<UniTask>()
        {
            // �t�F�[�hBGM
            // �t�F�[�h
        };
        await UniTask.WhenAll(tasks);

        SceneManager.LoadScene(scene);
        RayHit(true);
        Time.timeScale = 1.0f;
    }

    public async void ApplicationQuit()
    {
        RayHit(false);
        // �t�F�[�h

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //�Q�[���V�[���I��
#else
        Application.Quit(); //build��ɃQ�[���v���C�I�����K�p
#endif
    }
   

    public async void divOnClear()
    {
        if (!Instance._isUIMove)
        {
            var closeTask = UIManager.Instance.OpenClear(destroyCancellationToken);
            if (await closeTask.SuppressCancellationThrow()) { return; }
        }
    }
    private async UniTask OpenClear(CancellationToken ct)
    {
        await GameManager.Instance.StateChange(GameState.GAMECLEAR, ct);
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
    /// ������
    /// </summary>
    private async UniTask InitState(CancellationToken ct)
    {
       // �t�F�[�h

    }

    /// <summary>
    /// �ڐG����ύX
    /// </summary>
    /// <param name="isActive"></param>
    private void RayHit(bool isActive)
    {
        _baseCanvas.GetComponent<CanvasGroup>().interactable = isActive;
    }

}
