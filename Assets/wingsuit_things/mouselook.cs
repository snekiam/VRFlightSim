using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouselook : MonoBehaviour
{
    // Start is called before the first frame update
    float mouse_x = 0.0f, mouse_y = 0.0f;
    public float sensitivity = 100.0f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        mouse_x += mx * Time.deltaTime;
        mouse_y -= my * Time.deltaTime;

        //clamp mouse_y, since we can't rotate too much

        if(mouse_y > .8f)
        {
            mouse_y = .8f;
        }
        if(mouse_y < -.8f)
        {
            mouse_y = -.8f;
        }

        transform.localEulerAngles = new Vector3(mouse_y * sensitivity,
                                                 mouse_x * sensitivity,
                                                 0.0f);
    }

    public void Reset()
    {
        mouse_x = 0;
        mouse_y = 0;
        transform.localEulerAngles = new Vector3(0, 0, 0);
    }
}
