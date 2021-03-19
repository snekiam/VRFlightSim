using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreIndicator : MonoBehaviour
{
    public GameObject timeIndicator;
    public GameObject scoreIndicator;
    public GameObject directionIndicator;

    public GameObject gameOverIndicator;
    public GameObject gameWinIndicator;

    public GameObject plane;

    public bool gameOver = false;

    public uint time = 0;
    public uint score = 0;
    public float direction = 0;

    // internal timer decremented every fixedUpdate; used to set time.
    private uint timer;

    bool started = false;

    private TextMeshProUGUI timeText;
    private TextMeshProUGUI scoreText;

    private Vector3 ringPosition;

    // Start is called before the first frame update
    void Start()
    {
        timeText = timeIndicator.GetComponent<TextMeshProUGUI>();
        scoreText = scoreIndicator.GetComponent<TextMeshProUGUI>();
    }

    // FixedUpdate is called 50x/second (every 0.02 seconds).
    void FixedUpdate()
    {
        gameOverIndicator.SetActive(gameOver);
        if (!gameOver)
        {
            timer -= 1;
            if (timer <= 0) {
                gameOver = true;
            }
            time = (uint)Mathf.CeilToInt(timer * .01f);
            timeText.text = time.ToString("000");
            scoreText.text = score.ToString();
            direction = Vector3.SignedAngle(ringPosition - plane.transform.position, plane.transform.forward,  Vector3.up);
            directionIndicator.transform.localEulerAngles = new Vector3(0f, 0f, direction + 45);
        }

    }

    public bool NextRing(GameObject ring, uint timeIncrement = 200)
    {
        if ((time < 1 || gameOver) && started)
        {
            gameOver = true;
            return false;
        }
        started = true;
        score += time;
        time = timeIncrement; 
        timer = timeIncrement * 100;
        ringPosition = ring.transform.position;
        return true;
    }

    public void Finish()
    {
        if (time < 1 || gameOver)
        {
            gameOver = true;
        }
        if (!gameOver)
        {
            score += time;
            gameWinIndicator.SetActive(true);
        }
    }

    public bool isGameOver()
    {
        return time < 1 || gameOver;
    }

    public void GameOver()
    {
        gameOver = true;
    }
}
