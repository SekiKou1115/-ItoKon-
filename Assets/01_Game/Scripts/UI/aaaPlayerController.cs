using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class aaaPlayerController : MonoBehaviour
{
    public static aaaPlayerController Instance;



    private void Awake()
    {
        Instance = this;
    }



    /// <summary>
    /// �ꎞ��~
    /// </summary>
    /// <param name="ctx"></param>
    public void OnPause(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            //UIManager.Instance.OnPause();
        }
    }

    /// <summary>
    /// �Đ�
    /// </summary>
    /// <param name="ctx"></param>
    public void OnBack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            //UIManager.Instance.OnBack();
        }
    }


}
