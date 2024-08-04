using UnityEngine;
using UnityEngine.UIElements;


public class Bootstrap : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameStateController gameStateController;
    [SerializeField] private PillStorage pillStorage;
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private GuiLogic guiLogic;
    [SerializeField] private SoundPlayer soundPlayer;

    private void Awake()
    {
        soundPlayer.Initialize();
        playerMovement.Initialize(soundPlayer);
        pillStorage.Initialize(playerMovement);
        gameStateController.Initialize(playerMovement);
        guiLogic.Initialize(uiDocument, gameStateController, pillStorage);
    }
}