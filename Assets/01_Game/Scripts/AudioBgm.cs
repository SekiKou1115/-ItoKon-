using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BaseEnum;

public class AudioBgm : MonoBehaviour
{
    public static AudioBgm Instance;

    [SerializeField, Tooltip("タイトル")] private AudioSource _bgmTitle;
    [SerializeField, Tooltip("ゲーム中")] private AudioSource _bgmPlay;
    [SerializeField, Tooltip("ゲームクリア")] private AudioSource _bgmGameClear;
    [SerializeField, Tooltip("ゲームオーバー")] private AudioSource _bgmGameOver;

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
            case GameState.DEFAULT: // ゲーム
                if (!_bgmPlay.isPlaying)
                {
                    _bgmTitle?.Stop();
                    _bgmPlay?.Play();
                }
                break;

            case GameState.GAMECLEAR: // クリア
                if (!_bgmGameClear.isPlaying)
                {
                    _bgmPlay?.Stop();
                    _bgmGameClear?.Play();
                }
                break;

            case GameState.GAMEOVER: // オーバー
                _bgmPlay?.Stop();
                _bgmGameOver?.Play();
                break;

            case GameState.EVENTS: // タイトル
                _bgmGameClear?.Stop();
                _bgmGameOver?.Stop();
                _bgmPlay?.Stop();

                _bgmTitle?.Play();
                break;
        }
    }
}
