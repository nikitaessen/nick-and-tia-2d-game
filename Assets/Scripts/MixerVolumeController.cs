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
        mixer.SetFloat("SoundsVolume", level);
        Debug.Log($"sound: {level}");
    }

    public void SetMusicVolume(float level)
    {
        mixer.SetFloat("MusicVolume", level);
    }
}