using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Slider musicVolumeSlider;
    public Slider soundEffectsVolumeSlider;
    public Toggle musicToggle;
    public Toggle soundEffectsToggle;

    public AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.instance;

        // Load the saved audio settings from the AudioManager
        audioManager.GetAudioSettings(out bool isMusicMuted, out bool isSoundEffectsMuted, out float musicVolume, out float soundEffectsVolume);

        // Initialize UI elements with the loaded audio settings
        musicVolumeSlider.value = musicVolume;
        soundEffectsVolumeSlider.value = soundEffectsVolume;
        musicToggle.isOn = !isMusicMuted;
        soundEffectsToggle.isOn = !isSoundEffectsMuted;

        // Add event handlers for the sliders
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        soundEffectsVolumeSlider.onValueChanged.AddListener(SetSoundEffectsVolume);
    }

    public void ToggleMusic()
    {
        audioManager.ToggleMusic();
        musicToggle.isOn = !audioManager.isMusicMuted;
        audioManager.SaveAudioSettings();

        if (audioManager.isMusicMuted)
        {
            audioManager.musicSource.Pause();
        }
        else
        {
            audioManager.musicSource.Play();
        }
    }

    public void ToggleSoundEffects()
    {
        audioManager.ToggleSoundEffects();
        soundEffectsToggle.isOn = !audioManager.isSoundEffectsMuted;
        audioManager.SaveAudioSettings();
    }

    public void SetMusicVolume(float volume)
    {
        audioManager.SetMusicVolume(volume);
        audioManager.SaveAudioSettings();
    }

    public void SetSoundEffectsVolume(float volume)
    {
        audioManager.SetSoundEffectsVolume(volume);
        audioManager.SaveAudioSettings();
    }

}
