using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attitude_indicator : MonoBehaviour
{
    public GameObject background;
    public GameObject innerCircle;
    public GameObject outerRing;

    // for my project, piper_parent
    public GameObject plane;

    public float full_up_y;
    public float full_down_y;

    private float roll;
    private float pitch;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // TODO: Fix pitching up
        // TODO: Move rotation point to match pitch
        roll = plane.transform.localEulerAngles.z;
        pitch = plane.transform.localEulerAngles.x;
        if (pitch > 180)
        {
            pitch = -360 + pitch;
        }
        background.transform.localEulerAngles = new Vector3(0, 0, -roll);
        innerCircle.transform.localEulerAngles = new Vector3(0, 0, -roll);
        outerRing.transform.localEulerAngles = new Vector3(0, 0, -roll);
        float pitch_adjustment = Mathf.Abs((pitch + 30.0f) / 60.0f);
        innerCircle.transform.localPosition = new Vector3(0, Mathf.Lerp(full_down_y, full_up_y, pitch_adjustment), 0);
    }
}
