using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextHide : MonoBehaviour
{
    [SerializeField, Tooltip("操作キャラ")] private GameObject _players;
    [SerializeField, Tooltip("新郎新婦")] private PlayerManager.Name _name;

    // 切り替えるUIの宣言
    [SerializeField, Tooltip("待機")] private GameObject _textWait;
    [SerializeField, Tooltip("追従")] private GameObject _textFollow;
    [SerializeField, Tooltip("行動不能")] private GameObject _textDamage;
    [SerializeField, Tooltip("操作")] private GameObject _textPlay;

    // TextMeshProを参照する型
    private TextMeshProUGUI _targetWait;
    private TextMeshProUGUI _targetFollow;
    private TextMeshProUGUI _targetDamage;
    private TextMeshProUGUI _targetPlay;

    //呼び出し時に実行される関数
    void Start()
    {
        // 追従を消す
        _textFollow.SetActive(false);
        // 行動不能を消す
        _textDamage.SetActive(false);
        // 
    }

    // Update is called once per frame
    void Update()
    {
        // _nameがPlayerManagerのMovePlayerNameと一致するとき
        if (_name == PlayerManager.Instance.MovePlayerName)
        {
            _textPlay.SetActive(true);
            _textWait.SetActive(false);
            _textFollow.SetActive(false);
        }
        // _nameがPlayerManagerのMovePlayerNameと一致しない時
        else if(_name != PlayerManager.Instance.MovePlayerName)
        {
            _textPlay.SetActive(false);

            if(PlayerManager.Instance.IsWait)
            {
                _textWait.SetActive(true);
                _textFollow.SetActive(false);
            }
            else
            {
                _textWait.SetActive(false);
                _textFollow.SetActive(true);
            }
        }

        // 自分が行動不能の時
        if(_players.GetComponent<PlayerController>().IsIncapacitated)
        {
            _textDamage.SetActive(true);
        }
        else
        {
            _textDamage.SetActive(false);
        }

        // 相方が動けない時
        //if (_players[0].GetComponent<PlayerController>().IsIncapacitated ||
        //    _players[1].GetComponent<PlayerController>().IsIncapacitated)
        //{
        //    Debug.Log("行動不能");
        //    _textWait.SetActive(false);
        //    _textFollow.SetActive(false);
        //    _textDamage.SetActive(true);
        //}
        //else if (!_players[0].GetComponent<PlayerController>().IsIncapacitated ||
        //         !_players[1].GetComponent<PlayerController>().IsIncapacitated)
        //{
        //    Debug.Log("行動不能解除");
        //    _textDamage.SetActive(false);
        //}
        ////Eキーが押されたらテキストの表示を切り替える
        //else if (Input.GetKeyDown(KeyCode.E) ||
        //    !PlayerManager.Instance.IsWait)
        //{
        //    Debug.Log("ChangeFollw");
        //    _textWait.SetActive(false);    // _textWaitを見えなくする
        //    _textFollow.SetActive(true);    // _textFollowを見えるように
        //}
        //else if (Input.GetKeyDown(KeyCode.E) ||
        //    PlayerManager.Instance.IsWait)
        //{
        //    Debug.Log("ChangeWait");
        //    _textWait.SetActive(true);     // _textWaitを見えるように
        //    _textFollow.SetActive(false);   // _textFollowを見えなくする
        //}
    }
}
