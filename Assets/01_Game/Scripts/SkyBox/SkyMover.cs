using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyMover : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed;

    private float _rotationRepeatValue;

    private void Update()
    {
        _rotationRepeatValue = Mathf.Repeat(RenderSettings.skybox.GetFloat("_Rotation") + _rotateSpeed, 360f);

        RenderSettings.skybox.SetFloat("_Rotation", _rotationRepeatValue);
    }
}
