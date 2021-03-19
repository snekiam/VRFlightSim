using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.UIElements;

public class controllermove_old : MonoBehaviour
{
    // Mountain height is 200.
    // Start is called before the first frame update
    public CharacterController controller;
    public static Vector3 velocity = new Vector3(0.0f, 0.0f, 0.0f);
    public static float aoa = 0.0f;

    public float speed = 3.0f;
    // transformation of the player character
    public Transform groundCheck;

    // radius where unity will check around our object
    public float collisionRadius = 0.1f;

    // everything we're colliding with
    public LayerMask groundMask;
    bool touchingGround;

    public static float vel_mag = 0.0f;
    public static float drag_coefficient = 100f;
    public static float forward_amount = 0.65f;
    public static float up_amount = 128f;
    public float sensitivity = 100.0f;

    public float ad;
    public float ws;

    float ws_x = 0.0f, ws_y = 0.0f;

    bool paused = false;

    public Vector3 startPos;
    public Quaternion startRot;
    public static float start_drag_coefficient = 100f;
    public static float start_forward_amount = 0.75f;
    public static float start_up_amount = 128f;
    public float start_sensitivity = 100.0f;

    public SteamVR_Input_Sources leftController; //1
    public SteamVR_Input_Sources rightController; //1

    public SteamVR_Action_Boolean grabAction; //3
    public SteamVR_Action_Vector2 lift_drag;
    public SteamVR_Action_Vector2 movement;
    public SteamVR_Action_Single speed_up;
    public SteamVR_Action_Single speed_down;
    public SteamVR_Behaviour_Pose controllerPoseLeft;
    public SteamVR_Behaviour_Pose controllerPoseRight;

    public Vector3 up;
    public Vector3 forward;

    public Vector3 rightVector;
    public Vector3 leftVector;

    public GameObject crosshair;

    void Start()
    {
        startPos = this.gameObject.transform.position;
        startRot = this.gameObject.transform.rotation;
        start_drag_coefficient = drag_coefficient;
        start_forward_amount = forward_amount;
        start_up_amount = up_amount;
        start_sensitivity = sensitivity;
        rightVector = Vector3.Normalize(Camera.main.transform.localPosition - controllerPoseRight.transform.localPosition);
        leftVector = Vector3.Normalize(Camera.main.transform.localPosition - controllerPoseLeft.transform.localPosition);
    }

    // Update is called once per frame
    void Update()
    {
        ad = Input.GetAxis("Horizontal");
        ws = Input.GetAxis("Vertical");
        rightVector = Vector3.Normalize(controllerPoseRight.transform.localPosition - Camera.main.transform.localPosition);
        leftVector =  Vector3.Normalize(controllerPoseLeft.transform.localPosition - Camera.main.transform.localPosition);
        //used for "max speed" (drag)
        float speed_adj = Input.GetAxis("speed");
        float lift_adj = Input.GetAxis("lift");
        float drag_adj = Input.GetAxis("drag");

        speed_adj += speed_up.GetAxis(rightController) - speed_down.GetAxis(leftController);
        lift_adj += lift_drag.GetAxis(leftController).y;
        drag_adj += lift_drag.GetAxis(leftController).x;

                if (!paused)
        {
            ws_x += ad * Time.deltaTime;
            ws_y -= ws * Time.deltaTime;
            Vector3 avg = Vector3.Normalize(((controllerPoseLeft.transform.localPosition + controllerPoseRight.transform.localPosition) / 2.0f) - Camera.main.transform.localPosition);
            transform.rotation = Quaternion.LookRotation(avg, Vector3.Cross(leftVector, rightVector));
            drag_coefficient -= speed_adj * Time.deltaTime * 45.0f;
            drag_coefficient += drag_adj * Time.deltaTime * 5;
            up_amount += lift_adj * Time.deltaTime * 5;
            calc_forces();
            controller.Move(velocity); // pos = pos + move
        }
    }

    void calc_forces() {
        up = transform.up;
        forward = transform.forward;
        Vector3 norm_vel = Vector3.Normalize(velocity);
        aoa = Vector3.Dot(forward, norm_vel);
        /* if (aoa < 0)
        {
            aoa = 0;
        }*/
        vel_mag = Vector3.Magnitude(velocity);

        Vector3 FG = new Vector3(0, -0.00981f * Time.deltaTime, 0);

        float fwd_drag = drag_coefficient;
        float side_drag = drag_coefficient * 2.0f;
        float total_drag = fwd_drag * aoa + side_drag * (1.0f - aoa);

        Vector3 FD = -1 * norm_vel * vel_mag * vel_mag * Time.deltaTime * total_drag;

        Vector3 Ff = forward * vel_mag * aoa * Time.deltaTime * forward_amount;
        Vector3 Fl = up * vel_mag * vel_mag * Time.deltaTime * up_amount;

        velocity += FG + FD + Ff + Fl;
    }
    private void OnGUI()
    {
        Event e = Event.current;
        if (e.isKey && e.keyCode == KeyCode.P &&
            e.type == EventType.KeyUp)
        {
            paused = !paused;
        }
        if (e.isKey && e.keyCode == KeyCode.R &&
            e.type == EventType.KeyUp)
        {
            paused = true;
            controller.enabled = false;
            velocity = new Vector3(0, 0, 0);
            this.gameObject.transform.position = startPos;
            this.gameObject.transform.rotation = startRot;
            aoa = 0.0f;
            drag_coefficient = start_drag_coefficient;
            forward_amount = start_forward_amount;
            up_amount = start_up_amount;
            sensitivity = start_sensitivity;
            controller.enabled = true;
            ad = 0;
            ws = 0;
            //Camera.main.GetComponent<mouselook>().Reset();
            paused = false;
        }
    }
}

