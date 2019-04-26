using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlSurface : MonoBehaviour {

    public float deflection = 0f;
    private float zero;

	void Start ()
    {
        zero = transform.localEulerAngles.x;
	}
	
	void FixedUpdate ()
    {
        transform.localEulerAngles = new Vector3(zero + deflection, transform.localEulerAngles.y, transform.localEulerAngles.z);
	}
}
