using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainUIManager : MonoBehaviour
{
    public TextMeshProUGUI text_BestScore;
    public TextMeshProUGUI text_BestCombo;

    public Button start_Btn;
    public Button shop_Btn;
    public Button setting_Btn;
    public Button exit_Btn;

    public CanvasGroup backGround;

    public GameObject settingBoard;

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    // Start is called before the first frame update
    void Start()
    {
        UpdateMainMenuUI();

        start_Btn.onClick.AddListener(() => MainSceneManager.Instance.OnClickStartBtn());
        shop_Btn.onClick.AddListener(() => MainSceneManager.Instance.OnClickShopBtn());
        setting_Btn.onClick.AddListener(() => OnClickSettingBtn());
        exit_Btn.onClick.AddListener(() => MainSceneManager.Instance.OnClickExitBtn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateMainMenuUI()
    {
        int savedBest = PlayerPrefs.GetInt("BestScore", 0);
        int savedCombo = PlayerPrefs.GetInt("BestCombo", 0);

        text_BestScore.text = "BEST SCORE\n" + savedBest.ToString("N0");
        text_BestCombo.text = "BEST COMBO\n" + savedCombo.ToString("N0");
    }

    public void OnClickSettingBtn()
    {
        DOTween.Kill(backGround);

        backGround.alpha = 0f;
        backGround.gameObject.SetActive(true);

        settingBoard.SetActive(true);

        backGround.DOFade(0.8f, 0.5f).SetUpdate(true);
    }

    public void OnClickSettingCloseBtn()
    {
        settingBoard.SetActive(false);

        DOTween.Kill(backGround);

        backGround.DOFade(0f, 0.5f)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                backGround.gameObject.SetActive(false);
            });
    }

    public void OnClickSettingSaveBtn()
    {


        OnClickSettingCloseBtn();
    }
}
