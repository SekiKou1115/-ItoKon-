using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextHide : MonoBehaviour
{
    public GameObject TextMP;
    public GameObject TextMP2;

    //�Ăяo�����Ɏ��s�����֐�
    void Start()
    {
        //�e�L�X�g���b�V���v���̃I�u�W�F�N�g���擾
        TextMP = GameObject.Find("Wait");
        TextMP2 = GameObject.Find("Follow");

        TextMP2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        // E�L�[�������ꂽ��e�L�X�g�̕\����؂�ւ���
        if (Input.GetKeyDown(KeyCode.E) &&
            PlayerManager.Instance.IsWait == true)
        {
            Debug.Log("Change");
            TextMP.SetActive(false);
            TextMP2.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.E) &&
            PlayerManager.Instance.IsWait == false)
        {
            Debug.Log("ChangeWait");
            TextMP.SetActive(true);
            TextMP2.SetActive(false);
        }
    }
}
