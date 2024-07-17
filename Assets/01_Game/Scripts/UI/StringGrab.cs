using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StringGrab : MonoBehaviour
{
    // �؂�ւ���UI�̐錾
    [SerializeField, Tooltip("�͂�UI")] private GameObject _uiGrab;
    [SerializeField, Tooltip("����UI")] private GameObject _uiLet;
    [SerializeField, Tooltip("�͂�")] private GameObject _textGrab;
    [SerializeField, Tooltip("����")] private GameObject _textLet;

    // TextMeshPro���Q�Ƃ���^
    private TextMeshProUGUI _targetGrab;
    private TextMeshProUGUI _targetLet;

    // Start is called before the first frame update
    void Start()
    {
        // �摜������
        _uiGrab.SetActive(false);
        _uiLet.SetActive(false);

        // �R�̃e�L�X�g������
        _textGrab.SetActive(false);
        _textLet.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // ����͂�ł��邩�̔���
        if (PlayerManager.Instance.IsWait) // �������̒��Ɏ�������ł��邩�̔��������
        {
            // �����Ɏ��ۂɎg���摜������
            _uiGrab.SetActive(true);
            _uiLet.SetActive(false);

            // �R�̃I���E�I�t
            _textGrab.SetActive(true);
            _textLet.SetActive(false);
        }
        else
        {
            // �����Ɏ��ۂɎg���摜������
            _uiGrab.SetActive(false);
            _uiLet.SetActive(true);

            // �R�̃I���E�I�t
            _textGrab.SetActive(false);
            _textLet.SetActive(true);
        }
    }
}