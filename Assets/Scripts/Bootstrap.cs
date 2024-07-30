using UnityEngine;
using UnityEngine.UIElements;


public class Bootstrap : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameStateController gameStateController;
    [SerializeField] private PillStorage pillStorage;
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private GuiLogic guiLogic;

    private void Awake()
    {
        playerMovement.Initialize();
        pillStorage.Initialize(playerMovement);
        gameStateController.Initialize(playerMovement);
        guiLogic.Initialize(uiDocument, gameStateController, pillStorage);
    }
}