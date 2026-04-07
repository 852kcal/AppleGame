using CartoonFX;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
public enum GameState
{
    Main,
    Loading,
    Normal,
    Dragging,
    Remove,
    Shuffle,
    GameOver
}

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<GameManager>();
            return instance;
        }
    }

    [Header("Game Data & Logic")]
    private int bestScore = 0;
    private int bestCombo = 0;
    private int score = 0;
    private float time = 0f;    
    private int maxCombo = 0;
    public int combo = 0;
    public float comboTimer = 0f;
    public float comboDuration = 3f;
    public GameState state = GameState.Loading;

    [Header("Score & Combo UI (TMP)")]
    public TextMeshProUGUI text_PreScore;
    public TextMeshProUGUI text_PlusScore;
    public TextMeshProUGUI text_Timer;
    public TextMeshProUGUI text_PreCombo;
    public TextMeshProUGUI text_MaxCombo;

    [Header("Progress Bars")]
    public Slider slider_Timer;
    public Slider slider_ComboTimer;

    [Header("Effects")]
    public GameObject comboEffect;

    [Header("Item Buttons (Shortcuts)")]
    public Button hintButton;   // 단축키 q
    public Button shakeButton;  // 단축키 w
    public Button biteButton;   // 단축키 e
    public Button settingButton;// 단축키 esc

    [Header("Post Processing")]
    public PostProcessVolume postVolume;
    private Vignette vignette;

    [Header("Cursor")]
    public Texture2D removeCursor;

    [Header("Sound Clip")]
    [SerializeField] private AudioClip mountainKingBGM;


    private Tween vignetteTween;
    // Start is called before the first frame update
    void Start()
    {
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        bestCombo = PlayerPrefs.GetInt("BestCombo", 0);

        if (postVolume != null)
            postVolume.profile.TryGetSettings(out vignette);

        SetState(GameState.Loading);

        slider_ComboTimer.gameObject.SetActive(false);

        score = 0;
        time = 120;                
    }


    // Update is called once per frame
    void Update()
    {
        if (state == GameState.Loading || state == GameState.Shuffle || state == GameState.GameOver)
            return;              

        UpdateTime();
        CheckTimeOver();
        CheckCombo();
        ShortCutKeyDown();
    }

    public int GetScore()
    {
        return score;
    }

    public int GetMaxCombo()
    {
        return maxCombo;
    }

    public void GameStart()
    {
        SetState(GameState.Normal);
        
        if (mountainKingBGM != null && SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayBGM(mountainKingBGM);
        }

        BtnInterOn();
    }

    public void ShortCutKeyDown()
    {
        if (Input.GetKeyDown(KeyCode.Q) && hintButton != null)
            hintButton.onClick.Invoke();

        if (Input.GetKeyDown(KeyCode.W) && shakeButton != null)
            shakeButton.onClick.Invoke();

        if (Input.GetKeyDown(KeyCode.E) && biteButton != null)
            biteButton.onClick.Invoke();

        if (Input.GetKeyDown(KeyCode.Escape) && settingButton != null)
            settingButton.onClick.Invoke();
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
        int disScore = score - s;

        text_PreScore.DOKill();
        text_PreScore.transform.DOKill(); 
        
        text_PreScore.transform.localScale = Vector3.one;

        DOTween.To(() => disScore, x => disScore = x, score, 0.5f)
            .OnUpdate(() => {
                text_PreScore.text = disScore.ToString("N0"); // 천 단위 쉼표
            })
            .SetTarget(text_PreScore);

        text_PreScore.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f, 2, 0.5f);
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
    
    public void GameOver()
    {
        SetState(GameState.GameOver);

        if(score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);
        }

        if(maxCombo > bestCombo)
        {
            bestCombo = maxCombo;
            PlayerPrefs.SetInt("BestCombo", bestCombo);
        }

        PlayerPrefs.Save();

        GameUIManager.Instance.ShowResultBoard();                   
    }
    public void FadeVignette(float targetIntensity, float duration)
    {
        if (vignette == null) return;

        if (vignetteTween != null && vignetteTween.IsActive())
        {
            vignetteTween.Kill();
        }

        vignetteTween = DOTween.To(() => vignette.intensity.value, x => vignette.intensity.value = x, targetIntensity, duration)
               .SetEase(Ease.OutCubic); 
    }
    public void UseItem(ItemData data)
    {
        data.amount--;
        switch (data.itemIndex)
        {
            case 0:
                List<Apple> apple_list = GridManager.Instance.FindOneValidPackage();

                //연출
                foreach (Apple apple in apple_list)
                {
                    apple.transform.DOKill();

                    apple.transform.localScale = Vector3.one;

                    apple.transform.DOPunchScale(new Vector3(0.3f, 0.3f, 0.3f), 0.5f, 5, 0.5f)
                    .SetLoops(-1, LoopType.Restart);
                }

                break;
            case 1:
                GridManager.Instance.PlayRerollAnimation();
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
        GameUIManager.Instance.NoticeUpdate("SELECT MODE ACTIVE.\r\nTAP AN APPLE TO REMOVE IT.");
        FadeVignette(0.45f, 0.5f); 
        Vector2 hotSpot = new Vector2(removeCursor.width / 2, removeCursor.height / 2);
        Cursor.SetCursor(removeCursor, hotSpot, CursorMode.Auto);
    }
    public void EndRemoveItem()
    {
        SetState(GameState.Normal);
        FadeVignette(0f, 0.3f); 
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
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

    public void BtnInterOn()
    {
        hintButton.interactable = true;
        shakeButton.interactable = true;
        biteButton.interactable = true;
        settingButton.interactable = true;
    }        
}
