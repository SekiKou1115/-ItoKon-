using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

    public float Speed;
	
	void Update () {
         transform.Rotate(Vector3.right * Time.deltaTime * Speed);
    }
}
