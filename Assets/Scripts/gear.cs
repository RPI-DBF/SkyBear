using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gear : MonoBehaviour {

    public GameObject transmitter;
    public GameObject noseGear;

	void Start ()
    {
        transmitter = GameObject.FindGameObjectWithTag("ui");
    }
	
	void Update ()
    {
        transmitter = GameObject.FindGameObjectWithTag("ui");
    }

    void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.thisCollider.tag == "noseGear")
            {
                GetComponent<Rigidbody>().AddForceAtPosition(noseGear.transform.right * transmitter.GetComponent<transmitter>().inputYaw * 3 * transform.InverseTransformVector(GetComponent<Rigidbody>().velocity).z, contact.point);
                noseGear.transform.localEulerAngles = new Vector3(noseGear.transform.localEulerAngles.x, transmitter.GetComponent<transmitter>().inputYaw * 50, noseGear.transform.localEulerAngles.z);
            }

            if (contact.thisCollider.tag == "tire")
            {
                GetComponent<Rigidbody>().AddForceAtPosition(Vector3.ProjectOnPlane(transform.right, Vector3.up) * 10 * -Vector3.ProjectOnPlane(transform.InverseTransformVector(GetComponent<Rigidbody>().velocity), Vector3.up).x, contact.point);
            }
        }
    }
}
