using CartoonFX;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private int maxCombo = 0;
    public float comboTimer = 0f;
    public float comboDuration = 3f;

    public GameState state = GameState.Normal;

    public TextMeshProUGUI text_PreScore;
    public TextMeshProUGUI text_PlusScore;
    public TextMeshProUGUI text_Timer;

    public TextMeshProUGUI text_PreCombo;
    public TextMeshProUGUI text_MaxCombo;

    public Text text_GameOverScore;

    public Slider slider_Timer;
    public Slider slider_ComboTimer;
    public GameObject panel_GameOver;

    public GameObject comboEffect;

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

        slider_ComboTimer.gameObject.SetActive(false);

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
        UpdateScore(s);
    }

    void UpdateScore(int s)
    {
        text_PlusScore.text = "+" + s.ToString("N0");
        text_PreScore.text = score.ToString("N0");
    }
    void UpdateCombo()
    {
        text_PreCombo.text = combo.ToString();
        text_MaxCombo.text = maxCombo.ToString();
    }

    void UpdateTime()
    {
        time -= Time.deltaTime; 
        if (time < 0) time = 0;

        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);

        text_Timer.text = string.Format("[{0:00}:{1:00}]", minutes, seconds);
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
        text_GameOverScore.text = "최종 점수: " + score + "남은 시간: " + time.ToString("[00.00]");        
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
            slider_ComboTimer.value = comboTimer;

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

        slider_ComboTimer.gameObject.SetActive(false);
    }

    public void AddCombo()
    {
        slider_ComboTimer.gameObject.SetActive(true);

        combo++;
        comboTimer = comboDuration;

        if (maxCombo < combo)
            maxCombo = combo;

        UpdateCombo();
    }

    public void playComboEffect(Vector3 pos)
    {        
        if (combo < 2)
            return;        

        GameObject obj = Instantiate(comboEffect, pos, Quaternion.identity);

        CFXR_ParticleText text = obj.GetComponent<CFXR_ParticleText>();

        text.UpdateText(combo + " COMBO");

        var renderers = obj.GetComponentsInChildren<ParticleSystemRenderer>();

        foreach (var r in renderers)
        {
            r.sortingLayerName = "TargetBox";
            r.sortingOrder = 10;
        }

        Destroy(obj,1f);
    }
}
