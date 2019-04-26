using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windSock : MonoBehaviour {

    public GameObject airplane;
    public GameObject sock;

	void Start ()
    {
        airplane = GameObject.FindGameObjectWithTag("airplane");
	}
	
	void FixedUpdate ()
    {
        airplane = GameObject.FindGameObjectWithTag("airplane");
        transform.forward = airplane.GetComponent<flightModel>().gust;
        sock.transform.localEulerAngles = new Vector3(Mathf.Clamp(90 - Mathf.Abs(airplane.GetComponent<flightModel>().gust.magnitude) * 7.5f + Random.Range(-1f, 1f), 0, 90), 0, 0);
    }
}
