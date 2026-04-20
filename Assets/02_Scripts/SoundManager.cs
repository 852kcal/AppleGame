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

    public const string MASTER_VOLUME_KEY = "MASTERVolume";
    public const string BGM_VOLUME_KEY = "BGMVolume";
    public const string SFX_VOLUME_KEY = "SFXVolume";

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
        ApplyAllVolumes();
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

    public void StopBgm()
    {
        bgmSource.Stop();
    }

    public void SetVolume(string parameterName, float volume)
    {
        float dbVolume = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20f;
        masterMixer.SetFloat(parameterName, dbVolume);
    }

    public void ApplyAllVolumes()
    {
        SetVolume("MASTERVol", PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, 0.8f));
        SetVolume("BGMVol", PlayerPrefs.GetFloat(BGM_VOLUME_KEY, 1f));
        SetVolume("SFXVol", PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f));
    }

    public void SaveVolume(float master, float bgm, float sfx)
    {
        PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, master);
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, bgm);
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, sfx);
        PlayerPrefs.Save();
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}