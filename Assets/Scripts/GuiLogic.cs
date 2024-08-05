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
    [SerializeField] private VisualTreeAsset gameOverTemplate;

    private GameStateController _gameStateController;
    private UIDocument _uiDocument;
    private PillStorage _pillStorage;

    private VisualElement BrainElement => _uiDocument.rootVisualElement.Q("Brain");
    private VisualElement GameOverContainer => _uiDocument.rootVisualElement.Q("GameOverContainer");
    private Button RestartButton => GameOverContainer.Q<Button>("RestartButton");
    private Button QuitButton => GameOverContainer.Q<Button>("QuitButton");
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

        AddGameOver();
        DrawBrain(brainSprite);
        SubscribeToEvents();
    }

    private void AddGameOver()
    {
        var gameOverClone = gameOverTemplate.CloneTree();
        gameOverClone.style.width = new StyleLength(Length.Percent(100));
        gameOverClone.style.height = new StyleLength(Length.Percent(100));
        gameOverClone.style.display = DisplayStyle.None;
        
        GameOverContainer.Add(gameOverClone);
    }

    private void DrawBrain(Sprite sprite)
    {
        BrainElement.style.backgroundImage = new StyleBackground(sprite);
    }

    private void ShowGameOver()
    {
        Time.timeScale = 0;
        GameOverContainer.style.display = DisplayStyle.None;
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