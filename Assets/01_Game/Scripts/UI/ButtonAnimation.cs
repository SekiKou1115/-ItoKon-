using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour
{
    // ---------------------------- SerializeField
    [Header("基礎パラメータ")]
    [SerializeField, Tooltip("テキスト")] private TextMeshProUGUI _text;
    [SerializeField, Tooltip("テキスト移動位置")] private float _pressedTextPos;
    [SerializeField, Tooltip("イメージ")] private Image _image;

    [Header("ノーマル")]
    [SerializeField] private Sprite _normalImage;
    [SerializeField] private Color _normalColor;

    [Header("ハイライト")]
    [SerializeField] private Sprite _highlightImage;
    [SerializeField] private Color _highlightColor;
    [SerializeField] private UnityEvent _highlightClip;

    [Header("プレス")]
    [SerializeField] private Sprite _pressedImage;
    [SerializeField] private Color _pressedColor;
    [SerializeField] private UnityEvent _pressedClip;

    [Header("セレクト")]
    [SerializeField] private Color _selectedColor;
    // ---------------------------- Field
    private bool _isPlayPressClip;
    private Vector3 _initTextPos;

    // ---------------------------- UnityMessage
    private void Start()
    {
        _initTextPos = _text.GetComponent<RectTransform>().anchoredPosition;
    }


    // ---------------------------- PublicMethod
    #region ------ ButtonLayer
    /// <summary>
    /// ノーマル
    /// </summary>
    public void Normal()
    {
        UpdateAnimation(
            _normalColor, _initTextPos,
            null,
            _normalImage);

        _isPlayPressClip = false;
    }

    /// <summary>
    /// ハイライト
    /// </summary>
    public void Highlight()
    {
        UpdateAnimation(
            _highlightColor, _initTextPos,
            _highlightClip,
            _highlightImage);

        Debug.Log("AAA");
    }

    /// <summary>
    /// プレス
    /// </summary>
    public void Pressed()
    {
        UpdateAnimation(
            _pressedColor, new Vector3(_initTextPos.x, _pressedTextPos, _initTextPos.z),
            _pressedClip,
            _pressedImage);

        _isPlayPressClip = true;
    }

    /// <summary>
    /// セレクト
    /// </summary>
    public void Selected()
    {
        UpdateAnimation(
            _selectedColor, _initTextPos,
            _highlightClip,
            _highlightImage);
    }

    /// <summary>
    /// ディサブル
    /// </summary>
    public void Disabled()
    {

    }

    #endregion






    // ---------------------------- PrivateMethod

    /// <summary>
    /// アニメーション更新
    /// </summary>
    /// <param name="textColor"></param>
    /// <param name="textPos"></param>
    /// <param name="clip"></param>
    /// <param name="image"></param>
    private void UpdateAnimation(
        Color textColor, Vector3 textPos,
        UnityEvent clip,
        Sprite image)
    {
        _text.color = textColor;
        _text.rectTransform.localPosition = textPos;
        if (!_isPlayPressClip)
        {
            clip?.Invoke();
        }
        _image.sprite = image;
    }
}
