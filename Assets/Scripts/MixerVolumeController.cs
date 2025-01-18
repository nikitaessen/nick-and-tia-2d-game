using Events;
using UnityEngine;
using UnityEngine.Audio;

public class MixerVolumeController : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;

    private GameStateController _gameStateController;

    public void Initialize(GameStateController gameStateController)
    {
        _gameStateController = gameStateController;
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        _gameStateController.OnGameOver += OnGameOver;
    }

    private void OnGameOver(object sender, GameOverEventArgs args)
    {
        mixer.SetFloat("MusicVolume", -90);
    }

    public void SetSoundsVolume(float level)
    {
        mixer.SetFloat("SoundsVolume", Mathf.Log10(level) * 20);
        Debug.Log($"sound volume: {level}");
    }

    public void SetMusicVolume(float level)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(level) * 20);
        Debug.Log($"music volume: {level}");
    }
}