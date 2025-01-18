using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinBootstrap : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private SoundPlayer soundPlayer;
    [SerializeField] private CharacterSound characterSound;

    private void Awake()
    {
        soundPlayer.Initialize();
        playerMovement.Initialize();
        characterSound.Initialize(soundPlayer);
    }
}
