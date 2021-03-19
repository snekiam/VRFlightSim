using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class eject : MonoBehaviour
{
    public GameObject windshieldObject;
    public GameObject player;

    public GameObject[] toEnableForWingsuit;

    public SteamVR_Action_Boolean eject_button;

    public float eject_force;
    public float eject_start_time = -1;
    public float eject_wait_time = 3.0f;
    public float wingsuit_deploy_time = 1.0f;

    private bool eject_pressed = false;

    private bool eject_in_progress = false;
    private bool player_eject_in_progress = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (eject_in_progress && (eject_start_time + eject_wait_time) < Time.time)
        {
            Player_Eject();
            eject_in_progress = false;
        }

        if (player_eject_in_progress && (eject_start_time + eject_wait_time) < Time.time)
        {
            DeployWingsuit();
            player_eject_in_progress = false;
        }
        if (!eject_pressed && GetComponent<vr_joystick>().currentTouchingController != null && eject_button.GetState(GetComponent<vr_joystick>().currentTouchingController.inputSource))
        {
            Eject();
            eject_pressed = true;
            eject_in_progress = true;
        }

    }

    void DeployWingsuit()
    {
        foreach(GameObject g in toEnableForWingsuit){
            g.SetActive(true);
        }
        // we use our own physics
        Destroy(player.GetComponent<Rigidbody>());
        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<controllermove>().enabled = true;

    }

    // Actually shoot the player out
    void Player_Eject()
    {
        player.transform.SetParent(null);
        player.AddComponent<Rigidbody>();
        Rigidbody playerBody = player.GetComponent<Rigidbody>();
        playerBody.isKinematic = false;
        playerBody.AddForce(player.transform.up * eject_force);
        player_eject_in_progress = true;
        eject_start_time = Time.time;
    }
    // shoots windshield out, swaps to wingsuit
    void Eject()
    {
        eject_start_time = Time.time;
        windshieldObject.transform.SetParent(null);
        windshieldObject.AddComponent<Rigidbody>();
        Rigidbody rigidbody = windshieldObject.GetComponent<Rigidbody>();
        rigidbody.isKinematic = false;
        rigidbody.AddForce(windshieldObject.transform.up * eject_force);
    }
}
