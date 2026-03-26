using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemButton : MonoBehaviour
{
    public ItemData data;
    public TextMeshProUGUI text_Count;

    private CanvasGroup cg;

    private Vector3 originalPos;
    private Vector3 originalScale;

    // Start is called before the first frame update
    void Start()
    {
        cg = GetComponent<CanvasGroup>();
        
        originalPos = transform.localPosition;
        originalScale = transform.localScale;

        UpdateUI();        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUI()
    {
        if(data != null && text_Count != null)
            text_Count.text = "[" + data.amount + "]";
        text_Count.transform.DOKill();
        text_Count.transform.localScale = Vector3.one; 

        text_Count.transform.DOPunchScale(new Vector3(0.3f, 0.3f, 0), 0.2f, 10, 1f);

        SetButtonActive();
    }

    public void OnClick()
    {
        if(data.amount > 0)
        {
            OnClickEffect();
            GameManager.Instance.UseItem(data);
            UpdateUI();
        }
        else
        {
            Debug.Log("수량부족");
        }
    }

    public void OnClickEffect()
    {
        transform.DOKill();

        transform.localPosition = originalPos;
        transform.localScale = originalScale;

        transform.DOPunchScale(new Vector3(-0.05f, -0.05f, 0), 0.1f);

        transform.DOPunchPosition(Vector3.down * 10f, 0.2f, 0, 0);
    }

    public void SetButtonActive()
    {
        cg.DOKill();
        if (data.amount > 0)
        {
            cg.DOFade(1f, 0.2f);
            cg.interactable = true;
        }
        else
        {
            cg.DOFade(0.4f, 0.2f);
            cg.interactable = false;
        }

    }
}
