using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static BaseEnum;

public class Tutorial : MonoBehaviour
{
    // ---------------------------- SerializeField
    [Header("É`ÉÖÅ[ÉgÉäÉAÉã")]
    [SerializeField, Tooltip("ëÄçÏÉKÉCÉhòg")] private GameObject _tutorialFrame;

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

    // ---------------------------- PublicMethod
    
    public async void OnPlay(int Num)
    {
        if (!UIManager.Instance._isUIMove)
        {
            var openTask = OpenTutorial(Num,destroyCancellationToken);
            if (await openTask.SuppressCancellationThrow()) { return; }
        }
    }

    public async void OnClose()
    {
        if (!UIManager.Instance._isUIMove)
        {
            var closeTask = CloseTutorial(destroyCancellationToken);
            if (await closeTask.SuppressCancellationThrow()) { return; }
        }
    }

    // ---------------------------- PrivateMethod

    private async UniTask OpenTutorial(int Num,CancellationToken ct)
    {
        Cursor.visible = true;
        // èÛë‘ëJà⁄
        switch(Num)
        {
            case 0:
                await GameManager.Instance.StateChange(GameState.TUTORIAL, ct);
                break;
            case 1:
                await GameManager.Instance.StateChange(GameState.TUTORIAL2, ct);
                break;
        }
            
        Time.timeScale = 0;
        //  à⁄ìÆ
        await UIManager.Instance.Move(_tutorialFrame, _positions[0].position, _duration, _ease, ct);
    }

    private async UniTask CloseTutorial(CancellationToken ct)
    {
        Cursor.visible = false;
        // èÛë‘ëJà⁄
        await GameManager.Instance.StateChange(GameState.DEFAULT, ct);
        Time.timeScale = 1.0f;
        //  à⁄ìÆ
        await UIManager.Instance.Move(_tutorialFrame, _positions[1].position, _duration, _ease, ct);
        _tutorialFrame.SetActive(false);
    }
}
