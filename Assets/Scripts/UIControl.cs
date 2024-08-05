using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    public SoundManager soundManager;

    // Reference to toggle switch for sound on/off
    public Toggle soundToggle;

    // Reference to slider for volume adjustment
    public Slider volumeSlider;

    void Start()
    {
        // Set the initial state of toggle and slider based on the sound manager
        soundToggle.isOn = !soundManager.audioSource.mute;
        volumeSlider.value = soundManager.audioSource.volume;
    }

    // Called when the toggle switch state changes
    public void OnToggleSound(bool isSoundOn)
    {
        soundManager.ToggleSound(isSoundOn);
    }

    // Called when the slider value changes
    public void OnVolumeChanged(float volume)
    {
        soundManager.AdjustVolume(volume);
    }
}
