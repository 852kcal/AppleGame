using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIManager : MonoBehaviour
{
    public Button start_Btn;
    public Button shop_Btn;
    public Button exit_Btn;

    // Start is called before the first frame update
    void Start()
    {
        start_Btn.onClick.AddListener(() => MainSceneManager.Instance.OnClickStartBtn());
        shop_Btn.onClick.AddListener(() => MainSceneManager.Instance.OnClickShopBtn());
        exit_Btn.onClick.AddListener(() => MainSceneManager.Instance.OnClickExitBtn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
