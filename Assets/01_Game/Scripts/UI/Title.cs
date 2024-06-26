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
    [Header("�^�C�g��")]
    [SerializeField, Tooltip("���S")] private GameObject _logoFrame;
    [SerializeField, Tooltip("�e�L�X�g")] private GameObject _textFrame;

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
        //  �X�^�[�g���^�X�N
        var startTask = StartTitle(destroyCancellationToken);
        if (await startTask.SuppressCancellationThrow()) { return; }
    }

    private void Awake()
    {
    }

    // ---------------------------- PublicMethod

    /// <summary>
    /// �{�^���������ꂽ�Ƃ�
    /// </summary>
    public async void OnPlay()
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

    // ---------------------------- PrivateMethod

    /// <summary>
    /// �X�^�[�g������
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    private async UniTask StartTitle(CancellationToken ct)
    {
        var tasks = new List<UniTask>
        {
            // �t�F�[�h
            DelayMove(ct),

        };
        // �ҋ@�ړ�
        async UniTask DelayMove(CancellationToken ct) 
        {
            await UIManager.Instance.DelayTime(_waitTime, ct);
            await UIManager.Instance.Move(_logoFrame, _positions[0].position, _duration, _ease, ct);
        }
        await UniTask.WhenAll(tasks);

        //  �ҋ@2
        await UIManager.Instance.DelayTime(_waitTime, ct);

        _textFrame.GetComponent<CanvasGroup>().alpha = 1;
        _textFrame.GetComponent<CanvasGroup>().interactable = true;

    }

    /// <summary>
    /// ���ُI������
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    private async UniTask EndTitle(CancellationToken ct)
    {
        //  �ҋ@
        await UIManager.Instance.DelayTime(_waitTime, ct);

        var tasks = new List<UniTask>
        {
            // �t�F�[�h
            DelayMove(ct),

        };
        // �ҋ@�ړ�
        async UniTask DelayMove(CancellationToken ct)
        {
            await UIManager.Instance.DelayTime(_waitTime, ct);
            await UIManager.Instance.Move(_logoFrame, _positions[1].position, _duration, Ease.InBack, ct);
        }
        await UniTask.WhenAll(tasks);

        //  �ҋ@
        await UIManager.Instance.DelayTime(_waitTime * 2, ct);
        // ��ԑJ��
        await UIManager.Instance.StateChange(GameState.DEFAULT, ct);

    }
}
