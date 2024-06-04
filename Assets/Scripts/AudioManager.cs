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
        // Set up audio sources
        musicSource = gameObject.AddComponent<AudioSource>();
        soundEffectsSource = gameObject.AddComponent<AudioSource>();

        // Load volume settings from PlayerPrefs (if available)
        isMusicMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
        isSoundEffectsMuted = PlayerPrefs.GetInt("SoundEffectsMuted", 0) == 1;
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        soundEffectsVolume = PlayerPrefs.GetFloat("SoundEffectsVolume", 1f);

        // Apply volume settings to audio sources
        musicSource.mute = isMusicMuted;
        soundEffectsSource.mute = isSoundEffectsMuted;
        musicSource.volume = musicVolume;
        soundEffectsSource.volume = soundEffectsVolume;
    }

    public void PlaySound(AudioClip clip)
    {
        soundEffectsSource.PlayOneShot(clip, soundEffectsVolume);
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }


    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = loopBackgroundMusic;
        musicSource.Play();
    }

    public void ToggleMusic()
    {
        isMusicMuted = !isMusicMuted;
        musicSource.mute = isMusicMuted;
        PlayerPrefs.SetInt("MusicMuted", isMusicMuted ? 1 : 0);
    }

    public void ToggleSoundEffects()
    {
        isSoundEffectsMuted = !isSoundEffectsMuted;
        soundEffectsSource.mute = isSoundEffectsMuted;
        PlayerPrefs.SetInt("SoundEffectsMuted", isSoundEffectsMuted ? 1 : 0);
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSoundEffectsVolume(float volume)
    {
        soundEffectsVolume = volume;
        soundEffectsSource.volume = volume;
        PlayerPrefs.SetFloat("SoundEffectsVolume", volume);
    }
}
