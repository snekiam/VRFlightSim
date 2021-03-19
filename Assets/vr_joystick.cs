using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class vr_joystick : MonoBehaviour
{
    // Used to determine if someone is grabbing the item or not
    public SteamVR_Action_Boolean grabAction;
    // We're gonna need positions
    public SteamVR_Behaviour_Pose currentTouchingController;

    public Vector3 top_pos;

    public Vector3 up;

    public Vector3 forward;

    public GameObject plane;
    public Vector3 localtop;
    public float z;
    public float x;
    public Vector3 initial_rotation;

    public Vector3 current_angles;
    public float z_corrected;
    private Rigidbody planeBody;
    private float max_left_right = 20.0f;

    public Vector3 local_controller_pos;
    // Start is called before the first frame updatethrottle
    private void Start()
    {
        initial_rotation = transform.localEulerAngles;
        planeBody = plane.GetComponentInParent<Rigidbody>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        forward = transform.forward;
        up = transform.up;
        current_angles = transform.localEulerAngles;
        if (currentTouchingController && grabAction.GetState(currentTouchingController.inputSource))
        {
            top_pos = currentTouchingController.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(-Vector3.Cross(plane.transform.forward, top_pos), 
                top_pos
            );
            x = transform.localEulerAngles.y > 90 ? 90f - transform.localEulerAngles.x + 90 : transform.localEulerAngles.x;
            x = (x - 65f) / 50.0f;
            //z = transform.localEulerAngles.x > 90 ? -1 * (200 - transform.localEulerAngles.x) / 40.0f : transform.localEulerAngles.x;
            if (transform.localEulerAngles.y > 90)
            {
                // forward
                x = 90f - transform.localEulerAngles.x + 90;
                z = (200 - transform.localEulerAngles.z) / 40.0f;
            }
            else
            {
                // back
                x = transform.localEulerAngles.x;
                z = transform.localEulerAngles.z;
                if (z > 180)
                {
                    // to the right
                    z = (((360.0f - z) / (max_left_right * 2))) + 0.5f;
                }
                else
                {
                    // to the left 
                    z = (max_left_right - z) / (max_left_right * 2);
                }
            }
            x = (x - 65f) / 50.0f;
            planeBody.AddRelativeTorque(new Vector3(Mathf.Lerp(-.2f, .2f, x), Mathf.Lerp(-.1f, .1f, z), Mathf.Lerp(.2f, -.2f, z)));
        }
        else if (currentTouchingController)
        {
            // we let the thing go
            Debug.Log("let go");
            currentTouchingController = null;
            transform.localEulerAngles = initial_rotation;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other);
        if (other.gameObject.CompareTag("controller") && currentTouchingController == null)
        {
            currentTouchingController = other.gameObject.GetComponent<SteamVR_Behaviour_Pose>();
        }
    }

}
