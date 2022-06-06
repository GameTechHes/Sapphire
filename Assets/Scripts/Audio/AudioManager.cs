using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    void Awake()
    {
        AudioSource sp = FindObjectOfType<AudioSource>();
        if (sp != null)
        {
            sp.Stop();
        }

        float MusiqueVolume = SoundVolume.volumeMusique;
        float BruitageVolume = SoundVolume.volumeBruitage;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            if (s.name == "Music")
            {
                s.source.volume = MusiqueVolume;
                s.source.loop = true;
                // s.source.Play();
            }
            else
            {
                s.source.volume = BruitageVolume;
            }

            s.source.pitch = s.pitch;
        }
    }


    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }
}