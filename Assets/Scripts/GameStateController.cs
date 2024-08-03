using Events;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    public int insanity;

    public delegate void DamageTakenHandler(object sender, DamageTakenEventArgs args);

    public delegate void GameOverHandler(object sender, GameOverEventArgs args);

    public event DamageTakenHandler OnDamageTaken;
    public event GameOverHandler OnGameOver;

    private PlayerMovement _playerMovement;

    public void Initialize(PlayerMovement playerMovement)
    {
        _playerMovement = playerMovement;

        SubscribeToEvents();
    }

    private void PlayerMovementOnDamageTaken()
    {
        insanity++;
        Debug.Log(insanity);
        OnDamageTaken?.Invoke(this, new DamageTakenEventArgs(insanity));

        if (insanity > Constants.InsanityLimit)
        {
            OnGameOver?.Invoke(this, new GameOverEventArgs());
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