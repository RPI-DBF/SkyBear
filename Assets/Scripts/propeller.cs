using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class propeller : MonoBehaviour {

    public GameObject prop;
    public GameObject blur;

    public float throttle;

	void Start ()
    {
		
	}
	
	void FixedUpdate ()
    {
        prop.transform.localEulerAngles = new Vector3(prop.transform.localEulerAngles.x, prop.transform.localEulerAngles.y, prop.transform.localEulerAngles.z - (Mathf.Clamp(throttle, 0, Mathf.Infinity) * 10000f * Time.fixedDeltaTime));
        if (throttle > 0.25) { prop.SetActive(false); blur.SetActive(true); } else { prop.SetActive(true); blur.SetActive(false); }
    }
}
