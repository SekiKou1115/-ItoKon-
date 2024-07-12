using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextHide : MonoBehaviour
{
    public GameObject TextMP;
    public GameObject TextMP2;

    //呼び出し時に実行される関数
    void Start()
    {
        //テキストメッシュプロのオブジェクトを取得
        TextMP = GameObject.Find("Wait");
        TextMP2 = GameObject.Find("Follow");

        TextMP2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        // Eキーが押されたらテキストの表示を切り替える
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
