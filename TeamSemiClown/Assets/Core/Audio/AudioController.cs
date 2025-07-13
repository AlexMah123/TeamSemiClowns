using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    [Header("Pause Configs")]
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private bool EnableInGameLevel = true;

    private const string MasterVolumeKey = "MasterVolume";
    private const string MusicVolumeKey = "MusicVolume";
    private const string SFXVolumeKey = "SFXVolume";

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        LoadVolumeSettings();
    }

    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat("Master", LinearToDecibel(level));
        PlayerPrefs.SetFloat(MasterVolumeKey, level);
    }

    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("Music", LinearToDecibel(level));
        PlayerPrefs.SetFloat(MusicVolumeKey, level);
    }

    public void SetSFXVolume(float level)
    {
        audioMixer.SetFloat("SFX", LinearToDecibel(level));
        PlayerPrefs.SetFloat(SFXVolumeKey, level);
    }

    private void LoadVolumeSettings()
    {
        float defaultVolume = 0.5f; // Default volume
        float masterVolume = PlayerPrefs.GetFloat(MasterVolumeKey, defaultVolume);
        float musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, defaultVolume);
        float sfxVolume = PlayerPrefs.GetFloat(SFXVolumeKey, defaultVolume);

        masterVolumeSlider.value = masterVolume;
        musicVolumeSlider.value = musicVolume;
        sfxVolumeSlider.value = sfxVolume;

        SetMasterVolume(masterVolume);
        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);
    }

    private float LinearToDecibel(float linear)
    {
        return linear > 0.0001f ? Mathf.Log10(linear) * 20f : -80f;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (EnableInGameLevel && scene.buildIndex == (int)SceneType.Game)
        {
            pauseButton.SetActive(true);
        }
        else
        {
            pauseButton.SetActive(false);
        }
    }
}