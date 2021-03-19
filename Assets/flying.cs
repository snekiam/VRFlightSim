using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;


public class flying : MonoBehaviour
{
    public float aoa = 0.0f;
    public float vel_mag = 0.0f;

    public float drag_coefficient = 100f;
    public float forward_amount = 0.65f;
    public float up_amount = 128f;
    public float thrust = 0.0f;

    public float start_drag_coefficient = 100f;
    public float start_forward_amount = 0.75f;
    public float start_up_amount = 128f;

    public Quaternion startRot;


    public Vector3 startPos;
    public Vector3 velocity = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector3 up;
    public Vector3 forward;
    public Vector3 rightVector;
    public Vector3 leftVector;

    // when this hits something, we crash - not sure what to do with this yet.
    public GameObject fuselage;

    public GameObject airspeed_indicator;
    public GameObject altimeter;
    public GameObject vertical_speed_indicator;


    public SteamVR_Input_Sources leftController; //1
    public SteamVR_Input_Sources rightController; //1

    public SteamVR_Action_Vector2 lift_drag;
    public SteamVR_Action_Vector2 movement;
    public SteamVR_Action_Single speed_up;
    public SteamVR_Action_Single speed_down;
    public SteamVR_Behaviour_Pose controllerPoseLeft;
    public SteamVR_Behaviour_Pose controllerPoseRight;

    public Quaternion stick_input;
    public Vector3 rot_angles;
    public Vector3 Ff;
    // private stuff
    private adjust_dial airspeed_dial_script;
    private adjust_dial[] altimeter_scripts;
    private adjust_dial vertical_speed_indicator_script;
    float ad=0, ws=0, ws_x = 0, ws_y = 0;
    float sensitivity = 100;

    bool crashed = false;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
        start_drag_coefficient = drag_coefficient;
        start_forward_amount = forward_amount;
        start_up_amount = up_amount;
        airspeed_dial_script = airspeed_indicator.GetComponent<adjust_dial>();
        altimeter_scripts = altimeter.GetComponents<adjust_dial>();
        vertical_speed_indicator_script = vertical_speed_indicator.GetComponent<adjust_dial>();
    }

    // Update is called once per frame
    void Update()
    {

        ad = Input.GetAxis("Horizontal");
        ws = Input.GetAxis("Vertical");
        float throttle = Input.GetAxis("Mouse Y") / 100.0f;
        ws_x += ad * Time.deltaTime;
        ws_y -= ws * Time.deltaTime;
        thrust += throttle * Time.deltaTime;
        thrust = Mathf.Abs(thrust);

        calc_forces();
        // will need to go m/s -> knots for accurate readings
        // xz magnitude * 100 * 3.6 / 1.944 to get (approx) knots
        // Dot with forward since pitot tubes point forward
        // * 2.5f to look better; we want the stall line to *approximately* line up
        airspeed_dial_script.dial_value = Vector3.Dot(transform.forward, Vector3.Normalize(velocity)) * vel_mag * 3.6f * 1.5f;
        foreach (adjust_dial adjust_Dial in altimeter_scripts)
        {
            adjust_Dial.dial_value = transform.position.y * 3.281f;
        }

        // calculate knots going down, then convert to feet/minute
        // we need to add 20 since the VSI script can't handle negative numbers, and actualls is 0~40 instead of -20~20
        vertical_speed_indicator_script.dial_value = (velocity.y * 100f * 3.6f / 1.944f / 101.269f) + 20.0f;

        if (!crashed)
        {
            // Velocity * time = position
          transform.position = transform.position + velocity * Time.deltaTime / 2.0f;
        }
    }

    void calc_forces()
    {
        up = transform.up;
        forward = transform.forward;
        Vector3 norm_vel = Vector3.Normalize(velocity);
        aoa = Vector3.Dot(forward, norm_vel);
        if (aoa < 0)
        {
            aoa = 0;
        }

        vel_mag = Vector3.Magnitude(velocity);

        Vector3 FG = new Vector3(0, -9.81f, 0);

        float fwd_drag = drag_coefficient;
        float side_drag = drag_coefficient * 2.0f;
        float total_drag = fwd_drag * aoa + side_drag * (1.0f - aoa);


        Vector3 FD = -1 * norm_vel * vel_mag * vel_mag * total_drag;

        Ff = forward * Vector3.Magnitude(FG) * forward_amount * Mathf.Sin(norm_vel.y * Mathf.PI);

        Vector3 Fl = up * vel_mag * vel_mag * (up_amount * Vector3.Dot(transform.up, Vector3.up));

        Vector3 Fe = forward * thrust;

        // These are forces, so * time to get velocity
        velocity += (FG + FD + Ff + Fl + Fe) * Time.deltaTime;// + Fe;
    }

    public void crash()
    {
        crashed = true;
    }
}
