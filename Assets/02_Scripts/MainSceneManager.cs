using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClickStartBtn()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void onClickShopBtn()
    {
        SceneManager.LoadScene("ShopScene");
    }

    public void onClickExitBtn()
    {
        #if UNITY_EDITOR 
        UnityEditor.EditorApplication.isPlaying = false; 
        #else 
        Application.Quit();
        #endif

        Application.Quit();
    }
}
