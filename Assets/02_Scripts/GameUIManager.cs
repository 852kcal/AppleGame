using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class GameUIManager : MonoBehaviour
{
    private static GameUIManager instance;

    public CanvasGroup backGround;

    public RectTransform noticeBoard;
    public RectTransform resultBoard;

    public Button restart_Btn;
    public Button main_Btn;
    public Button result_Close_Btn;

    public TextMeshProUGUI text_Notice;
    public TextMeshProUGUI text_ResultTitle;
    public TextMeshProUGUI text_Result;
        
    private float notice_targetY = 0f;
    private float notice_startY = 1000f;

    private float dropDuration = 1f;
    private float swingDuration = 1.5f;   
    private float swingAngle = 0.5f;
    private float resut_startY = 1500f;

    private Sequence swingSequence;

    private bool isAnimating = false;

    public static GameUIManager Instance { get { return instance; } }
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        restart_Btn.onClick.AddListener(() => MainSceneManager.Instance.OnClickStartBtn());
        main_Btn.onClick.AddListener(() => MainSceneManager.Instance.OnClickMainBtn());
    }

    public void NoticeUpdate(string text)
    {
        text_Notice.text = text;
        PlayPulleyAnimation();
    }

    private void PlayPulleyAnimation()
    {
        noticeBoard.DOKill();

        noticeBoard.anchoredPosition = new Vector2(0f, notice_startY);
        noticeBoard.localEulerAngles = new Vector3(0f, 0f, -4f);

        Sequence pulleySeq = DOTween.Sequence();

        pulleySeq.Append(noticeBoard.DOAnchorPosY(notice_targetY, 0.6f).SetEase(Ease.OutBack, 0.7f));
        pulleySeq.Join(noticeBoard.DORotate(Vector3.zero, 0.9f).SetEase(Ease.OutElastic, 0.3f, 0.6f));

        pulleySeq.AppendInterval(1.5f);

        pulleySeq.Append(noticeBoard.DORotate(new Vector3(0f, 0f, 3f), 0.2f).SetEase(Ease.OutQuad));
        pulleySeq.Join(noticeBoard.DOAnchorPosY(notice_startY, 0.5f).SetEase(Ease.InBack, 0.6f));
    }

    public void ShowResultBoard()
    {
        if(GameManager.Instance.state == GameState.GameOver)
        {
            result_Close_Btn.gameObject.SetActive(false);

            text_ResultTitle.text = "RESULT";
        }
        else
        {
            text_ResultTitle.text = "MENU";
            if (isAnimating)
                return;
        }

        text_Result.text = "SCORE : " + GameManager.Instance.GetScore().ToString("N0");
        text_Result.text += "\nMAX COMBO : " + GameManager.Instance.GetMaxCombo().ToString("N0");

        DOTween.Kill(resultBoard);
        DOTween.Kill(backGround);

        swingSequence?.Kill();

        if (GameManager.Instance.state != GameState.GameOver && backGround.gameObject.activeSelf)
        {
            HideMenu();
            return;
        }

        isAnimating = true;

        resultBoard.anchoredPosition = new Vector2(resultBoard.anchoredPosition.x, resut_startY);
        resultBoard.rotation = Quaternion.identity;
        backGround.alpha = 0f;
        backGround.gameObject.SetActive(true);

        backGround.DOFade(0.8f, dropDuration * 0.5f).SetUpdate(true);

        resultBoard.DOAnchorPosY(   0, dropDuration)
            .SetEase(Ease.OutBounce)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                StartSwinging();
                isAnimating = false;
            });
    }

    private void StartSwinging()
    {
        swingSequence = DOTween.Sequence()
            .Append(resultBoard.DORotate(new Vector3(0, 0, swingAngle), swingDuration).SetEase(Ease.InOutSine))
            .Append(resultBoard.DORotate(new Vector3(0, 0, -swingAngle), swingDuration).SetEase(Ease.InOutSine))
            .SetLoops(-1, LoopType.Yoyo)
            .SetUpdate(true); 
    }

    public void HideMenu()
    {
        isAnimating = true;
        swingSequence?.Kill();

        resultBoard.DOAnchorPosY(resut_startY, dropDuration * 0.5f)
            .SetEase(Ease.InBack, 0.8f)
            .SetUpdate(true);

        backGround.DOFade(0f, dropDuration * 0.5f)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                backGround.gameObject.SetActive(false);
                isAnimating = false;
            });
    }

    private void OnDestroy()
    {
        if (resultBoard != null)
        {
            DOTween.Kill(resultBoard);
        }

        if (swingSequence != null)
        {
            swingSequence.Kill();
        }

        if (backGround != null)
        {
            DOTween.Kill(backGround);
        }
    }
}
