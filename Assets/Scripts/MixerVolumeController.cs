using UnityEngine;
using UnityEngine.Audio;

public class MixerVolumeController : MonoBehaviour
{
    public static MixerVolumeController instance;
    
    [SerializeField] private AudioMixer mixer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void SetSoundsVolume(float level)
    {
        mixer.SetFloat("SoundsVolume", Mathf.Log10(level) * 20);
    }

    public void SetMusicVolume(float level)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(level) * 20);
    }
}