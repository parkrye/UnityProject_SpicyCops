using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    AudioMixer audioMixer;

    void Awake()
    {
        audioMixer = GameManager.Resource.Load<AudioMixer>("Audio/AudioMixer");
    }

    public float MasterVolume
    {
        get
        {
            float volume;
            audioMixer.GetFloat("Master", out volume);
            return volume;
        }
        set
        {
            audioMixer.SetFloat("Master", value * 40f);
        }
    }

    public float BGMVolume
    {
        get
        {
            float volume;
            audioMixer.GetFloat("BGM", out volume);
            return volume;
        }
        set
        {
            audioMixer.SetFloat("BGM", value * 40f);
        }
    }

    public float SFXVolume
    {
        get
        {
            float volume;
            audioMixer.GetFloat("SFX", out volume);
            return volume;
        }
        set
        {
            audioMixer.SetFloat("SFX", value * 40f);
        }
    }
}