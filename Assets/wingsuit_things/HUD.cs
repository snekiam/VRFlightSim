using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HUD : MonoBehaviour
{
    Vector3 startScale;
    bool hud_visible = false;
    public Vector3 startPos;
    float prev_y = 0.0f;

    public SteamVR_Input_Sources hudController; //1
    public SteamVR_Action_Boolean openhudAction; //3


    // Start is called before the first frame update
    void Start()
    {
        startScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        // controls
        if (openhudAction.GetState(hudController))
        {
            GetComponent<Renderer>().enabled = true;
        } else
        {
            GetComponent<Renderer>().enabled = false;
        }



        transform.localScale = startScale;
        // could increase size if we are pointing at something...
        float fps = 1.0f / Time.deltaTime;
        Vector3 velocity = (controllermove.velocity * 1.0f) * 1.0f * fps * 360.0f;
        float v_mag = Vector3.Magnitude(velocity);
        GetComponent<TextMesh>().text = string.Format("x: {0:#000.0000} ", velocity.x) +
            string.Format(", y: {0:000.0000}", velocity.y) +
            string.Format(", z: {0:000.0000}\n", velocity.z) +
            string.Format("magnitude: {0:000.0000}\n", v_mag) +
            string.Format("aoa: {0:0.00}\n", controllermove.aoa) +
            string.Format("magnitude: {0:000.0000}\n", v_mag) +
            string.Format("Lift Percentage: {0:000.00}%\n", controllermove.up_amount / controllermove.start_up_amount * 100.0f) +
            string.Format("Air Density Percentage: {0:000.00}%\n", controllermove.drag_coefficient / controllermove.start_drag_coefficient * 100.0f) +
            string.Format("Max Speed Percentage: {0:000.00}%", controllermove.start_drag_coefficient / controllermove.drag_coefficient * 100.0f);
        prev_y = velocity.y;
        assign();

    }

    private void assign()
    {
        this.transform.rotation = Camera.main.transform.rotation;
        this.transform.position = startPos + Camera.main.transform.position + Camera.main.transform.forward * 10.0f;
    }

    private void OnGUI()
    {
        Event e = Event.current;
        if (e.isKey && e.keyCode == KeyCode.E && e.type == EventType.KeyUp)
        {
            hud_visible = !hud_visible;
        }

        if (hud_visible)
        {
            //GetComponent<Renderer>().enabled = true;
        }
        else
        {
            //GetComponent<Renderer>().enabled = false;
        }
    }
}
