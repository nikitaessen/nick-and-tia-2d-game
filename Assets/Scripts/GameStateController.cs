using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameStateController : MonoBehaviour
{
    public int insanity = 3;

    [SerializeField] private VisualTreeAsset sanityItemUi;

    private UIDocument _uiDocument;
    private PlayerMovement _playerMovement;

    private VisualElement InsanityPanel => _uiDocument.rootVisualElement.Q("InsanityPanel");
    private VisualElement GameOverOverlay => _uiDocument.rootVisualElement.Q("GameOver");
    private Button RestartButton => GameOverOverlay.Q<Button>("RestartButton");
    private Button QuitButton => GameOverOverlay.Q<Button>("QuitButton");

    public void Initialize(UIDocument uiDocument, PlayerMovement playerMovement)
    {
        _uiDocument = uiDocument;
        _playerMovement = playerMovement;

        SubscribeToEvents();
        FillInsanity();
    }

    private void PlayerMovementOnDamageTaken()
    {
        insanity++;
        DrawInsanityItem();
    }

    private void FillInsanity()
    {
        for (var i = 0; i < insanity; i++)
        {
            DrawInsanityItem();
        }

        Debug.Log("Log");
    }

    private void DrawInsanityItem()
    {
        var insanityItem = sanityItemUi.CloneTree();
        InsanityPanel.Add(insanityItem);
    }


    #region Unity Lifecycle

    private void Update()
    {
        if (insanity > 3)
        {
            ShowGameOver();
        }
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    #endregion


    private void ShowGameOver()
    {
        Time.timeScale = 0;
        GameOverOverlay.style.display = DisplayStyle.Flex;
    }

    private static void OnRestartClick()
    {
        SceneManager.LoadScene("Level");
        Time.timeScale = 1;
    }

    private static void OnQuitClick()
    {
        SceneManager.LoadScene("Menu");
    }

    private void SubscribeToEvents()
    {
        _playerMovement.DamageTaken += PlayerMovementOnDamageTaken;
        RestartButton.clicked += OnRestartClick;
        QuitButton.clicked += OnQuitClick;
    }

    private void UnsubscribeFromEvents()
    {
        _playerMovement.DamageTaken -= PlayerMovementOnDamageTaken;
        RestartButton.clicked -= OnRestartClick;
        QuitButton.clicked -= OnQuitClick;
    }
}