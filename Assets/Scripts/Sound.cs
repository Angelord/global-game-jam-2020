using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public List<AudioClip> clips;

    [Range(0f, 1f)]
    public float volume;
    [Range(.0f, .5f)]
    public float pitch_variation = 0;
    public bool loop;
    public bool RandomSound;

    [HideInInspector]
    public AudioSource source;

    public AudioClip GetRandomClip()
    {
        int randomNum = Random.Range(0, 10000) % clips.Count;
        return clips[randomNum];
    }

    public float GetRandomPitch()
    {
        return Random.Range(-pitch_variation, pitch_variation) + 1;
    }
}
