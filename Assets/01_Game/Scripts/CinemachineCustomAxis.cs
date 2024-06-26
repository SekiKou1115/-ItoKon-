using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineCustomAxis : MonoBehaviour
{
    private bool xInversion = true, yInversion = true;

    void Start()
    {
        Cinemachine.CinemachineCore.GetInputAxis = GetAxisCustom;
    }

    private float GetAxisCustom(string axisName)
    {
        if (axisName == "Mouse X")
        {
            return Input.GetAxis(axisName) * (xInversion ? -1f : 1f);
        }
        else if (axisName == "Mouse Y")
        {
            return Input.GetAxis(axisName) * (yInversion ? -1 : 1);
        }

        return 0;
    }
}
