using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoG : MonoBehaviour {

    private GameObject parent;

	void Start () {
        parent = transform.parent.gameObject;
	}
	
	void Update ()
    {
		
	}

    private void FixedUpdate()
    {
        transform.position = parent.transform.position + transform.TransformDirection(parent.GetComponent<Rigidbody>().centerOfMass);
    }
}
