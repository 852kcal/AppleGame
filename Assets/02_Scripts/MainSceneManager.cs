using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{
    public static MainSceneManager Instance { get; private set; }

    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 0.5f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); 
        }
    }
    public void OnClickMainBtn() => StartCoroutine(FadeAndLoadScene("00_Main"));
    public void OnClickStartBtn() => StartCoroutine(FadeAndLoadScene("01_Play"));
    public void OnClickShopBtn() => StartCoroutine(FadeAndLoadScene("03_Shop"));
    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        fadeCanvasGroup.blocksRaycasts = true; 
        yield return fadeCanvasGroup.DOFade(1f, fadeDuration).WaitForCompletion();

        SceneManager.LoadScene(sceneName);

        yield return fadeCanvasGroup.DOFade(0f, fadeDuration).WaitForCompletion();
        fadeCanvasGroup.blocksRaycasts = false;
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
}
