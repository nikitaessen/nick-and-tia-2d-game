using UnityEngine;
using UnityEngine.Audio;

public class MixerSound : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;

    public void SetSoundsVolume(float level)
    {
        mixer.SetFloat("SoundsVolume", Mathf.Log10(level) * 20);
    }

    public void SetMusicVolume(float level)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(level) * 20);
    }
}