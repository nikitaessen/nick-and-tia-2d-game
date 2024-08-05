using System;
using Events;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameStateController : MonoBehaviour
{
    public int insanity;

    public bool isPaused;

    public delegate void DamageTakenHandler(object sender, DamageTakenEventArgs args);

    public delegate void GameOverHandler(object sender, GameOverEventArgs args);

    public event DamageTakenHandler OnDamageTaken;
    public event GameOverHandler OnGameOver;
    public event Action OnGamePaused;
    public event Action OnGameUnpaused;

    private PlayerMovement _playerMovement;
    private PlayerInput _playerInput;

    public void Initialize(PlayerMovement playerMovement, PlayerInput playerInput)
    {
        _playerMovement = playerMovement;
        _playerInput = playerInput;

        SubscribeToEvents();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        isPaused = true;
        SwitchInput(GameInputType.UI);
        OnGamePaused?.Invoke();
    }

    public void OnUnpause()
    {
        isPaused = false;
        SwitchInput(GameInputType.Player);
        OnGameUnpaused?.Invoke();
    }

    private void PlayerMovementOnDamageTaken()
    {
        insanity++;
        Debug.Log($"Damage taken: {insanity}");
        OnDamageTaken?.Invoke(this, new DamageTakenEventArgs(insanity));

        if (insanity > Constants.InsanityLimit)
        {
            OnGameOver?.Invoke(this, new GameOverEventArgs());
            SwitchInput(GameInputType.UI);
        }
    }

    public static void StopTime()
    {
        Time.timeScale = 0;
    }

    public static void ResumeTime()
    {
        Time.timeScale = 1;
    }

    private void SwitchInput(GameInputType inputType)
    {
        switch (inputType)
        {
            case GameInputType.UI:
                _playerInput.SwitchCurrentActionMap("UI");
                break;
            case GameInputType.Player:
                _playerInput.SwitchCurrentActionMap("Player");
                break;
            default:
                _playerInput.SwitchCurrentActionMap("Player");
                break;
        }
    }


    #region Unity Lifecycle

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    #endregion

    private void SubscribeToEvents()
    {
        _playerMovement.DamageTaken += PlayerMovementOnDamageTaken;
    }

    private void UnsubscribeFromEvents()
    {
        _playerMovement.DamageTaken -= PlayerMovementOnDamageTaken;
    }
}

public enum GameInputType
{
    Player,
    UI
}