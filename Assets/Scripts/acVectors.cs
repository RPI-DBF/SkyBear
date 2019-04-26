using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class acVectors : MonoBehaviour {

    public GameObject UI;

    private GameObject liftVectorPlaceholder;
    private GameObject dragVectorPlaceholder;

    public LineRenderer freestream;
    public LineRenderer lift;
    public LineRenderer drag;

    public float freestreamMagnitude;
    public float liftMagnitude;
    public float dragMagnitude;

    public Vector3 freestreamVector;
    public Vector3 liftVector;
    public Vector3 dragVector;

    private float vectorThickness = 0.01f;

	void Start ()
    {
        UI = GameObject.Find("UI");

        liftVectorPlaceholder = new GameObject();
        dragVectorPlaceholder = new GameObject();
        liftVectorPlaceholder.transform.position = transform.position;
        dragVectorPlaceholder.transform.position = transform.position;
        liftVectorPlaceholder.transform.SetParent(transform);
        dragVectorPlaceholder.transform.SetParent(transform);

        freestream = gameObject.AddComponent<LineRenderer>();
        lift = liftVectorPlaceholder.AddComponent<LineRenderer>();
        drag = dragVectorPlaceholder.AddComponent<LineRenderer>();

        freestream.material = new Material(Shader.Find("Sprites/Default"));
        lift.material = new Material(Shader.Find("Sprites/Default"));
        drag.material = new Material(Shader.Find("Sprites/Default"));

        freestream.positionCount = 2;
        lift.positionCount = 2;
        drag.positionCount = 2;

        freestream.startWidth = vectorThickness;
        lift.startWidth = vectorThickness;
        drag.startWidth = vectorThickness;

        freestream.endWidth = vectorThickness;
        lift.endWidth = vectorThickness;
        drag.endWidth = vectorThickness;

        freestream.startColor = Color.yellow;
        lift.startColor = Color.blue;
        drag.startColor = Color.red;

        freestream.endColor = Color.yellow;
        lift.endColor = Color.blue;
        drag.endColor = Color.red;
    }

    void Update ()
    {
        if (UI.GetComponent<UI>().showVectors)
        {
            freestream.enabled = true;
            lift.enabled = true;
            drag.enabled = true;
        }
        else
        {
            freestream.enabled = false;
            lift.enabled = false;
            drag.enabled = false;
        }

        freestream.SetPosition(0, transform.position);
        lift.SetPosition(0, transform.position);
        drag.SetPosition(0, transform.position);

        freestream.SetPosition(1, freestreamVector * freestreamMagnitude + transform.position);
        lift.SetPosition(1, liftVector * liftMagnitude + transform.position);
        drag.SetPosition(1, dragVector * dragMagnitude + transform.position);
    }
}
