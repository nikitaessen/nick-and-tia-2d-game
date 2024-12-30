using UnityEngine;
using UnityEngine.Audio;

public class MixerVolumeController : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;

    public void SetSoundsVolume(float level)
    {
        mixer.SetFloat("SoundsVolume", Mathf.Log10(level)*20);
        Debug.Log($"sound volume: {level}");
    }

    public void SetMusicVolume(float level)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(level)*20);
        Debug.Log($"music volume: {level}");

    }
}