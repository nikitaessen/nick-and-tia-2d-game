using System;
using System.Collections.Generic;
using Events;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GuiLogic : MonoBehaviour
{
    [SerializeField] private Sprite brainSprite;
    [SerializeField] private Sprite damagedBrainSprite;
    [SerializeField] private Sprite destroyedBrainSprite;

    private GameStateController _gameStateController;
    private UIDocument _uiDocument;
    private PillStorage _pillStorage;

    private VisualElement InsanityPanel => _uiDocument.rootVisualElement.Q("InsanityPanel");
    private VisualElement BrainElement => _uiDocument.rootVisualElement.Q("Brain");
    private VisualElement GameOverOverlay => _uiDocument.rootVisualElement.Q("GameOver");
    private Button RestartButton => GameOverOverlay.Q<Button>("RestartButton");
    private Button QuitButton => GameOverOverlay.Q<Button>("QuitButton");
    private List<Label> Pills => _uiDocument.rootVisualElement.Q("PillsPanel").Query<Label>().ToList();

    public void Initialize(UIDocument uiDocument, GameStateController gameStateController, PillStorage pillStorage)
    {
        _uiDocument = uiDocument;
        _gameStateController = gameStateController;
        _pillStorage = pillStorage;

        foreach (var pill in Pills)
        {
            pill.style.display = DisplayStyle.None;
        }

        DrawBrain(brainSprite);
        SubscribeToEvents();
    }

    private void DrawBrain(Sprite sprite)
    {
        BrainElement.style.backgroundImage = new StyleBackground(sprite);
    }

    private void ShowGameOver()
    {
        Time.timeScale = 0;
        _uiDocument.rootVisualElement.Q("GameOver").style.display = DisplayStyle.Flex;
    }

    private void OnDamageTaken(object sender, DamageTakenEventArgs args)
    {
        DrawBrain(SelectBrainSprite(args.Insanity));
    }

    private Sprite SelectBrainSprite(int insanityAmount)
    {
        return insanityAmount switch
        {
            0 => brainSprite,
            1 => damagedBrainSprite,
            _ => destroyedBrainSprite
        };
    }

    private void OnRestartClick()
    {
        UnsubscribeFromEvents();
        SceneManager.LoadScene("Level");
        Time.timeScale = 1;
    }

    private void OnQuitClick()
    {
        UnsubscribeFromEvents();
        SceneManager.LoadScene("Menu");
    }

    private void OnGameOver(object sender, GameOverEventArgs args)
    {
        ShowGameOver();
    }

    private void SubscribeToEvents()
    {
        _gameStateController.OnDamageTaken += OnDamageTaken;
        _gameStateController.OnGameOver += OnGameOver;
        _pillStorage.OnPillCollect += OnPillCollect;
        RestartButton.clicked += OnRestartClick;
        QuitButton.clicked += OnQuitClick;
    }

    private void OnPillCollect(object sender, EventArgs args)
    {
        foreach (var pill in Pills)
        {
            if (pill.style.display == DisplayStyle.None)
            {
                pill.style.display = DisplayStyle.Flex;
                break;
            }
        }
    }

    private void UnsubscribeFromEvents()
    {
        _gameStateController.OnDamageTaken += OnDamageTaken;
        _gameStateController.OnGameOver += OnGameOver;
        _pillStorage.OnPillCollect += OnPillCollect;
        RestartButton.clicked -= OnRestartClick;
        QuitButton.clicked -= OnQuitClick;
    }
}