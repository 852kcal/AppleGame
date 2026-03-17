using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemButton : MonoBehaviour
{
    public ItemData data;
    public TextMeshProUGUI text_Count;

    // Start is called before the first frame update
    void Start()
    {
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
    }

    public void OnClick()
    {
        if(data.amount > 0)
        {
            transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.2f, 10, 1f);
            GameManager.Instance.UseItem(data);
            UpdateUI();
        }
        else
        {
            Debug.Log("수량부족");
        }
    }
}
