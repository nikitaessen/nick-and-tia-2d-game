using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class Bootstrap : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameStateController gameStateController;
    [SerializeField] private PillStorage pillStorage;
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private GuiLogic guiLogic;
    [SerializeField] private MixerVolumeController mixerVolumeController;
    [SerializeField] private SoundPlayer soundPlayer;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private CharacterSound characterSound;

    private void Awake()
    {
        playerMovement.Initialize();
        soundPlayer.Initialize();
        characterSound.Initialize(soundPlayer);
        pillStorage.Initialize(playerMovement);
        gameStateController.Initialize(playerMovement, playerInput, soundPlayer);
        mixerVolumeController.Initialize(gameStateController);
        guiLogic.Initialize(uiDocument, gameStateController, pillStorage, mixerVolumeController);
    }
}