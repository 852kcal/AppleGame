using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource; 
    [SerializeField] private AudioSource ambientSource;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer masterMixer;

    private const string BGM_VOLUME_KEY = "BGMVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";

    private void Awake()
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
    private void Start()
    {
        float savedBGMVolume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, 0.75f);
        float savedSFXVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 0.75f);

        /*
        if (bgmSlider != null) bgmSlider.value = savedBGMVolume;
        if (sfxSlider != null) sfxSlider.value = savedSFXVolume;
        */

        SetBgmVolume(savedBGMVolume);
        SetSfxVolume(savedSFXVolume);

        /*
        if (bgmSlider != null) bgmSlider.onValueChanged.AddListener(SetBgmVolume);
        if (sfxSlider != null) sfxSlider.onValueChanged.AddListener(SetSfxVolume);
        */
    }

    public void PlayBGM(AudioClip clip)
    {
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayAmbient(AudioClip clip)
    {
        ambientSource.clip = clip;
        ambientSource.loop = true;
        ambientSource.Play();
    }
    public void StopAmbient()
    {
        ambientSource.Stop();
    }

    public void SetBgmVolume(float volume)
    {
        float dbVolume = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20f;
        masterMixer.SetFloat("BGMVol", dbVolume);
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, volume);
    }

    public void SetSfxVolume(float volume)
    {
        float dbVolume = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20f;
        masterMixer.SetFloat("SFXVol", dbVolume);
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);
    }

    public void StopBgm()
    {
        bgmSource.Stop();
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}
