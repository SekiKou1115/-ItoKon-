using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StringGrab : MonoBehaviour
{
    // 切り替えるUIの宣言
    [SerializeField, Tooltip("掴むUI")] private GameObject _uiGrab;
    [SerializeField, Tooltip("離すUI")] private GameObject _uiLet;
    [SerializeField, Tooltip("掴む")] private GameObject _textGrab;
    [SerializeField, Tooltip("離す")] private GameObject _textLet;

    // TextMeshProを参照する型
    private TextMeshProUGUI _targetGrab;
    private TextMeshProUGUI _targetLet;

    // Start is called before the first frame update
    void Start()
    {
        // 画像を消す
        _uiGrab.SetActive(false);
        _uiLet.SetActive(false);

        // 紐のテキストを消す
        _textGrab.SetActive(false);
        _textLet.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // 糸を掴んでいるかの判定
        if (PlayerManager.Instance.IsWait) // 条件式の中に糸をつかんでいるかの判定を入れる
        {
            // ここに実際に使う画像を入れる
            _uiGrab.SetActive(true);
            _uiLet.SetActive(false);

            // 紐のオン・オフ
            _textGrab.SetActive(true);
            _textLet.SetActive(false);
        }
        else
        {
            // ここに実際に使う画像を入れる
            _uiGrab.SetActive(false);
            _uiLet.SetActive(true);

            // 紐のオン・オフ
            _textGrab.SetActive(false);
            _textLet.SetActive(true);
        }
    }
}