using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{
    static MainSceneManager instance;
    public static MainSceneManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MainSceneManager>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickMainBtn()
    {
        LoadMainScene();
    }
    public void OnClickStartBtn()
    {
        LoadPlayScene();
    }

    public void OnClickShopBtn()
    {
        LoadShopScene();
    }

    public void OnClickExitBtn()
    {
        #if UNITY_EDITOR 
        UnityEditor.EditorApplication.isPlaying = false; 
        #else 
        Application.Quit();
        #endif

        Application.Quit();
    }

    void LoadMainScene()
    {
        SceneManager.LoadScene("00_Main");
    }
    
    void LoadPlayScene()
    {
        SceneManager.LoadScene("01_Play");
    }

    void LoadShopScene()
    {
        SceneManager.LoadScene("03_Shop");
    }
}
