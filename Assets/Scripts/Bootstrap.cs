using UnityEngine;
using UnityEngine.UIElements;


public class Bootstrap : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameStateController gameStateController;
    [SerializeField] private UIDocument uiDocument;

    private void Awake()
    {
        playerMovement.Initialize();
        gameStateController.Initialize(uiDocument, playerMovement);
    }
}