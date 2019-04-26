using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chase : MonoBehaviour
{

    public GameObject target;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("airplane").transform.Find("chaseTarget").gameObject;
    }

    void FixedUpdate()
    {
        target = GameObject.FindGameObjectWithTag("airplane").transform.Find("chaseTarget").gameObject;
        transform.position = Vector3.Lerp(transform.position, target.transform.position, 20f * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, target.transform.rotation, 20f * Time.fixedDeltaTime);
    }
}