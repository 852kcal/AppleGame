using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int score = 0;
    private float time = 0f;

    public Text text_Score;
    public Text text_Timer;

    public Slider slider_Timer;

    static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        time = 110;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime();
        CheckTimeOver();
    }

    public void AddScore(int s)
    {
        score += s;
        UpdateScore();
    }

    void UpdateScore()
    {
        text_Score.text = "SCORE: " + score;
    }

    void UpdateTime()
    {
        time += Time.deltaTime;
        text_Timer.text = time.ToString("000.00");
        slider_Timer.value = time;
    }

    void CheckTimeOver()
    {
        if (time >= slider_Timer.maxValue)
        {
            GameOver();
        }
    }
    
    void GameOver()
    {
        Debug.Log("Game Over!");
        Time.timeScale = 0f;        
    }
}
