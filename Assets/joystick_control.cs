using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class joystick_control : MonoBehaviour
{
    // item with flying script
    public GameObject plane;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion rot = transform.localRotation;
        rot.eulerAngles.Set(rot.eulerAngles.x - 90, rot.eulerAngles.y, rot.eulerAngles.z);
        Vector3 ang = transform.localEulerAngles;
        ang.x -= 90;
        plane.GetComponent<flying>().rot_angles = ang;
        plane.GetComponent<flying>().stick_input = rot;
    }
}
