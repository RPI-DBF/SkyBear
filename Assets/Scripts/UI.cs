using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour {

    public bool showVectors = false;
    public GameObject xCG;
    public Text airspeedIndicator;
    public Text altitudeIndicator;
    public GameObject airplane;

    private GameObject selectedAirplane;
    private GameObject skyBear;
    private GameObject spyBear;
    private GameObject skyGrizzly;

    // Sliders
    public Slider timeScale;
    public Slider maxThrust;
    public Slider weight;
    public Slider staticMargin;
    public Slider airDensity;
    public Slider gustMagnitude;
    public Slider gustUnsteadiness;
    public Text maxThrustText;
    public Text weightText;
    public Text smText;
    public Text airDensityText;
    public Text gustMagnitudeText;
    public Text gustUnsteadinessText;

    public GameObject trimWeight;
    public GameObject chaseCamera;
    public GameObject pilotCamera;
    private Vector3 trimZero;
    private float thrustZero;
    private float densityZero;
    private float shiftWeight;

    // Sprites
    private Sprite runwayIcon;
    private Sprite rampIcon;

    // Other UI Elements
    public GameObject configurationSelection;
    public Image spawnLocation;

    // Rocket Manager
    private int rocketCount = 6;
    private bool rocketsLoaded = false;

    // Vectors
    public Vector3 spawnLocationVector = new Vector3(0.056f, 0.242f, 0.301f);

    void Start ()
    {
        skyBear = (GameObject)Resources.Load("skyBear");
        spyBear = (GameObject)Resources.Load("spyBear");
        skyGrizzly = (GameObject)Resources.Load("skyGrizzly");

        selectedAirplane = skyBear;
        Instantiate(selectedAirplane, spawnLocationVector, Quaternion.Euler(-5, 0, 0));

        airplane = GameObject.FindGameObjectWithTag("airplane");
        xCG = airplane.transform.Find("x_cg").gameObject;
        trimWeight = airplane.transform.Find("massDistribution").gameObject;

        trimZero = trimWeight.transform.localPosition;
        thrustZero = airplane.GetComponent<flightModel>().thrustMax_lbf;
        densityZero = airplane.GetComponent<flightModel>().rho;

        runwayIcon = Resources.Load<Sprite>("Runway Icon");
        rampIcon = Resources.Load<Sprite>("Ramp Icon");
    }

    void Update ()
    {
        airplane = GameObject.FindGameObjectWithTag("airplane");
        xCG = airplane.transform.Find("x_cg").gameObject;
        trimWeight = airplane.transform.Find("massDistribution").gameObject;

        // Check Rockets Loaded
        if (selectedAirplane == skyGrizzly) rocketsLoaded = true;
        else rocketsLoaded = false;

        // Calculate & Assign Rocket Payload Weight
        if (rocketsLoaded) airplane.GetComponent<flightModel>().W_payload = 0.14375f * rocketCount;
        else airplane.GetComponent<flightModel>().W_payload = 0;

        airspeedIndicator.text = Mathf.RoundToInt(airplane.GetComponent<flightModel>().U_inf).ToString();
        altitudeIndicator.text = Mathf.RoundToInt(airplane.GetComponent<flightModel>().alt * 3.28084f).ToString();
        maxThrustText.text = "Max. Thrust = " + airplane.GetComponent<flightModel>().thrustMax_lbf.ToString() + " lbf";
        weightText.text = "Vehicle Empty Weight = " + airplane.GetComponent<flightModel>().W_empty.ToString() + " lbf";
        smText.text = "Static Margin = " + airplane.GetComponent<flightModel>().SM.ToString() + "%";
        airDensityText.text = "Air Density = " + airplane.GetComponent<flightModel>().rho + " kg/m3";
        gustMagnitudeText.text = "Gust Magnitude = " + airplane.GetComponent<flightModel>().gustMagnitude + " mph";
        gustUnsteadinessText.text = "Gust Unsteadiness = " + airplane.GetComponent<flightModel>().gustUnsteadiness;

        shiftWeight = trimZero.z + (staticMargin.value - 0.5f)/10;
        trimWeight.transform.localPosition = new Vector3(trimWeight.transform.localPosition.x, trimWeight.transform.localPosition.y, shiftWeight);

        Time.timeScale = timeScale.value;
        airplane.GetComponent<flightModel>().thrustMax_lbf = maxThrust.value;
        airplane.GetComponent<flightModel>().W_empty = weight.value;
        airplane.GetComponent<flightModel>().rho = airDensity.value;
        airplane.GetComponent<flightModel>().gustMagnitude = gustMagnitude.value;
        airplane.GetComponent<flightModel>().gustUnsteadiness = gustUnsteadiness.value;

		if (showVectors)
        {
            xCG.GetComponent<TrailRenderer>().enabled = true;
        }
        else
        {
            xCG.GetComponent<TrailRenderer>().enabled = false;
        }
    }

    public void toggleVectors ()
    {
        showVectors = !showVectors;
    }

    public void toggleCamera ()
    {
        chaseCamera.GetComponent<Camera>().enabled = !chaseCamera.GetComponent<Camera>().isActiveAndEnabled;
        pilotCamera.GetComponent<Camera>().enabled = !pilotCamera.GetComponent<Camera>().isActiveAndEnabled;
        chaseCamera.GetComponent<AudioListener>().enabled = !chaseCamera.GetComponent<AudioListener>().isActiveAndEnabled;
        pilotCamera.GetComponent<AudioListener>().enabled = !pilotCamera.GetComponent<AudioListener>().isActiveAndEnabled;
    }

    public void dropRocket()
    {
        if (rocketsLoaded && rocketCount > 0)
        {
            airplane.GetComponent<rocketManager>().dropRocket(6 - rocketCount);
            rocketCount = rocketCount - 1;
        }
    }

    public void restart ()
    {
        Destroy(airplane);
        /*GameObject[] droppedRockets = GameObject.FindGameObjectsWithTag("droppedRocket");
        for (var droppedRocket = 0; droppedRocket < droppedRockets.Length; droppedRocket++)
        {
            Destroy(droppedRockets[droppedRocket]);
        }*/
        Instantiate(selectedAirplane, spawnLocationVector, Quaternion.Euler(-5, 0, 0));
        rocketCount = 6;
    }

    public void missionSelect()
    {
        configurationSelection.SetActive(true);
    }

    public void selectMission1()
    {
        selectedAirplane = skyBear;
        restart();
        configurationSelection.SetActive(false);
    }

    public void selectMission2()
    {
        selectedAirplane = spyBear;
        restart();
        configurationSelection.SetActive(false);
    }

    public void selectMission3()
    {
        selectedAirplane = skyGrizzly;
        restart();
        configurationSelection.SetActive(false);
    }

    public void swapSpawnLocation()
    {
        if (spawnLocationVector == new Vector3(0.056f, 0.242f, 0.301f))
        {
            spawnLocationVector = new Vector3(-8, 0.242f, 0.301f);
            spawnLocation.sprite = rampIcon;
        }

        else
        {
            spawnLocationVector = new Vector3(0.056f, 0.242f, 0.301f);
            spawnLocation.sprite = runwayIcon;
        }
    }

}
