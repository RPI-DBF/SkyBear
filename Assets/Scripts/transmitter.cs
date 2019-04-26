using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transmitter : MonoBehaviour {

    public float inputThrottle;
    public float inputPitch;
    public float inputRoll;
    public float inputYaw;

    public bool useTouch = true;

    public GameObject receiver;
    private Joystick leftJoystick;
    private Joystick rightJoystick;
    public GameObject noseGear;
    private bool safety = true;

    void Start() {
        //receiver = GameObject.FindGameObjectWithTag("airplane");

        //leftJoystick = transform.Find("Left Joystick").GetComponent<Joystick>();
        //rightJoystick = transform.Find("Right Joystick").GetComponent<Joystick>();

        //if (Application.platform == RuntimePlatform.WindowsPlayer && !Application.isEditor) { useTouch = false; }
        //else { useTouch = true; }
	}

    void FixedUpdate()
    {
        //noseGear.transform.localEulerAngles = new Vector3(0, inputYaw * 100, 0);
    }

    void Update () {
        /*
        //receiver = GameObject.FindGameObjectWithTag("airplane");

        // TOUCHSCREEN INPUT
        if (useTouch)
        {
            leftJoystick.gameObject.SetActive(true);
            rightJoystick.gameObject.SetActive(true);

            if (safety && leftJoystick.Vertical < -0.95)
            {
                safety = false;
            }
            if (!safety)
            {
                inputThrottle = (leftJoystick.Vertical + 1) / 2;
            }
            else { inputThrottle = 0; }
            inputRoll = -rightJoystick.Horizontal * 10;
            inputPitch = -rightJoystick.Vertical * 20;
            inputYaw = leftJoystick.Horizontal * 0.5f;
        }

        // JOYSTICK INPUT
        else
        {
            leftJoystick.gameObject.SetActive(false);
            rightJoystick.gameObject.SetActive(false);

            inputRoll = -Input.GetAxisRaw("Roll") * 10;
            inputPitch = -Input.GetAxisRaw("Pitch") * 20;
            inputYaw = Input.GetAxisRaw("Yaw") * 0.5f;
            inputYaw = 0;
            inputThrottle = (Input.GetAxisRaw("Throttle") + 1) / 2;
        }

        
        receiver.GetComponent<flightModel>().inputThrottle = inputThrottle;
        receiver.GetComponent<flightModel>().inputPitch = inputPitch;
        receiver.GetComponent<flightModel>().inputRoll = inputRoll;
        receiver.GetComponent<flightModel>().inputYaw = inputYaw;*/
    }
}
