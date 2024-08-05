using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; // Singleton instance

    [Header("Sound Settings")]
    public bool isMusicMuted;
    public bool isSoundEffectsMuted;
    public bool loopBackgroundMusic = true;
    [Range(0f, 1f)]
    public float musicVolume = 1f;
    [Range(0f, 1f)]
    public float soundEffectsVolume = 1f;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource soundEffectsSource;

    [Header("Audio Clips")]
    public AudioClip menuClickSound;
    public AudioClip menuOpenCloseSound;
    public AudioClip ak74GunSound;
    public AudioClip enemyShotSound;
    public AudioClip enemySlashSound;
    public AudioClip shotGunSound;
    public AudioClip clipEmptySound;
    public AudioClip RocketLauncherSound;
    public AudioClip pickupSound;
    public AudioClip ammoPickupSound;
    public AudioClip explosionSound;
    public AudioClip BulletHitSound;
    public AudioClip deathRoarSound;
    public AudioClip doorOpenSound;
    public AudioClip beamDownSound;
    public AudioClip backgroundMusic;

    // Add more audio clips as needed

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep the AudioManager alive across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Load volume settings from PlayerPrefs
        
        //PlayMusic();

        // Set up audio sources
        //musicSource = gameObject.AddComponent<AudioSource>();
        //soundEffectsSource = gameObject.AddComponent<AudioSource>();
        
        // Create the AudioSource components if they don't exist
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }

        if (soundEffectsSource == null)
        {
            soundEffectsSource = gameObject.AddComponent<AudioSource>();
        }

        LoadAudioSettings();
        PlayBackgroundMusic();

    }

    public void GetAudioSettings(out bool isMusicMuted, out bool isSoundEffectsMuted, out float musicVolume, out float soundEffectsVolume)
    {
        isMusicMuted = this.isMusicMuted;
        isSoundEffectsMuted = this.isSoundEffectsMuted;
        musicVolume = this.musicVolume;
        soundEffectsVolume = this.soundEffectsVolume;
    }

    public void LoadAudioSettings()
    {
        

        // Load volume settings from PlayerPrefs (if available)
        isMusicMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
        isSoundEffectsMuted = PlayerPrefs.GetInt("SoundEffectsMuted", 0) == 1;
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        soundEffectsVolume = PlayerPrefs.GetFloat("SoundEffectsVolume", 1f);

        UpdateAudioSourceSettings();

        //// Apply volume settings to audio sources
        //musicSource.mute = isMusicMuted;
        //soundEffectsSource.mute = isSoundEffectsMuted;
        //musicSource.volume = musicVolume;
        //soundEffectsSource.volume = soundEffectsVolume;
    }

    public void SaveAudioSettings()
    {
        PlayerPrefs.SetInt("MusicMuted", isMusicMuted ? 1 : 0);
        PlayerPrefs.SetInt("SoundEffectsMuted", isSoundEffectsMuted ? 1 : 0);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SoundEffectsVolume", soundEffectsVolume);
    }

    private void UpdateAudioSourceSettings()
    {
        if (musicSource != null)
        {
            musicSource.mute = isMusicMuted;
            musicSource.volume = musicVolume;
        }

        if (soundEffectsSource != null)
        {
            soundEffectsSource.mute = isSoundEffectsMuted;
            soundEffectsSource.volume = soundEffectsVolume;
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        musicSource.volume = volume;
        SaveAudioSettings();
    }

    public void SetSoundEffectsVolume(float volume)
    {
        soundEffectsVolume = volume;
        soundEffectsSource.volume = volume;
        SaveAudioSettings();
    }

    public void PlaySound(AudioClip clip)
    {
        soundEffectsSource.PlayOneShot(clip, soundEffectsVolume);
    }

    public void PlayBackgroundMusic()
    {
        musicSource.clip = backgroundMusic;
        musicSource.loop = loopBackgroundMusic;
        musicSource.Play();
    }

    public void ToggleMusic()
    {
        isMusicMuted = !isMusicMuted;
        musicSource.mute = isMusicMuted;
        SaveAudioSettings();
    }

    public void ToggleSoundEffects()
    {
        isSoundEffectsMuted = !isSoundEffectsMuted;
        soundEffectsSource.mute = isSoundEffectsMuted;
        SaveAudioSettings();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

}
