using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SFXController : MonoBehaviour
{
    public static SFXController Instance;

    [Header("SFX Controller Settings")]
    [SerializeField] private AudioSource soundFXObject;
    [SerializeField] private AudioSource BGM_Source;
    
    [Header("SFX Setup")]
    [SerializeField] private AudioClip GameBGM;
    
    private void OnDisable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Awake()
    {
        if (GameBGM == null)
        {
            Debug.LogError("GameBGM is not assigned in SFXController.");
            return;
        }
        
        if(soundFXObject == null)
        {
            Debug.LogError("SoundFXObject is not assigned in SFXController.");
            return;
        }

        if (BGM_Source == null)
        {
            Debug.LogError("BGM AudioSource is not assigned in SFXController.");
            return;
        }

        SetupSingleton();
    }

    void SetupSingleton()
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

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume, float pitchRange = 0.0f)
    {
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        float pitch = audioSource.pitch;

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.pitch = UnityEngine.Random.Range(pitch - pitchRange, pitch + pitchRange);
        audioSource.Play();

        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlayBGM()
    {
        StopBGM();

        BGM_Source.clip = GameBGM;
        BGM_Source.loop = true;
        BGM_Source.Play();
    }

    public void StopBGM()
    {
        if (BGM_Source.isPlaying)
        {
            BGM_Source.Stop();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StopBGM();
    }

}