using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class vr_adjust_axis : MonoBehaviour
{
    // Used to determine if someone is grabbing the item or not
    public SteamVR_Action_Boolean grabAction;
    // doesn't matter what hand you use, but still needs to be assigned; should be 'any'
    public SteamVR_Input_Sources leftController, rightController;
    // We're gonna need positions
    public SteamVR_Behaviour_Pose controllerPoseLeft, controllerPoseRight;
    public SteamVR_Behaviour_Pose currentTouchingController;
    // We can move in x, y, z, or a combo thereof;

    public GameObject bottom;

    public Vector3 top_pos;
    public GameObject plane;
    // Start is called before the first frame updatethrottle
    void Start()
    {
        // I can't think of anything to do here...we'll see
        currentTouchingController = null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // we split this up because it matters *which* controller grabs
        if (currentTouchingController && grabAction.GetState(currentTouchingController.inputSource))
        {
            top_pos = currentTouchingController.transform.position - bottom.transform.position;
            transform.rotation = Quaternion.LookRotation(-Vector3.Cross(plane.transform.forward, plane.transform.up),
                top_pos
            );
        }
        else if (currentTouchingController)
        {
            // we let the thing go
            Debug.Log("let go");
            currentTouchingController = null;
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
