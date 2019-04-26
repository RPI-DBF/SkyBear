using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dummyRocket : MonoBehaviour {

    private float cD = 0.5f;
    private float S = Mathf.PI * Mathf.Pow(0.08255f / 2, 2);
    private float D;

    void Start ()
    {
		
	}
	
	void FixedUpdate ()
    {
        D = 0.5f * 1.225f * Mathf.Pow(GetComponent<Rigidbody>().velocity.magnitude, 2) * S * cD;
        GetComponent<Rigidbody>().AddForceAtPosition(-GetComponent<Rigidbody>().velocity.normalized * D, transform.position);
        transform.forward = Vector3.Lerp(transform.forward, GetComponent<Rigidbody>().velocity.normalized, GetComponent<Rigidbody>().velocity.magnitude * 5 * Time.fixedDeltaTime);
    }
}
