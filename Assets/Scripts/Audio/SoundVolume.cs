using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundVolume : MonoBehaviour
{
    public static float volumeMusique = 1;
    public static float volumeBruitage = 1;

    public void Musique(float newVolume)
    {
        AudioSource soundplayer = FindObjectOfType<AudioSource>();

        soundplayer.volume = newVolume;

        volumeMusique = newVolume;
    }

    public void Bruitage(float newValue)
    {
        volumeBruitage = newValue;
    }
}
