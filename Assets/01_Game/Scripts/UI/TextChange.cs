using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextHide : MonoBehaviour
{
    [SerializeField, Tooltip("����L����")] private GameObject _players;
    [SerializeField, Tooltip("�V�Y�V�w")] private PlayerManager.Name _name;

    // �؂�ւ���UI�̐錾
    [SerializeField, Tooltip("�ҋ@")] private GameObject _textWait;
    [SerializeField, Tooltip("�Ǐ]")] private GameObject _textFollow;
    [SerializeField, Tooltip("�s���s�\")] private GameObject _textDamage;
    [SerializeField, Tooltip("����")] private GameObject _textPlay;

    // TextMeshPro���Q�Ƃ���^
    private TextMeshProUGUI _targetWait;
    private TextMeshProUGUI _targetFollow;
    private TextMeshProUGUI _targetDamage;
    private TextMeshProUGUI _targetPlay;

    //�Ăяo�����Ɏ��s�����֐�
    void Start()
    {
        // �Ǐ]������
        _textFollow.SetActive(false);
        // �s���s�\������
        _textDamage.SetActive(false);
        // 
    }

    // Update is called once per frame
    void Update()
    {
        // _name��PlayerManager��MovePlayerName�ƈ�v����Ƃ�
        if (_name == PlayerManager.Instance.MovePlayerName)
        {
            _textPlay.SetActive(true);
            _textWait.SetActive(false);
            _textFollow.SetActive(false);
        }
        // _name��PlayerManager��MovePlayerName�ƈ�v���Ȃ���
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

        // �������s���s�\�̎�
        if(_players.GetComponent<PlayerController>().IsIncapacitated)
        {
            _textDamage.SetActive(true);
        }
        else
        {
            _textDamage.SetActive(false);
        }

        // �����������Ȃ���
        //if (_players[0].GetComponent<PlayerController>().IsIncapacitated ||
        //    _players[1].GetComponent<PlayerController>().IsIncapacitated)
        //{
        //    Debug.Log("�s���s�\");
        //    _textWait.SetActive(false);
        //    _textFollow.SetActive(false);
        //    _textDamage.SetActive(true);
        //}
        //else if (!_players[0].GetComponent<PlayerController>().IsIncapacitated ||
        //         !_players[1].GetComponent<PlayerController>().IsIncapacitated)
        //{
        //    Debug.Log("�s���s�\����");
        //    _textDamage.SetActive(false);
        //}
        ////E�L�[�������ꂽ��e�L�X�g�̕\����؂�ւ���
        //else if (Input.GetKeyDown(KeyCode.E) ||
        //    !PlayerManager.Instance.IsWait)
        //{
        //    Debug.Log("ChangeFollw");
        //    _textWait.SetActive(false);    // _textWait�������Ȃ�����
        //    _textFollow.SetActive(true);    // _textFollow��������悤��
        //}
        //else if (Input.GetKeyDown(KeyCode.E) ||
        //    PlayerManager.Instance.IsWait)
        //{
        //    Debug.Log("ChangeWait");
        //    _textWait.SetActive(true);     // _textWait��������悤��
        //    _textFollow.SetActive(false);   // _textFollow�������Ȃ�����
        //}
    }
}
