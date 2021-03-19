using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throttle_lever_script : MonoBehaviour
{
    // the gameobject with flying attached to it
    public GameObject plane;

    // the propeller; this is used to spin it dynamically based on thrust. After all, it is the engine we're controlling.
    public GameObject prop;

    public float max_angle;
    public float max_value;
    public float min_value = 0.0f;
    public float engine_responsiveness = .25f;
    public float min_angle;
    public float angle;
    public float actual_thrust;
    public float wanted_thrust;
    public Vector3 rot;
    // Start is called before the first frame update
    void Start()
    {
        //min_angle = transform.localEulerAngles.z;
        angle = transform.localEulerAngles.z;
        actual_thrust = wanted_thrust = Mathf.Lerp(min_value, max_value, (transform.localEulerAngles.z - min_angle) / (max_angle - min_angle));
    }

    // Update is called once per frame
    void Update()
    {
        angle = transform.localEulerAngles.z;

        // dont let us go below 0 throttle or above full throttle
        if (transform.localEulerAngles.z < min_angle)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, min_angle);
        }
        if (transform.localEulerAngles.z > max_angle)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, max_angle);
        }
        // we want to be between min_thrust (probably 0) and max thrust
        // the percent we want is how 
        wanted_thrust = Mathf.Lerp(min_value, max_value,  (transform.localEulerAngles.z - min_angle) / (max_angle - min_angle));
        actual_thrust = Mathf.MoveTowards(actual_thrust, wanted_thrust, engine_responsiveness);

        plane.GetComponent<flying>().thrust = actual_thrust;
        // TODO set the prop here too.
    }
}
