using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GameUIManager : MonoBehaviour
{
    private static GameUIManager instance;

    public RectTransform noticeBoard;
    public TextMeshProUGUI text_Notice;

    private float targetY = 0f;
    private float startY = 1000f;

    public static GameUIManager Instance { get { return instance; } }
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        
    }

    public void NoticeUpdate(string text)
    {
        text_Notice.text = text;
        PlayPulleyAnimation();
    }

    private void PlayPulleyAnimation()
    {
        noticeBoard.DOKill();

        noticeBoard.anchoredPosition = new Vector2(0f, startY);
        noticeBoard.localEulerAngles = new Vector3(0f, 0f, -4f);

        Sequence pulleySeq = DOTween.Sequence();

        pulleySeq.Append(noticeBoard.DOAnchorPosY(targetY, 0.6f).SetEase(Ease.OutBack, 0.7f));
        pulleySeq.Join(noticeBoard.DORotate(Vector3.zero, 0.9f).SetEase(Ease.OutElastic, 0.3f, 0.6f));

        pulleySeq.AppendInterval(1.5f);

        pulleySeq.Append(noticeBoard.DORotate(new Vector3(0f, 0f, 3f), 0.2f).SetEase(Ease.OutQuad));
        pulleySeq.Join(noticeBoard.DOAnchorPosY(startY, 0.5f).SetEase(Ease.InBack, 0.6f));
    }
}
