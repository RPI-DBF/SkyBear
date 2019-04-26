using UnityEngine;
using System.Collections;

public class RightController : MonoBehaviour {

    //define the trackedObject and device 
    private SteamVR_TrackedObject trackedObject;
    private SteamVR_Controller.Device device;

    //define the UI
    public GameObject ui;

    //public fields for the x/y touchpad locations
    public float X;
    public float Y;

    //public fields for the calculated values of pitch/roll
    public float pitch;
    public float roll;

    //receiver will reference airplane game object
    //transmitter inputs will be sent to the receiver
    public GameObject receiver;

    //initialize trackedObject on startup
    void Start () {
    	//assign the airplane gameobject to the receiver game object
        receiver = GameObject.FindGameObjectWithTag("airplane");

        //define the tracked object as the vr controller
        trackedObject = GetComponent<SteamVR_TrackedObject>();

        //assign the UI gameobject to ui
        ui = GameObject.FindGameObjectWithTag("ui");
    }
    
    //Update method runs each frame
    void Update () {
        //assign the airplane gameobject to the receiver game object
        receiver = GameObject.FindGameObjectWithTag("airplane");

        //define the device
        device = SteamVR_Controller.Input((int)trackedObject.index);

        //save the x and y values to public fields
        Y = device.GetAxis().y;
        X = device.GetAxis().x;

        //calculate the inupt pitch and roll
        pitch = Y * -40;
        roll = X * -20;

        //assign the calculated pitch and roll to the flight model inputs
        receiver.GetComponent<flightModel>().inputPitch = pitch;
        receiver.GetComponent<flightModel>().inputRoll = roll;

        //check to see if the tigger is pressed
        if(device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger)){
            //restart the program
            ui.GetComponent<UI>().restart();
        }
    }
}
