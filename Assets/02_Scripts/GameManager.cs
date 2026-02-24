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

    public Text text_GameOverScore;

    public Slider slider_Timer;
    public GameObject panel_GameOver;

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
        time = 120;
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
        text_Score.text = "점수: " + score;
    }

    void UpdateTime()
    {
        time -= Time.deltaTime; 
        if (time < 0) time = 0;
        text_Timer.text = time.ToString("000.00");
        slider_Timer.value = time;
    }

    void CheckTimeOver()
    {
        if (time <= 0)        
        {
            GameOver();
        }
    }
    
    void GameOver()
    {
        Debug.Log("Game Over!");

        text_GameOverScore.text = "최종 점수: " + score + "남은 시간: " + time.ToString("000.00");

        panel_GameOver.SetActive(true);

        Time.timeScale = 0f;        
    }
}
