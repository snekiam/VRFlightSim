using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class adjust_dial : MonoBehaviour
{
    public float dial_value;
    public GameObject dial;
    public bool clockwise = false;
    public Vector2[] waypoints;
    private float starting_rotation = 0.0f;
    public float current_rotation = 0.0f;
    public bool loop = false;
    // Start is called before the first frame update
    void Start()
    {
        current_rotation = starting_rotation = dial.transform.rotation.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        // positive numbers are easier to think about
        if (loop)
        {
            dial_value = dial_value % waypoints[waypoints.Length - 1].x;
        }

        for (int i = 1; i < waypoints.Length; i++)
        {
            if (dial_value <= waypoints[i].x && waypoints[i - 1].x < dial_value)
            {
                current_rotation = Mathf.Lerp(waypoints[i - 1].y, waypoints[i].y, (dial_value - waypoints[i-1].x) / (waypoints[i].x - waypoints[i-1].x));
            }
        }

        if (dial_value < 1)
        {
            current_rotation = starting_rotation;
        }
        if (dial_value >= waypoints[waypoints.Length - 1].x)
        {
            current_rotation = waypoints[waypoints.Length - 1].y;
        }
        //current_rotation = starting_rotation + (dial_value * degrees_per_unit);

        dial.transform.localEulerAngles = new Vector3(0, 0, current_rotation);
    }
}

