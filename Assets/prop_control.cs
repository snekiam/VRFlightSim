using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prop_control : MonoBehaviour
{
    public GameObject prop;
    public GameObject plane;

    public float constant = 5.0f;
    public float dir = -1.0f;
    public float multiplier;

    private flying flying;

    // Start is called before the first frame update
    void Start()
    {
        flying = plane.GetComponent<flying>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // can't use rigidbody in a child...kinda stinks but whatever.
        prop.transform.localEulerAngles += new Vector3(0, 0, (constant + flying.thrust) * multiplier * dir);
    }
}
