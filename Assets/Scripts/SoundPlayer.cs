using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource soundSource;
    public void Initialize()
    {
    }

    public void PlaySound(AudioClip audioClip, Transform spawnPosition, float volume)
    {
        var audioSource = Instantiate(soundSource, spawnPosition.position, Quaternion.identity);

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();

        var length = audioSource.clip.length;
        Destroy(audioSource.gameObject, length);
    }

    public void PlayRandomSoundFromArray(AudioClip[] clipsArray, Transform spawnPosition, float volume, float pitch)
    {
        var audioSource = Instantiate(soundSource, spawnPosition.position, Quaternion.identity);
        var randomIndex = Random.Range(0, clipsArray.Length);

        audioSource.clip = clipsArray[randomIndex];
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.Play();

        var length = audioSource.clip.length;
        Destroy(audioSource.gameObject, length);
    }
}