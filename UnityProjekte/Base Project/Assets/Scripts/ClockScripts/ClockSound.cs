using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockSound : MonoBehaviour
{
    public AudioSource clockAudioSource;  // Reference to the central AudioSource

    public void PlayTickingSound()
    {
        // Play the ticking sound from the central AudioSource
        if (clockAudioSource != null)
        {
            clockAudioSource.Play();
        }
        else
        {
            Debug.LogError("No central AudioSource assigned.");
        }
    }
}
