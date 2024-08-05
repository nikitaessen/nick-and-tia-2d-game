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
    [SerializeField] private VisualTreeAsset pauseTemplate;

    private GameStateController _gameStateController;
    private UIDocument _uiDocument;
    private PillStorage _pillStorage;

    private VisualElement BrainElement => _uiDocument.rootVisualElement.Q("Brain");

    private VisualElement GameOverContainer => _uiDocument.rootVisualElement.Q<VisualElement>("GameOverContainer");
    private Button GameOverRestartButton => GameOverContainer.Q<Button>("RestartButton");
    private Button GameOverQuitButton => GameOverContainer.Q<Button>("QuitButton");

    //TODO
    // add pause container and set display to none
    // fix game over and pause mechanics by adding new input scheme
    // add resume button func, register callback
    private VisualElement PauseMenuContainer => _uiDocument.rootVisualElement.Q("PauseMenuContainer");
    private Button PauseResumeButton => PauseMenuContainer.Q<Button>("ResumeButton");
    private Button PauseQuitButton => PauseMenuContainer.Q<Button>("QuitButton");

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
        AddPause();
        DrawBrain(brainSprite);
        SubscribeToEvents();
    }

    private void AddPause()
    {
        var gameOverClone = pauseTemplate.CloneTree();
        ApplyFullscreenTemplateStyles(gameOverClone);
        PauseMenuContainer.Add(gameOverClone);
        HidePause();
    }

    private void AddGameOver()
    {
        var gameOverClone = gameOverTemplate.CloneTree();
        ApplyFullscreenTemplateStyles(gameOverClone);
        GameOverContainer.Add(gameOverClone);
        GameOverContainer.style.display = DisplayStyle.None;
    }

    private static void ApplyFullscreenTemplateStyles(VisualElement template)
    {
        template.style.width = new StyleLength(Length.Percent(100));
        template.style.height = new StyleLength(Length.Percent(100));
    }

    private void DrawBrain(Sprite sprite)
    {
        BrainElement.style.backgroundImage = new StyleBackground(sprite);
    }

    private void ShowGameOver()
    {
        GameStateController.StopTime();
        GameOverContainer.style.display = DisplayStyle.Flex;
    }

    private void OnDamageTaken(object sender, DamageTakenEventArgs args)
    {
        DrawBrain(SelectBrainSprite(args.Insanity));
    }

    private void OnGamePaused()
    {
        ShowPause();
    }

    private void OnGameUnpaused()
    {
        if (!_gameStateController.isPaused)
            HidePause();
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
        GameStateController.ResumeTime();
    }

    private void OnQuitClick()
    {
        UnsubscribeFromEvents();
        SceneManager.LoadScene("Menu");
    }

    private void OnResumeClick()
    {
        HidePause();
        _gameStateController.OnUnpause();
    }

    private void ShowPause()
    {
        PauseMenuContainer.style.display = DisplayStyle.Flex;
    }

    private void HidePause()
    {
        PauseMenuContainer.style.display = DisplayStyle.None;
    }

    private void OnGameOver(object sender, GameOverEventArgs args)
    {
        ShowGameOver();
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

    private void SubscribeToEvents()
    {
        _gameStateController.OnDamageTaken += OnDamageTaken;
        _gameStateController.OnGameOver += OnGameOver;
        _gameStateController.OnGamePaused += OnGamePaused;
        _gameStateController.OnGameUnpaused += OnGameUnpaused;

        _pillStorage.OnPillCollect += OnPillCollect;
        GameOverRestartButton.clicked += OnRestartClick;
        GameOverQuitButton.clicked += OnQuitClick;
        PauseQuitButton.clicked += OnQuitClick;
        PauseResumeButton.clicked += OnResumeClick;
    }

    private void UnsubscribeFromEvents()
    {
        _gameStateController.OnDamageTaken -= OnDamageTaken;
        _gameStateController.OnGameOver -= OnGameOver;
        _gameStateController.OnGamePaused -= OnGamePaused;
        _gameStateController.OnGameUnpaused -= OnGameUnpaused;

        _pillStorage.OnPillCollect -= OnPillCollect;
        GameOverRestartButton.clicked -= OnRestartClick;
        GameOverQuitButton.clicked -= OnQuitClick;
        PauseQuitButton.clicked -= OnQuitClick;
        PauseResumeButton.clicked -= OnResumeClick;
    }
}