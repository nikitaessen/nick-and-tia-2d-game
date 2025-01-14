using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] footstepAudioClips;
    [SerializeField] private AudioClip takeDamageAudio;
    [SerializeField] private AudioClip matchLightingAudio;

    private SoundPlayer _soundPlayer;

    public void Initialize(SoundPlayer soundPlayer)
    {
        _soundPlayer = soundPlayer;
    }

    public void PlayFootstepSound()
    {
        _soundPlayer.PlayRandomSoundFromArray(
            footstepAudioClips,
            transform,
            Random.Range(0.05f, 0.1f),
            Random.Range(0.6f, 1.3f));
    }

    public void PlayDamageTakenSound()
    {
        _soundPlayer.PlaySound(takeDamageAudio, transform, 1f);
    }

    public void PlayMatchLightingSound()
    {
        _soundPlayer.PlaySound(matchLightingAudio, transform, 1f);
    }
}