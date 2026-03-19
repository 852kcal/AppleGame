using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    public TextMeshProUGUI text_CountDown;
    public float scaleUpAmount = 1.5f;
    public float duration = 0.8f;

    // Start is called before the first frame update
    void Start()
    {
        startCountDown();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startCountDown()
    {
        text_CountDown.gameObject.SetActive(true);

        Sequence seq = DOTween.Sequence();

        for (int i = 3; i > 0; i--)
        {
            int number = i; 
            seq.AppendCallback(() => {
                text_CountDown.text = number.ToString();
                PlayTextAnim();
            });
            seq.AppendInterval(1f); 
        }

        // 마지막 START!! 연출
        seq.AppendCallback(() => {
            text_CountDown.text = "START!!";
            //text_CountDown.color = Color.yellow; 
            PlayTextAnim();
        });

        seq.AppendInterval(1f);

        // 연출 종료 후 처리
        seq.OnComplete(() => {
            text_CountDown.gameObject.SetActive(false);
            GameManager.Instance.GameStart();
        });
    }
    void PlayTextAnim()
    {
        text_CountDown.transform.DOKill();
        text_CountDown.transform.localScale = Vector3.one * 0.5f; 
        text_CountDown.alpha = 0; 

        text_CountDown.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        text_CountDown.DOFade(1f, 0.3f);

        text_CountDown.DOFade(0f, 0.2f).SetDelay(0.6f);
    }
}
