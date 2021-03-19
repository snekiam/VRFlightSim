using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class rudder_control : MonoBehaviour
{
    public SteamVR_Input_Sources controller;

    public SteamVR_Action_Vector2 rudder_input;

    private Rigidbody planeBody;

    // Start is called before the first frame update
    void Start()
    {
        planeBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = rudder_input.GetAxis(controller).x;
        float z = rudder_input.GetAxis(controller).y;
        x /= 3.0f;
        planeBody.AddRelativeTorque(new Vector3(0f, x, 0f));

    }
}
