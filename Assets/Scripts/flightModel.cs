using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flightModel : MonoBehaviour {

    // FLOATS
    // General Floats
    public float W_empty = 5;
    public float W_payload = 0;
    public float U_inf;
    public float alt;
    public float alpha;
    public float beta;
    public float SM;
    private float c_L;
    private float c_D;
    public float lift;
    public float drag;
    public float thrust;
    private float thrustMax;
    public float thrustMax_lbf = 5f;
    private float thrustTimer = 0;
    public float gustMagnitude = 1f;
    public float gustUnsteadiness = 1f;

    public float rho = 1.225f;
    private float mu = 1.81e-5f;
    public float q_inf;

    public float fCDi = 5.5f;

    // Wing Geometrical Floats
    private float S_ref = 0.35447619427f;
    private float b = 1.3716f;
    private float mac = 0.28556204f;
    private float AR = 5.3072f;

    // Vertical Tail Geometrical Floats
    private float mac_vt = 0.1100709f;
    private float S_vt = 0.0309677f;
    private float S_wetted_vt = 0.0619354f;
    private float b_vt = 6f;
    private float AR_vt = 1.5f;
    private float sweep_vt = 40f;

    // Control Input Floats
    public float inputThrottle;
    public float inputPitch;
    public float inputRoll;
    public float inputYaw;

    // Flight Model Database Floats
    private int n = 17;
    private float[] airfoil = { 23021, 23021, 23021, 23021, 23021, 35124, 35124, 35124, 35124, 35124, 35124, 35124, 23021, 23021, 23021, 23021, 23021 };
    private float[] c = { 0.1649f, 0.1900f, 0.2151f, 0.2371f, 0.2559f, 0.2785f, 0.3318f, 0.4051f, 0.4572f, 0.4051f, 0.3318f, 0.2785f, 0.2559f, 0.2371f, 0.2151f, 0.1900f, 0.1649f };
    private float[] S = { 0.01676747f, 0.01933491f, 0.02190235f, 0.01811164f, 0.01955583f, 0.01779702f, 0.02108996f, 0.02565308f, 0.03431955f, 0.02565308f, 0.02108996f, 0.01779702f, 0.01955583f, 0.01811164f, 0.02190235f, 0.01933491f, 0.01676747f};
    private float[] S_wetted = { 0.03538798f, 0.04080528f, 0.04622611f, 0.03822849f, 0.04127984f, 0.03807552f, 0.04605768f, 0.05567526f, 0.07239974f, 0.05567526f, 0.04605768f, 0.03807552f, 0.04127984f, 0.03822849f, 0.04622611f, 0.04080528f, 0.03538798f };
    private float[] sweep = { 20f, 20f, 20f, 20f, 20f, 38.71f, 38.71f, 38.71f, 38.71f, 38.71f, 38.71f, 38.71f, 20f, 20f, 20f, 20f, 20f };
    private float[] Cla = { 4.98f, 4.98f, 4.98f, 4.98f, 4.98f, 6.589f, 6.589f, 6.589f, 6.589f, 6.589f, 6.589f, 6.589f, 4.98f, 4.98f, 4.98f, 4.98f, 4.98f };
    private float[] alpha_0L = { -0.95f, -0.95f, -0.95f, -0.95f, -0.95f, -3.65f, -3.65f, -3.65f, -3.65f, -3.65f, -3.65f, -3.65f, -0.95f, -0.95f, -0.95f, -0.95f, -0.95f };
    private float[] alpha_geo = { -1.778f, -1.333f, -0.889f, -0.5f, -0.167f, 0.0694f, 0.208f, 0.347f, 0.5f, 0.347f, 0.208f, 0.0694f, -0.167f, -0.5f, -0.889f, -1.333f, -1.778f };

    private float[] alpha_data = { -180, -170, -90, -45, -20, -19, -18, -17, -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 45, 90, 170, 180 };
    private float[] cl_23021 = { -0.1f, 1, 0, -0.4f, -1.0195f, -1.1037f, -1.0888f, -1.0828f, -1.0696f, -1.0257f, -0.9897f, -0.9359f, -0.8957f, -0.8819f, -0.8338f, -0.7053f, -0.5911f, -0.5151f, -0.4464f, -0.3885f, -0.295f, -0.1967f, -0.096f, 0.0038f, 0.1067f, 0.2076f, 0.308f, 0.4047f, 0.4991f, 0.595f, 0.7101f, 0.8541f, 0.9979f, 1.1353f, 1.1309f, 1.1643f, 1.2169f, 1.2747f, 1.3338f, 1.3751f, 1.4226f, 1.4435f, 1.4664f, 1.4745f, 1.4577f, 1.4502f, 1.4282f, 1.3595f, 1.3069f, 0.75f, 0, -1, -0.1f };
    private float[] cl_35124 = { -0.1f, 1, 0, -0.8f, -1.0625f, -1.0605f, -1.0438f, -1.0165f, -0.9807f, -0.9468f, -0.8676f, -0.677f, -0.5993f, -0.5324f, -0.4611f, -0.3853f, -0.3099f, -0.2306f, -0.1525f, -0.0756f, -0.0063f, -0.0092f, 0.0293f, 0.1169f, 0.2251f, 0.3522f, 0.4962f, 0.6397f, 0.7635f, 0.8741f, 0.9803f, 1.0677f, 1.1657f, 1.2596f, 1.3534f, 1.3856f, 1.3745f, 1.367f, 1.3613f, 1.3585f, 1.3509f, 1.3444f, 1.3413f, 1.3233f, 1.2963f, 1.2811f, 1.2835f, 1.2589f, 1.2423f, 1.2f, 0, -1, -0.1f };
    private float[] cl_fp = { 0f, 1f, 0f, -0.8f, -1f, -0.9f, -0.8f, -0.8f, -0.8f, -0.8f, -0.8f, -0.85f, -0.85f, -0.9f, -1f, -0.9f, -0.8f, -0.7f, -0.6f, -0.5f, -0.4f, -0.3f, -0.2f, -0.1f, 0f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1f, 0.9f, 0.85f, 0.85f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.9f, 1f, 1f, 1f, 0.9f, 0.8f, 0.8f, 0f, -1f, 0f };
    private float[] cdp_23021 = { 0.00654f, 0.15f, 1.4f, 0.5f, 0.09952f, 0.07308f, 0.06279f, 0.05174f, 0.04215f, 0.03652f, 0.03037f, 0.02628f, 0.02228f, 0.0195f, 0.0167f, 0.01351f, 0.01063f, 0.00855f, 0.00679f, 0.00519f, 0.00462f, 0.00427f, 0.00407f, 0.00402f, 0.00404f, 0.00421f, 0.00457f, 0.00512f, 0.00589f, 0.00688f, 0.00815f, 0.00962f, 0.01115f, 0.01275f, 0.01362f, 0.0153f, 0.01744f, 0.02043f, 0.02359f, 0.02842f, 0.03305f, 0.04061f, 0.04849f, 0.05867f, 0.07259f, 0.08564f, 0.10126f, 0.1246f, 0.14628f, 0.5f, 1.4f, 0.15f, 0.00654f };
    private float[] cdp_35124 = { 0.00654f, 0.15f, 1.4f, 0.5f, 0.05478f, 0.04496f, 0.0375f, 0.03206f, 0.02759f, 0.02402f, 0.02077f, 0.01671f, 0.01442f, 0.01259f, 0.01104f, 0.00969f, 0.00853f, 0.00761f, 0.00681f, 0.0062f, 0.00563f, 0.00513f, 0.00539f, 0.00579f, 0.00654f, 0.00749f, 0.00877f, 0.00984f, 0.01061f, 0.01144f, 0.01234f, 0.0132f, 0.01423f, 0.0156f, 0.01751f, 0.02014f, 0.02403f, 0.02891f, 0.03494f, 0.04207f, 0.05067f, 0.06005f, 0.06963f, 0.08157f, 0.09524f, 0.10771f, 0.11802f, 0.13238f, 0.14598f, 0.5f, 1.4f, 0.15f, 0.00654f };
    private float[] cdp_fp = { 0.00654f, 0.15f, 1.4f, 0.5f, 0.14628f, 0.1246f, 0.10126f, 0.08564f, 0.07259f, 0.05867f, 0.04849f, 0.04061f, 0.03305f, 0.02842f, 0.02359f, 0.02043f, 0.01744f, 0.0153f, 0.01362f, 0.01275f, 0.01115f, 0.00962f, 0.00815f, 0.00688f, 0.00589f, 0.00512f, 0.00457f, 0.00512f, 0.00589f, 0.00688f, 0.00815f, 0.00962f, 0.01115f, 0.01275f, 0.01362f, 0.0153f, 0.01744f, 0.02043f, 0.02359f, 0.02842f, 0.03305f, 0.04061f, 0.04849f, 0.05867f, 0.07259f, 0.08564f, 0.10126f, 0.1246f, 0.14628f, 0.5f, 1.4f, 0.15f, 0.00654f };
    private float[] cm_23021 = { 0f, 0.3f, 0.5f, 0.2f, -0.0484f, -0.0612f, -0.0636f, -0.0656f, -0.0656f, -0.064f, -0.0614f, -0.0577f, -0.0515f, -0.0393f, -0.033f, -0.0403f, -0.0434f, -0.0368f, -0.0285f, -0.0188f, -0.0144f, -0.011f, -0.008f, -0.0049f, -0.0026f, 0.0001f, 0.0028f, 0.0062f, 0.0099f, 0.0134f, 0.0124f, 0.0045f, -0.0038f, -0.0115f, 0.0077f, 0.0192f, 0.0259f, 0.0303f, 0.0334f, 0.0363f, 0.0381f, 0.0387f, 0.0384f, 0.0363f, 0.0323f, 0.0277f, 0.0209f, 0.0092f, -0.0034f, -0.3f, -0.5f, -0.4f, 0f };
    private float[] cm_35124 = { 0f, 0.3f, 0.5f, 0.2f, -0.0937f, -0.0939f, -0.0912f, -0.0867f, -0.0808f, -0.073f, -0.0713f, -0.0901f, -0.0856f, -0.0784f, -0.0717f, -0.0654f, -0.0589f, -0.0527f, -0.0461f, -0.0392f, -0.0308f, -0.0081f, 0.0082f, 0.0146f, 0.0169f, 0.0149f, 0.0096f, 0.0035f, 0.0008f, 0.0001f, -0.0002f, 0.0025f, 0.0021f, 0.0011f, -0.0017f, 0.0051f, 0.0175f, 0.0274f, 0.0346f, 0.0395f, 0.0423f, 0.0436f, 0.0438f, 0.0426f, 0.0402f, 0.037f, 0.0335f, 0.0281f, 0.0219f, -0.3f, -0.5f, -0.4f, 0f };

    // Wing Model Floats
    public float[] y = { };
    private float[] AoA = { };
    private float[] CLa = { };
    private float[] CL = { };
    private float[] CDp = { };
    private float[] Di = { };
    private float[] Df = { };
    private float[] Dp = { };
    private float[] CM = { };
    private float[] L = { };
    private float[] D = { };
    private float[] M = { };
    private float[] Re = { };
    private float[] alpha_delta;

    // Vertical Tail Model Floats
    private float Cla_vt = 2 * Mathf.PI;
    private float CLa_vt;
    private float CL_vt;
    private float CDp_vt;
    private float Di_vt;
    private float Df_vt;
    private float Dp_vt;
    private float L_vt;
    private float D_vt;
    private float Re_vt;

    // Control Surface Floats
    private float delta_elevon_l;
    private float delta_elevon_r;
    private float alpha_delta_elevon_l;
    private float alpha_delta_elevon_r;
    public float cs_effectiveness = 0.5f;

    // Angular Velocities
    public float fc_alpha;
    public float fc_beta;
    public float fc_phi;
    public float fc_p;
    public float fc_q;
    public float fc_r;

    // GAME OBJECTS

    // General Game Objects
    private Transform thrustVector;
    private Rigidbody rigidBody;
    public GameObject elevonLeft;
    public GameObject elevonRight;

    // Aerodynamic Centers
    private Transform[] ac;
    public Transform ac_vt_l;
    public Transform ac_1;
    public Transform ac_2;
    public Transform ac_3;
    public Transform ac_4;
    public Transform ac_5;
    public Transform ac_6;
    public Transform ac_7;
    public Transform ac_8;
    public Transform ac_9;
    public Transform ac_10;
    public Transform ac_11;
    public Transform ac_12;
    public Transform ac_13;
    public Transform ac_14;
    public Transform ac_15;
    public Transform ac_16;
    public Transform ac_17;
    public Transform ac_vt_r;

    // VECTORS
    public Vector3 cg;
    public Vector3 freestream;
    private Vector3[] chordwise;
    private Vector3 chordwise_vt_l;
    private Vector3 chordwise_vt_r;

    public Vector3 gust;
    private Vector3 gustVector;
    private Vector2 randomGustDirection;

    // CURVES
    public AnimationCurve liftCurve_23021;
    public AnimationCurve liftCurve_35124;
    public AnimationCurve liftCurve_fp;
    public AnimationCurve dragCurve_23021;
    public AnimationCurve dragCurve_35124;
    public AnimationCurve dragCurve_fp;
    public AnimationCurve momentCurve_23021;
    public AnimationCurve momentCurve_35124;

    void Start()
    {
        // DECLARE ARRAYS
        y = new float[n];
        AoA = new float[n];
        CLa = new float[n];
        CL = new float[n];
        CDp = new float[n];
        Di = new float[n];
        Df = new float[n];
        Dp = new float[n];
        CM = new float[n];
        L = new float[n];
        D = new float[n];
        M = new float[n];
        Re = new float[n];
        ac = new Transform[] { ac_1, ac_2, ac_3, ac_4, ac_5, ac_6, ac_7, ac_8, ac_9, ac_10, ac_11, ac_12, ac_13, ac_14, ac_15, ac_16, ac_17 };
        chordwise = new Vector3[n];

        // GET SPANWISE LOCATIONS
        for (int i = 0; i < n; i++)
        {
            y[i] = ac[i].transform.localPosition.x;
        }

        // PERFORM 3D FINITE AND SWEPT WING LIFT SLOPE REDUCTION
        for (int i = 0; i < n; i++)
        {
            // Wing
            CLa[i] = (Cla[i] * Mathf.Cos(Mathf.Deg2Rad * sweep[i])) / (Mathf.Pow(Mathf.Sqrt(1 + ((Cla[i] * Mathf.Cos(Mathf.Deg2Rad * sweep[i])) / (Mathf.PI * AR))), 2) + ((Cla[i] * Mathf.Cos(Mathf.Deg2Rad * sweep[i])) / (Mathf.PI * AR)));
        }
        // Vertical Tail
        CLa_vt = (Cla_vt * Mathf.Cos(Mathf.Deg2Rad * sweep_vt)) / (Mathf.Pow(Mathf.Sqrt(1 + ((Cla_vt * Mathf.Cos(Mathf.Deg2Rad * sweep_vt)) / (Mathf.PI * AR_vt))), 2) + ((Cla_vt * Mathf.Cos(Mathf.Deg2Rad * sweep_vt)) / (Mathf.PI * AR_vt)));

        // BUILD LIFT SLOPE CURVES
        for (int i = 0; i < alpha_data.Length; i++)
        {
            liftCurve_23021.AddKey(alpha_data[i], cl_23021[i]);
            liftCurve_35124.AddKey(alpha_data[i], cl_35124[i]);
            liftCurve_fp.AddKey(alpha_data[i], cl_fp[i]);
            dragCurve_23021.AddKey(alpha_data[i], cdp_23021[i]);
            dragCurve_35124.AddKey(alpha_data[i], cdp_35124[i]);
            dragCurve_fp.AddKey(alpha_data[i], cdp_fp[i]);
            momentCurve_23021.AddKey(alpha_data[i], cm_23021[i]);
            momentCurve_35124.AddKey(alpha_data[i], cm_35124[i]);
        }

        // EXTRANEOUS
        rigidBody = GetComponent<Rigidbody>();
        thrustVector = transform.Find("vectorThrust").transform;
        gustVector = new Vector3(Mathf.Sin(Random.Range(0, 2 * Mathf.PI)), 0, Mathf.Cos(Random.Range(0, 2 * Mathf.PI)));

        for (int i = 0; i < n; i++)
        {
            ac[i].transform.localEulerAngles = new Vector3(ac[i].transform.localEulerAngles.x, 0, ac[i].transform.localEulerAngles.z);
        }
    }

    void Update()
    {
        GetComponent<propeller>().throttle = inputThrottle;
        elevonLeft.GetComponent<controlSurface>().deflection = inputPitch + inputRoll;
        elevonRight.GetComponent<controlSurface>().deflection = inputPitch - inputRoll;
    }

    void FixedUpdate ()
    {
        // SET VEHICLE WEIGHT
        rigidBody.mass = (W_empty + W_payload) * 0.453592f;

        // SET MAXIMUM THRUST
        thrustMax = thrustMax_lbf * 4.44822f;

        // GET FREESTREAM & MODEL GUSTS
        gustVector = Vector3.Slerp(gustVector, new Vector3(Mathf.Sin(Random.Range(0f, 2f * Mathf.PI)), 0, Mathf.Cos(Random.Range(0f, 2f * Mathf.PI))), gustUnsteadiness * Time.fixedDeltaTime);
        gust = gustVector * gustMagnitude;
        freestream = rigidBody.velocity - gust;
        U_inf = freestream.magnitude;
        alt = transform.position.y;
        cg = rigidBody.centerOfMass;

        // UPDATE STATIC MARGIN
        SM = 100 * (0.2748407f + cg.z) / 0.28556204f;

        // CALCULATE VEHICLE ANGLE OF ATTACK & SIDESLIP
        alpha = Vector3.SignedAngle(transform.forward, Vector3.ProjectOnPlane(freestream, transform.right), transform.right);
        beta = Vector3.SignedAngle(transform.forward, Vector3.ProjectOnPlane(freestream, transform.up), transform.up);

        // CALCULATE ELEVON DEFLECTIONS
        delta_elevon_l = inputPitch + inputRoll;
        delta_elevon_r = inputPitch - inputRoll;

        // CALCULATE ELEVON DEFLECTION ANGLE OF ATTACK SHIFT
        alpha_delta_elevon_l = cs_effectiveness * delta_elevon_l * (alpha_0L[0] / 1.6f);
        alpha_delta_elevon_r = cs_effectiveness * delta_elevon_r * (alpha_0L[16] / 1.6f);
        alpha_delta = new float[] { alpha_delta_elevon_l, alpha_delta_elevon_l, alpha_delta_elevon_l, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, alpha_delta_elevon_r, alpha_delta_elevon_r, alpha_delta_elevon_r };

        // CALCULATE FORCES & MOMENTS
        // Vertical Tails
        chordwise_vt_l = Vector3.ProjectOnPlane(rigidBody.GetPointVelocity(ac_vt_l.position) - gust, transform.up);
        chordwise_vt_r = Vector3.ProjectOnPlane(rigidBody.GetPointVelocity(ac_vt_r.position) - gust, transform.up);
        Re_vt = (rho * U_inf * mac_vt) / mu;

        // Lift
        //CL_vt = CLa_vt * beta; 
        CL_vt = liftCurve_fp.Evaluate(beta); // REALISM: Calculate BETA from point velocity to improve.
        CL_vt = (CL_vt * Mathf.Cos(Mathf.Deg2Rad * sweep_vt)) / (Mathf.Pow(Mathf.Sqrt(1 + ((CL_vt * Mathf.Cos(Mathf.Deg2Rad * sweep_vt)) / (Mathf.PI * AR_vt))), 2) + ((CL_vt * Mathf.Cos(Mathf.Deg2Rad * sweep_vt)) / (Mathf.PI * AR_vt)));
        L_vt = CL_vt * q_inf * S_vt;

        // Drag
        CDp_vt = dragCurve_fp.Evaluate(beta);
        //Di_vt = fCDi * (Mathf.Pow(CL_vt, 2) / (Mathf.PI * AR_vt)) * q_inf * S_vt;
        Di_vt = CL_vt * q_inf * S_vt;
        Df_vt = (0.664f / Mathf.Sqrt(Re_vt)) * q_inf * S_wetted_vt;
        Dp_vt = CDp_vt * q_inf * S_vt;
        D_vt = Di_vt + Df_vt + Dp_vt;

        // Wings
        q_inf = 0.5f * rho * Mathf.Pow(U_inf, 2);
        for (int i = 0; i < n; i++)
        {
            chordwise[i] = Vector3.ProjectOnPlane(rigidBody.GetPointVelocity(ac[i].position) - gust, ac[i].right);
            AoA[i] = Vector3.SignedAngle(ac[i].forward, chordwise[i], ac[i].right);
            AoA[i] = AoA[i] + alpha_geo[i] + alpha_delta[i];
            Re[i] = (rho * U_inf * c[i]) / mu;

            // Lift
            // CL[i] = CLa[i] * Mathf.Deg2Rad * (AoA[i] - alpha_0L[i] + alpha_geo[i] + alpha_delta[i]); OLD LIFT COEFFICIENT.
            if (airfoil[i] == 23021) { CL[i] = liftCurve_23021.Evaluate(AoA[i]); }
            else if (airfoil[i] == 35124) { CL[i] = liftCurve_35124.Evaluate(AoA[i]); }
            CL[i] = (CL[i] * Mathf.Cos(Mathf.Deg2Rad * sweep[i])) / (Mathf.Pow(Mathf.Sqrt(1 + ((CL[i] * Mathf.Cos(Mathf.Deg2Rad * sweep[i])) / (Mathf.PI * AR))), 2) + ((CL[i] * Mathf.Cos(Mathf.Deg2Rad * sweep[i])) / (Mathf.PI * AR)));
            L[i] = CL[i] * q_inf * S[i];

            // Drag
            if (airfoil[i] == 23021) { CDp[i] = dragCurve_23021.Evaluate(AoA[i]); }
            else if (airfoil[i] == 35124) { CDp[i] = dragCurve_35124.Evaluate(AoA[i]); }
            //Di[i] = fCDi * (Mathf.Pow(CL[i], 2) / (Mathf.PI * AR)) * q_inf * S[i]; OLD INDUCED DRAG COMPONENT.
            if (i == 0) Di[i] = (CL[i] / Mathf.Abs(CL[i])) * Mathf.Abs((CL[i] * q_inf * S[i]) - Di_vt);
            else if (i == n-1) Di[i] = (CL[i] / Mathf.Abs(CL[i])) * Mathf.Abs((CL[i] * q_inf * S[i]) - Di_vt);
            else Di[i] = (CL[i] / Mathf.Abs(CL[i]))*Mathf.Abs(CL[i] - CL[i - 1]) * q_inf * S[i];
            Df[i] = (0.664f / Mathf.Sqrt(Re[i])) * q_inf * S_wetted[i];
            Dp[i] = CDp[i] * q_inf * S[i];
            D[i] = Di[i] + Df[i] + Dp[i];

            // Pitching Moment
            if (airfoil[i] == 23021) { CM[i] = -momentCurve_23021.Evaluate(AoA[i]); }
            else if (airfoil[i] == 35124) { CM[i] = -momentCurve_35124.Evaluate(AoA[i]); }
            M[i] = CM[i] * q_inf * S[i] * c[i];
        }

        // APPLY FORCES & MOMENTS
        // Apply Thrust Force
        thrust = Mathf.Pow(inputThrottle, 2) * thrustMax;
        rigidBody.AddForceAtPosition(thrustVector.forward * thrust, thrustVector.position);

        // Wings
        for (int i = 0; i < n; i++)
        {
            // Forces
            rigidBody.AddForceAtPosition(Vector3.Cross(chordwise[i], ac[i].right).normalized * L[i], ac[i].position);
            if (!float.IsNaN(D[i])) { rigidBody.AddForceAtPosition(-chordwise[i].normalized * D[i], ac[i].position); }

            // Moments
            rigidBody.AddTorque(ac[i].right * M[i]);
        }

        // Vertical Tails
        rigidBody.AddForceAtPosition(Vector3.Cross(chordwise_vt_l, ac_vt_l.right).normalized * L_vt, ac_vt_l.position);
        rigidBody.AddForceAtPosition(Vector3.Cross(chordwise_vt_r, ac_vt_r.right).normalized * L_vt, ac_vt_r.position);
        if (!float.IsNaN(D_vt))
        {
            //rigidBody.AddForceAtPosition(-chordwise_vt_l.normalized * (-Di_vt + Df_vt + Dp_vt), ac_vt_l.position);
            //rigidBody.AddForceAtPosition(-chordwise_vt_r.normalized * (Di_vt + Df_vt + Dp_vt), ac_vt_r.position);
            rigidBody.AddForceAtPosition(-chordwise_vt_l.normalized * Mathf.Abs(D_vt), ac_vt_l.position);
            rigidBody.AddForceAtPosition(-chordwise_vt_r.normalized * Mathf.Abs(D_vt), ac_vt_r.position);
        }

        // SHOW VECTORS
        for (int i = 0; i < n; i++)
        {
            ac[i].GetComponent<acVectors>().freestreamVector = chordwise[i];
            ac[i].GetComponent<acVectors>().liftVector = Vector3.Cross(chordwise[i], ac[i].right).normalized;
            ac[i].GetComponent<acVectors>().dragVector = -chordwise[i].normalized;

            ac[i].GetComponent<acVectors>().freestreamMagnitude = U_inf / 100;
            ac[i].GetComponent<acVectors>().liftMagnitude = L[i];
            ac[i].GetComponent<acVectors>().dragMagnitude = D[i];
        }
        ac_vt_l.GetComponent<acVectors>().freestreamVector = chordwise_vt_l;
        ac_vt_l.GetComponent<acVectors>().liftVector = Vector3.Cross(chordwise_vt_l, ac_vt_l.right).normalized;
        ac_vt_l.GetComponent<acVectors>().dragVector = -chordwise_vt_l.normalized;

        ac_vt_l.GetComponent<acVectors>().freestreamMagnitude = U_inf / 100;
        ac_vt_l.GetComponent<acVectors>().liftMagnitude = L_vt;
        //ac_vt_l.GetComponent<acVectors>().dragMagnitude = -Di_vt + Df_vt + Dp_vt;
        ac_vt_l.GetComponent<acVectors>().dragMagnitude = Mathf.Abs(D_vt);

        ac_vt_r.GetComponent<acVectors>().freestreamVector = chordwise_vt_r;
        ac_vt_r.GetComponent<acVectors>().liftVector = Vector3.Cross(chordwise_vt_r, ac_vt_r.right).normalized;
        ac_vt_r.GetComponent<acVectors>().dragVector = -chordwise_vt_r.normalized;

        ac_vt_r.GetComponent<acVectors>().freestreamMagnitude = U_inf / 100;
        ac_vt_r.GetComponent<acVectors>().liftMagnitude = L_vt;
        //ac_vt_r.GetComponent<acVectors>().dragMagnitude = Di_vt + Df_vt + Dp_vt;
        ac_vt_r.GetComponent<acVectors>().dragMagnitude = Mathf.Abs(D_vt);
    }
}
