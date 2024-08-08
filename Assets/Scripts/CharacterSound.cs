using UnityEngine;

public class CharacterSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] footstepAudioClips;
    [SerializeField] private AudioClip takeDamageAudio;

    private SoundPlayer _soundPlayer;
    
    public void Initialize(SoundPlayer soundPlayer)
    {
        _soundPlayer = soundPlayer;
    }
    
    public void PlayFootstepSound()
    {
        _soundPlayer.PlayRandomSoundFromArray(footstepAudioClips, transform, 1f);
    }
}
