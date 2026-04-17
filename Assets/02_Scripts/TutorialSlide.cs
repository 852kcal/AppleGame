using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSlide : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
}
