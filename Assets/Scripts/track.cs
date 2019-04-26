using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class track : MonoBehaviour {

    public GameObject target;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("airplane");
    }

    void Update()
    {
        target = GameObject.FindGameObjectWithTag("airplane");
        transform.LookAt(target.transform.position);
    }
}
