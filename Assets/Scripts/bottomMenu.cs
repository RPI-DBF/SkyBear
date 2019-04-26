using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bottomMenu : MonoBehaviour {

    public GameObject toggleButton;
    private bool menuActive = false;
    private float menuPosition;
    private float initialPosition;

	void Start ()
    { 
        menuPosition = transform.position.y;
        initialPosition = menuPosition;
	}
	
	void Update ()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, menuPosition, transform.position.z), 15f * (Time.deltaTime + 0.000001f) / Mathf.Clamp(Time.timeScale, 0.000001f, 1));
	}

    public void toggleMenu ()
    {
        if (menuActive)
        {
            menuPosition = initialPosition;
            menuActive = !menuActive;
            toggleButton.transform.localEulerAngles = new Vector3(toggleButton.transform.localEulerAngles.x, toggleButton.transform.localEulerAngles.y, 0);
        }

        else
        {
            menuPosition = 0f;
            menuActive = !menuActive;
            toggleButton.transform.localEulerAngles = new Vector3(toggleButton.transform.localEulerAngles.x, toggleButton.transform.localEulerAngles.y, 180);
        }
    }
}
