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
    private MixerVolumeController _mixerVolumeController;
    private UIDocument _uiDocument;
    private PillStorage _pillStorage;

    private VisualElement BrainElement => _uiDocument.rootVisualElement.Q("Brain");

    private VisualElement GameOverContainer => _uiDocument.rootVisualElement.Q<VisualElement>("GameOverContainer");
    private Button GameOverRestartButton => GameOverContainer.Q<Button>("RestartButton");
    private Button GameOverQuitButton => GameOverContainer.Q<Button>("QuitButton");

    private VisualElement PauseMenuContainer => _uiDocument.rootVisualElement.Q("PauseMenuContainer");
    private Slider SoundsSlider => PauseMenuContainer.Q<Slider>("SoundsSlider");
    private Slider MusicSlider => PauseMenuContainer.Q<Slider>("MusicSlider");
    private Button PauseResumeButton => PauseMenuContainer.Q<Button>("ResumeButton");
    private Button PauseQuitButton => PauseMenuContainer.Q<Button>("QuitButton");

    private List<Label> Pills => _uiDocument.rootVisualElement.Q("PillsPanel").Query<Label>().ToList();

    public void Initialize(
        UIDocument uiDocument,
        GameStateController gameStateController,
        PillStorage pillStorage,
        MixerVolumeController mixerVolumeController)
    {
        _uiDocument = uiDocument;
        _gameStateController = gameStateController;
        _pillStorage = pillStorage;
        _mixerVolumeController = mixerVolumeController;

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

    private void OnSoundsSliderValueChanged(ChangeEvent<float> evt)
    {
        _mixerVolumeController.SetSoundsVolume(evt.newValue);
        SaveSoundSettings(evt.newValue);
    }

    private void OnMusicSliderValueChanged(ChangeEvent<float> evt)
    {
        _mixerVolumeController.SetMusicVolume(evt.newValue);
        SaveMusicSettings(evt.newValue);
    }

    private static void SaveSoundSettings(float value)
    {
        var settings = SettingsRepository.LoadSettings();
        SettingsRepository.SaveSettings(new GameSettings
        {
            SoundVolume = value,
            MusicVolume = settings.MusicVolume
        });
    }
    
    private static void SaveMusicSettings(float value)
    {
        var settings = SettingsRepository.LoadSettings();
        SettingsRepository.SaveSettings(new GameSettings
        {
            SoundVolume = settings.SoundVolume,
            MusicVolume = value
        });
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
        
        var settings = SettingsRepository.LoadSettings();
        SoundsSlider.RegisterValueChangedCallback(OnSoundsSliderValueChanged);
        SoundsSlider.value = settings.SoundVolume;
        MusicSlider.RegisterValueChangedCallback(OnMusicSliderValueChanged);
        MusicSlider.value = settings.MusicVolume;
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
        SoundsSlider.UnregisterValueChangedCallback(OnSoundsSliderValueChanged);
        MusicSlider.UnregisterValueChangedCallback(OnMusicSliderValueChanged);
    }
}