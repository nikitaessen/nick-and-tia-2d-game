using Events;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    public int insanity = Constants.InsanityAmount;

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
        OnDamageTaken?.Invoke(this, new DamageTakenEventArgs(insanity));
    }


    #region Unity Lifecycle

    private void Update()
    {
        if (insanity > 3)
        {
            OnGameOver?.Invoke(this, new GameOverEventArgs());
        }
    }

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