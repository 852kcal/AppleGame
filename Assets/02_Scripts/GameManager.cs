using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum GameState
{
    Normal,
    Dragging,
    Remove,
    Shuffle,
    GameOver
}

public class GameManager : MonoBehaviour
{    
    private int score = 0;
    private float time = 0f;

    public int combo = 0;
    public float comboTimer = 0f;
    public float comboDuration = 3f;

    public GameState state = GameState.Normal;

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
        SetState(GameState.Normal);

        score = 0;
        time = 120;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime();
        CheckTimeOver();
        CheckCombo();
    }

    public void SetState(GameState newState)
    {
        state = newState;
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

        SetState(GameState.GameOver);

        panel_GameOver.SetActive(true);
        text_GameOverScore.text = "최종 점수: " + score + "남은 시간: " + time.ToString("000.00");        
    }

    public void UseItem(int idx)
    {
        switch (idx)
        {
            case 0:
                List<Apple> apple_list = GridManager.Instance.FindOneValidPackage();

                //연출
                foreach (Apple apple in apple_list)
                {                    
                    apple.GetMeshRender().enabled = true;
                    apple.OnOutline();
                }

                break;
            case 1:
                GridManager.Instance.RerollNumbers();
                break;
            case 2:
                UseRemoveItem();                
                break;
            default:
                break;
        }
    }

    public void UseRemoveItem()
    {
        SetState(GameState.Remove);
    }

    public void CheckCombo()
    {
        if(combo > 0)
        {
            comboTimer -= Time.deltaTime;

            if (comboTimer <= 0)
            {
                ResetCombo();
            }
        }
    }

    public void ResetCombo()
    {
        combo = 0;
        comboTimer = comboDuration;

        Debug.Log("Combo reset");
    }

    public void AddCombo()
    {
        combo++;
        comboTimer = comboDuration - (float)combo*0.1f;
        
        Debug.Log("Combo: " + combo + " Timer:" + comboTimer);
    }
}
