using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class rocket : MonoBehaviour {

    private float cD = 0.5f;
    private float S = Mathf.PI * Mathf.Pow(0.08255f / 2, 2);
    private float D;

    public bool createDummy = false;

    private GameObject airplane;
    private GameObject UI;
    private GameObject rigidbodyRocket;

    public LineRenderer dragVector;

    void Start () {
        airplane = transform.parent.gameObject;
        UI = GameObject.Find("UI");
        rigidbodyRocket = (GameObject)Resources.Load("rigidbodyRocket");
        
        dragVector = gameObject.AddComponent<LineRenderer>();
        dragVector.material = new Material(Shader.Find("Sprites/Default"));
        dragVector.positionCount = 2;
        dragVector.startWidth = 0.01f;
        dragVector.endWidth = 0.01f;
        dragVector.startColor = Color.red;
        dragVector.endColor = Color.red;
    }
	
	void FixedUpdate ()
    {
        D = airplane.GetComponent<flightModel>().q_inf * S * cD;
        airplane.GetComponent<Rigidbody>().AddForceAtPosition(-airplane.GetComponent<flightModel>().freestream.normalized * D, transform.position);
    }

    void Update()
    {
        if (UI.GetComponent<UI>().showVectors)
        {
            dragVector.enabled = true;
        }
        else
        {
            dragVector.enabled = false;
        }

        dragVector.SetPosition(0, transform.position);
        dragVector.SetPosition(1, -airplane.GetComponent<flightModel>().freestream.normalized * D + transform.position);
    }

    void OnDestroy()
    {
        if (createDummy)
        {
            GameObject newRocket = Instantiate(rigidbodyRocket, transform.position, transform.rotation);
            newRocket.GetComponent<Rigidbody>().velocity = airplane.GetComponent<Rigidbody>().velocity;
            newRocket.GetComponent<Rigidbody>().angularVelocity = airplane.GetComponent<Rigidbody>().angularVelocity;
        }
    }
}
