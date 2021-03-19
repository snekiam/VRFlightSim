using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game_logic : MonoBehaviour
{
    public GameObject[] rings;

    public Material clearedMat;

    private bool game_started = true;

    private flying plane;

    private ScoreIndicator scoreIndicator;

    private int clearedRings = 0;

    // Start is called before the first frame update
    void Start()
    {
        plane = GetComponent<flying>();
        scoreIndicator = GetComponentInChildren<ScoreIndicator>();
        scoreIndicator.NextRing(rings[clearedRings]);
    }

    void FixedUpdate()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ring") && game_started && !scoreIndicator.isGameOver())
        {
            clearedRings += 1;
            if (clearedRings == rings.Length)
            {
                // record the final score
                scoreIndicator.Finish();
                // end the game here too
                game_started = false;
            }

            if (scoreIndicator.NextRing(rings[clearedRings]))
            {
                // the player didn't run out of time if we get here
                other.gameObject.tag = "cleared_ring";
                other.gameObject.GetComponent<MeshRenderer>().material = clearedMat;
            }
        }
        else if (other.gameObject.CompareTag("throttle") || other.gameObject.CompareTag("controller") || other.gameObject.CompareTag("cleared_ring"))
        {
            return;
        }
        else
        {
            // we hit something..its game over.
            scoreIndicator.GameOver();
            plane.crash();
        }
    }
}
