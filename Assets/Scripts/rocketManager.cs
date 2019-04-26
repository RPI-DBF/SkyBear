using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocketManager : MonoBehaviour {

    public GameObject rocket1;
    public GameObject rocket2;
    public GameObject rocket3;
    public GameObject rocket4;
    public GameObject rocket5;
    public GameObject rocket6;
    private GameObject[] rockets;
    private int[] rocketOrder = { 0, 3, 2, 5, 1, 4 };
    
	void Start ()
    {
        rockets = new GameObject[] { rocket1, rocket2, rocket3, rocket4, rocket5, rocket6 };
	}

    public void dropRocket(int i)
    {
        rockets[rocketOrder[i]].GetComponent<rocket>().createDummy = true;
        Destroy(rockets[rocketOrder[i]]);
    }
}
