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
    public Button exit_Btn;

    // Start is called before the first frame update
    void Start()
    {
        UpdateMainMenuUI();

        start_Btn.onClick.AddListener(() => MainSceneManager.Instance.OnClickStartBtn());
        shop_Btn.onClick.AddListener(() => MainSceneManager.Instance.OnClickShopBtn());
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
}
