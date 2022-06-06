using UnityEngine;

public class SoundVolume : MonoBehaviour
{
    public static float volumeMusique = 0.1f;
    public static float volumeBruitage = 0.2f;

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
