using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public List<TutorialData> tutorialDataList;

    public CanvasGroup contentCanvasGroup;
    public Image displayImage;
    public TextMeshPro displayText;

    public Button nextBtn;
    public Button prevBtn;
    public Button skipBtn;

    private int currentIndex = 0;

    void Start()
    {
        ShowSlide(currentIndex);

        nextBtn.onClick.AddListener(OnClickNextBtn);
        prevBtn.onClick.AddListener(OnClickPrevBtn);
    }

    public void OnClickNextBtn()
    {
        if (currentIndex < tutorialDataList.Count - 1)
        {
            currentIndex++;
            UpdateUI();
        }
        else
        {
            Debug.Log("Tutorial ³¡");
        }
    }

    public void OnClickPrevBtn()
    {
        if(currentIndex > 0)
        {
            currentIndex--;
            UpdateUI();
        }
    }
    public void UpdateUI()
    {
        contentCanvasGroup.DOFade(0f, 0.2f).OnComplete(() =>
        {
            ShowSlide(currentIndex);

            contentCanvasGroup.DOFade(1f, 0.2f);
        });
    }

    public void ShowSlide(int index)
    {        
        TutorialData data = tutorialDataList[index];
                
        displayImage.sprite = data.tutorialImage;
        displayText.text = data.tutorialText;
                
        prevBtn.interactable = (index > 0);
    }
}
