using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BaseEnum;

public class AudioBgm : MonoBehaviour
{
    public static AudioBgm Instance;

    [SerializeField, Tooltip("�^�C�g��")] private AudioSource _bgmTitle;
    [SerializeField, Tooltip("�Q�[����")] private AudioSource _bgmPlay;
    [SerializeField, Tooltip("�Q�[���N���A")] private AudioSource _bgmGameClear;
    [SerializeField, Tooltip("�Q�[���I�[�o�[")] private AudioSource _bgmGameOver;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        BgmPlay();
    }

    public void BgmPlay()
    {
        Debug.Log("BGM" + GameManager.Instance.State);
        switch (GameManager.Instance.State)
        {
            case GameState.DEFAULT: // �Q�[��
                if (!_bgmPlay.isPlaying)
                {
                    _bgmTitle?.Stop();
                    _bgmPlay?.Play();
                }
                break;

            case GameState.GAMECLEAR: // �N���A
                if (!_bgmGameClear.isPlaying)
                {
                    _bgmPlay?.Stop();
                    _bgmGameClear?.Play();
                }
                break;

            case GameState.GAMEOVER: // �I�[�o�[
                _bgmPlay?.Stop();
                _bgmGameOver?.Play();
                break;

            case GameState.EVENTS: // �^�C�g��
                _bgmGameClear?.Stop();
                _bgmGameOver?.Stop();
                _bgmPlay?.Stop();

                _bgmTitle?.Play();
                break;
        }
    }
}
