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
    [SerializeField] private SoundPlayer soundPlayer;
    [SerializeField] private PlayerInput playerInput;

    private void Awake()
    {
        soundPlayer.Initialize();
        playerMovement.Initialize(soundPlayer);
        pillStorage.Initialize(playerMovement);
        gameStateController.Initialize(playerMovement, playerInput);
        guiLogic.Initialize(uiDocument, gameStateController, pillStorage);
    }
}