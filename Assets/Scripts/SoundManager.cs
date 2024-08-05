using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Add AudioSource component to this GameObject in the Unity Editor
    public AudioSource audioSource;

    // Method to toggle sound on/off
    public void ToggleSound(bool isSoundOn)
    {
        audioSource.mute = !isSoundOn;
    }

    // Method to adjust volume
    public void AdjustVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
