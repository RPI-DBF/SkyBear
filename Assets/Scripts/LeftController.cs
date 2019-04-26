using UnityEngine;
using System.Collections;

public class LeftController : MonoBehaviour {

    //define the trackedObject and device 
    private SteamVR_TrackedObject trackedObject;
    private SteamVR_Controller.Device device;

    //public fields for the x/y touchpad locations
    public float X;
    public float Y;

    //public fields for the calucated throttle and yaw inputs
    public float throttle = 0;
    public float yaw;

    //receiver will reference airplane game object
    //transmitter inputs will be sent to the receiver
    public GameObject receiver;

    //initialize trackedObject on startup
    void Start () {
        //assign the airplane gameobject to the receiver game object
        receiver = GameObject.FindGameObjectWithTag("airplane");

        //define the tracked object as the vr controller
        trackedObject = GetComponent<SteamVR_TrackedObject>();
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

        //calculate the input throttle and yaw
        yaw = X * 0;

        if(Y != 0){
            throttle = (Y + 1) / 2;
        }

        //assign the calculated throttle and yaw to the flight model inputs
        receiver.GetComponent<flightModel>().inputThrottle = throttle;
        receiver.GetComponent<flightModel>().inputYaw = yaw;

        //adjust the volume of the motor with the throttle input
        receiver.GetComponent<AudioSource>().volume = throttle;
    }
}
